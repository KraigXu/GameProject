using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld.QuestGen
{
	
	public class Slate
	{
		
		// (get) Token: 0x060068A5 RID: 26789 RVA: 0x00248DF9 File Offset: 0x00246FF9
		public string CurrentPrefix
		{
			get
			{
				return this.prefix;
			}
		}

		
		public T Get<T>(string name, T defaultValue = default(T), bool isAbsoluteName = false)
		{
			T result;
			if (this.TryGet<T>(name, out result, isAbsoluteName))
			{
				return result;
			}
			return defaultValue;
		}

		
		public bool TryGet<T>(string name, out T var, bool isAbsoluteName = false)
		{
			if (name.NullOrEmpty())
			{
				var = default(T);
				return false;
			}
			if (!isAbsoluteName && !this.prefix.NullOrEmpty())
			{
				name = this.prefix + "/" + name;
			}
			name = QuestGenUtility.NormalizeVarPath(name);
			if (this.allowNonPrefixedLookup)
			{
				name = this.TryResolveFirstAvailableName(name);
			}
			object obj;
			if (!this.vars.TryGetValue(name, out obj))
			{
				var = default(T);
				return false;
			}
			if (obj == null)
			{
				var = default(T);
				return true;
			}
			if (obj is T)
			{
				var = (T)((object)obj);
				return true;
			}
			if (ConvertHelper.CanConvert<T>(obj))
			{
				var = ConvertHelper.Convert<T>(obj);
				return true;
			}
			Log.Error(string.Concat(new string[]
			{
				"Could not convert slate variable \"",
				name,
				"\" (",
				obj.GetType().Name,
				") to ",
				typeof(T).Name
			}), false);
			var = default(T);
			return false;
		}

		
		public void Set<T>(string name, T var, bool isAbsoluteName = false)
		{
			if (name.NullOrEmpty())
			{
				Log.Error("Tried to set a variable with null name. var=" + var.ToStringSafe<T>(), false);
				return;
			}
			if (!isAbsoluteName && !this.prefix.NullOrEmpty())
			{
				name = this.prefix + "/" + name;
			}
			name = QuestGenUtility.NormalizeVarPath(name);
			ISlateRef slateRef = var as ISlateRef;
			if (slateRef != null)
			{
				object value;
				slateRef.TryGetConvertedValue<object>(this, out value);
				this.vars[name] = value;
				return;
			}
			this.vars[name] = var;
		}

		
		public void SetIfNone<T>(string name, T var, bool isAbsoluteName = false)
		{
			if (this.Exists(name, isAbsoluteName))
			{
				return;
			}
			this.Set<T>(name, var, isAbsoluteName);
		}

		
		public bool Remove(string name, bool isAbsoluteName = false)
		{
			if (name.NullOrEmpty())
			{
				return false;
			}
			if (!isAbsoluteName && !this.prefix.NullOrEmpty())
			{
				name = this.prefix + "/" + name;
			}
			name = QuestGenUtility.NormalizeVarPath(name);
			return this.vars.Remove(name);
		}

		
		public bool Exists(string name, bool isAbsoluteName = false)
		{
			if (name.NullOrEmpty())
			{
				return false;
			}
			if (!isAbsoluteName && !this.prefix.NullOrEmpty())
			{
				name = this.prefix + "/" + name;
			}
			name = QuestGenUtility.NormalizeVarPath(name);
			if (this.allowNonPrefixedLookup)
			{
				name = this.TryResolveFirstAvailableName(name);
			}
			return this.vars.ContainsKey(name);
		}

		
		private string TryResolveFirstAvailableName(string nameWithPrefix)
		{
			if (nameWithPrefix == null)
			{
				return null;
			}
			nameWithPrefix = QuestGenUtility.NormalizeVarPath(nameWithPrefix);
			if (this.vars.ContainsKey(nameWithPrefix))
			{
				return nameWithPrefix;
			}
			int num = nameWithPrefix.LastIndexOf('/');
			if (num < 0)
			{
				return nameWithPrefix;
			}
			string text = nameWithPrefix.Substring(num + 1);
			string text2 = nameWithPrefix.Substring(0, num);
			string text3;
			for (;;)
			{
				text3 = text;
				if (!text2.NullOrEmpty())
				{
					text3 = text2 + "/" + text3;
				}
				if (this.vars.ContainsKey(text3))
				{
					break;
				}
				if (text2.NullOrEmpty())
				{
					return nameWithPrefix;
				}
				int num2 = text2.LastIndexOf('/');
				if (num2 >= 0)
				{
					text2 = text2.Substring(0, num2);
				}
				else
				{
					text2 = "";
				}
			}
			return text3;
		}

		
		public void PushPrefix(string newPrefix, bool allowNonPrefixedLookup = false)
		{
			if (newPrefix.NullOrEmpty())
			{
				Log.Error("Tried to push a null prefix.", false);
				newPrefix = "unnamed";
			}
			if (!this.prefix.NullOrEmpty())
			{
				this.prefix += "/";
			}
			this.prefix += newPrefix;
			this.prevAllowNonPrefixedLookupStack.Push(this.allowNonPrefixedLookup);
			if (allowNonPrefixedLookup)
			{
				this.allowNonPrefixedLookup = true;
			}
		}

		
		public void PopPrefix()
		{
			int num = this.prefix.LastIndexOf('/');
			if (num >= 0)
			{
				this.prefix = this.prefix.Substring(0, num);
			}
			else
			{
				this.prefix = "";
			}
			if (this.prevAllowNonPrefixedLookupStack.Count != 0)
			{
				this.allowNonPrefixedLookup = this.prevAllowNonPrefixedLookupStack.Pop();
			}
		}

		
		public Slate.VarRestoreInfo GetRestoreInfo(string name)
		{
			bool flag = this.allowNonPrefixedLookup;
			this.allowNonPrefixedLookup = false;
			Slate.VarRestoreInfo result;
			try
			{
				object value;
				bool exists = this.TryGet<object>(name, out value, false);
				result = new Slate.VarRestoreInfo(name, exists, value);
			}
			finally
			{
				this.allowNonPrefixedLookup = flag;
			}
			return result;
		}

		
		public void Restore(Slate.VarRestoreInfo varRestoreInfo)
		{
			if (varRestoreInfo.exists)
			{
				this.Set<object>(varRestoreInfo.name, varRestoreInfo.value, false);
				return;
			}
			this.Remove(varRestoreInfo.name, false);
		}

		
		public void SetAll(Slate otherSlate)
		{
			this.vars.Clear();
			foreach (KeyValuePair<string, object> keyValuePair in otherSlate.vars)
			{
				this.vars.Add(keyValuePair.Key, keyValuePair.Value);
			}
		}

		
		public void Reset()
		{
			this.vars.Clear();
		}

		
		public Slate DeepCopy()
		{
			Slate slate = new Slate();
			slate.prefix = this.prefix;
			foreach (KeyValuePair<string, object> keyValuePair in this.vars)
			{
				slate.vars.Add(keyValuePair.Key, keyValuePair.Value);
			}
			return slate;
		}

		
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (KeyValuePair<string, object> keyValuePair in from x in this.vars
			orderby x.Key
			select x)
			{
				if (stringBuilder.Length != 0)
				{
					stringBuilder.AppendLine();
				}
				string str = (keyValuePair.Value is IEnumerable && !(keyValuePair.Value is string)) ? ((IEnumerable)keyValuePair.Value).ToStringSafeEnumerable() : keyValuePair.Value.ToStringSafe<object>();
				stringBuilder.Append(keyValuePair.Key + "=" + str);
			}
			if (stringBuilder.Length == 0)
			{
				stringBuilder.Append("(none)");
			}
			return stringBuilder.ToString();
		}

		
		private Dictionary<string, object> vars = new Dictionary<string, object>();

		
		private string prefix = "";

		
		private bool allowNonPrefixedLookup;

		
		private Stack<bool> prevAllowNonPrefixedLookupStack = new Stack<bool>();

		
		public const char Separator = '/';

		
		public struct VarRestoreInfo
		{
			
			public VarRestoreInfo(string name, bool exists, object value)
			{
				this.name = name;
				this.exists = exists;
				this.value = value;
			}

			
			public string name;

			
			public bool exists;

			
			public object value;
		}
	}
}
