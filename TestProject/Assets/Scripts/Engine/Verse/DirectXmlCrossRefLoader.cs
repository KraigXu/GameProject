using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;

namespace Verse
{
	// Token: 0x020002B4 RID: 692
	public static class DirectXmlCrossRefLoader
	{
		// Token: 0x1700041F RID: 1055
		// (get) Token: 0x060013B8 RID: 5048 RVA: 0x000713DD File Offset: 0x0006F5DD
		public static bool LoadingInProgress
		{
			get
			{
				return DirectXmlCrossRefLoader.wantedRefs.Count > 0;
			}
		}

		// Token: 0x060013B9 RID: 5049 RVA: 0x000713EC File Offset: 0x0006F5EC
		public static void RegisterObjectWantsCrossRef(object wanter, FieldInfo fi, string targetDefName, string mayRequireMod = null, Type assumeFieldType = null)
		{
			DeepProfiler.Start("RegisterObjectWantsCrossRef (object, FieldInfo, string)");
			try
			{
				DirectXmlCrossRefLoader.WantedRefForObject item = new DirectXmlCrossRefLoader.WantedRefForObject(wanter, fi, targetDefName, mayRequireMod, assumeFieldType);
				DirectXmlCrossRefLoader.wantedRefs.Add(item);
			}
			finally
			{
				DeepProfiler.End();
			}
		}

		// Token: 0x060013BA RID: 5050 RVA: 0x00071434 File Offset: 0x0006F634
		public static void RegisterObjectWantsCrossRef(object wanter, string fieldName, string targetDefName, string mayRequireMod = null, Type overrideFieldType = null)
		{
			DeepProfiler.Start("RegisterObjectWantsCrossRef (object,string,string)");
			try
			{
				DirectXmlCrossRefLoader.WantedRefForObject item = new DirectXmlCrossRefLoader.WantedRefForObject(wanter, wanter.GetType().GetField(fieldName), targetDefName, mayRequireMod, overrideFieldType);
				DirectXmlCrossRefLoader.wantedRefs.Add(item);
			}
			finally
			{
				DeepProfiler.End();
			}
		}

		// Token: 0x060013BB RID: 5051 RVA: 0x00071488 File Offset: 0x0006F688
		public static void RegisterObjectWantsCrossRef(object wanter, string fieldName, XmlNode parentNode, string mayRequireMod = null, Type overrideFieldType = null)
		{
			DeepProfiler.Start("RegisterObjectWantsCrossRef (object,string,XmlNode)");
			try
			{
				string text = mayRequireMod;
				if (mayRequireMod == null)
				{
					XmlAttributeCollection attributes = parentNode.Attributes;
					if (attributes == null)
					{
						text = null;
					}
					else
					{
						XmlAttribute xmlAttribute = attributes["MayRequire"];
						text = ((xmlAttribute != null) ? xmlAttribute.Value.ToLower() : null);
					}
				}
				string mayRequireMod2 = text;
				DirectXmlCrossRefLoader.WantedRefForObject item = new DirectXmlCrossRefLoader.WantedRefForObject(wanter, wanter.GetType().GetField(fieldName), parentNode.Name, mayRequireMod2, overrideFieldType);
				DirectXmlCrossRefLoader.wantedRefs.Add(item);
			}
			finally
			{
				DeepProfiler.End();
			}
		}

		// Token: 0x060013BC RID: 5052 RVA: 0x0007150C File Offset: 0x0006F70C
		public static void RegisterListWantsCrossRef<T>(List<T> wanterList, string targetDefName, object debugWanterInfo = null, string mayRequireMod = null)
		{
			DeepProfiler.Start("RegisterListWantsCrossRef");
			try
			{
				DirectXmlCrossRefLoader.WantedRef wantedRef;
				DirectXmlCrossRefLoader.WantedRefForList<T> wantedRefForList;
				if (!DirectXmlCrossRefLoader.wantedListDictRefs.TryGetValue(wanterList, out wantedRef))
				{
					wantedRefForList = new DirectXmlCrossRefLoader.WantedRefForList<T>(wanterList, debugWanterInfo);
					DirectXmlCrossRefLoader.wantedListDictRefs.Add(wanterList, wantedRefForList);
					DirectXmlCrossRefLoader.wantedRefs.Add(wantedRefForList);
				}
				else
				{
					wantedRefForList = (DirectXmlCrossRefLoader.WantedRefForList<T>)wantedRef;
				}
				wantedRefForList.AddWantedListEntry(targetDefName, mayRequireMod);
			}
			finally
			{
				DeepProfiler.End();
			}
		}

		// Token: 0x060013BD RID: 5053 RVA: 0x0007157C File Offset: 0x0006F77C
		public static void RegisterDictionaryWantsCrossRef<K, V>(Dictionary<K, V> wanterDict, XmlNode entryNode, object debugWanterInfo = null)
		{
			DeepProfiler.Start("RegisterDictionaryWantsCrossRef");
			try
			{
				DirectXmlCrossRefLoader.WantedRef wantedRef;
				DirectXmlCrossRefLoader.WantedRefForDictionary<K, V> wantedRefForDictionary;
				if (!DirectXmlCrossRefLoader.wantedListDictRefs.TryGetValue(wanterDict, out wantedRef))
				{
					wantedRefForDictionary = new DirectXmlCrossRefLoader.WantedRefForDictionary<K, V>(wanterDict, debugWanterInfo);
					DirectXmlCrossRefLoader.wantedRefs.Add(wantedRefForDictionary);
					DirectXmlCrossRefLoader.wantedListDictRefs.Add(wanterDict, wantedRefForDictionary);
				}
				else
				{
					wantedRefForDictionary = (DirectXmlCrossRefLoader.WantedRefForDictionary<K, V>)wantedRef;
				}
				wantedRefForDictionary.AddWantedDictEntry(entryNode);
			}
			finally
			{
				DeepProfiler.End();
			}
		}

		// Token: 0x060013BE RID: 5054 RVA: 0x000715EC File Offset: 0x0006F7EC
		public static T TryResolveDef<T>(string defName, FailMode failReportMode, object debugWanterInfo = null)
		{
			DeepProfiler.Start("TryResolveDef");
			T result;
			try
			{
				T t = (T)((object)GenDefDatabase.GetDefSilentFail(typeof(T), defName, true));
				if (t != null)
				{
					result = t;
				}
				else
				{
					if (failReportMode == FailMode.LogErrors)
					{
						string text = string.Concat(new object[]
						{
							"Could not resolve cross-reference to ",
							typeof(T),
							" named ",
							defName
						});
						if (debugWanterInfo != null)
						{
							text = text + " (wanter=" + debugWanterInfo.ToStringSafe<object>() + ")";
						}
						Log.Error(text, false);
					}
					result = default(T);
				}
			}
			finally
			{
				DeepProfiler.End();
			}
			return result;
		}

		// Token: 0x060013BF RID: 5055 RVA: 0x0007169C File Offset: 0x0006F89C
		public static void Clear()
		{
			DeepProfiler.Start("Clear");
			try
			{
				DirectXmlCrossRefLoader.wantedRefs.Clear();
				DirectXmlCrossRefLoader.wantedListDictRefs.Clear();
			}
			finally
			{
				DeepProfiler.End();
			}
		}

		// Token: 0x060013C0 RID: 5056 RVA: 0x000716E0 File Offset: 0x0006F8E0
		public static void ResolveAllWantedCrossReferences(FailMode failReportMode)
		{
			DeepProfiler.Start("ResolveAllWantedCrossReferences");
			try
			{
				HashSet<DirectXmlCrossRefLoader.WantedRef> resolvedRefs = new HashSet<DirectXmlCrossRefLoader.WantedRef>();
				object resolvedRefsLock = new object();
				DeepProfiler.enabled = false;
				GenThreading.ParallelForEach<DirectXmlCrossRefLoader.WantedRef>(DirectXmlCrossRefLoader.wantedRefs, delegate(DirectXmlCrossRefLoader.WantedRef wantedRef)
				{
					if (wantedRef.TryResolve(failReportMode))
					{
						object resolvedRefsLock = resolvedRefsLock;
						lock (resolvedRefsLock)
						{
							resolvedRefs.Add(wantedRef);
						}
					}
				}, -1);
				foreach (DirectXmlCrossRefLoader.WantedRef wantedRef2 in resolvedRefs)
				{
					wantedRef2.Apply();
				}
				DirectXmlCrossRefLoader.wantedRefs.RemoveAll((DirectXmlCrossRefLoader.WantedRef x) => resolvedRefs.Contains(x));
				DeepProfiler.enabled = true;
			}
			finally
			{
				DeepProfiler.End();
			}
		}

		// Token: 0x04000D4E RID: 3406
		private static List<DirectXmlCrossRefLoader.WantedRef> wantedRefs = new List<DirectXmlCrossRefLoader.WantedRef>();

		// Token: 0x04000D4F RID: 3407
		private static Dictionary<object, DirectXmlCrossRefLoader.WantedRef> wantedListDictRefs = new Dictionary<object, DirectXmlCrossRefLoader.WantedRef>();

		// Token: 0x0200147C RID: 5244
		private abstract class WantedRef
		{
			// Token: 0x06007AD4 RID: 31444
			public abstract bool TryResolve(FailMode failReportMode);

			// Token: 0x06007AD5 RID: 31445 RVA: 0x00002681 File Offset: 0x00000881
			public virtual void Apply()
			{
			}

			// Token: 0x04004DB0 RID: 19888
			public object wanter;
		}

		// Token: 0x0200147D RID: 5245
		private class WantedRefForObject : DirectXmlCrossRefLoader.WantedRef
		{
			// Token: 0x170014AD RID: 5293
			// (get) Token: 0x06007AD7 RID: 31447 RVA: 0x00299404 File Offset: 0x00297604
			private bool BadCrossRefAllowed
			{
				get
				{
					return !this.mayRequireMod.NullOrEmpty() && !ModsConfig.IsActive(this.mayRequireMod);
				}
			}

			// Token: 0x06007AD8 RID: 31448 RVA: 0x00299423 File Offset: 0x00297623
			public WantedRefForObject(object wanter, FieldInfo fi, string targetDefName, string mayRequireMod = null, Type overrideFieldType = null)
			{
				this.wanter = wanter;
				this.fi = fi;
				this.defName = targetDefName;
				this.mayRequireMod = mayRequireMod;
				this.overrideFieldType = overrideFieldType;
			}

			// Token: 0x06007AD9 RID: 31449 RVA: 0x00299450 File Offset: 0x00297650
			public override bool TryResolve(FailMode failReportMode)
			{
				if (this.fi == null)
				{
					Log.Error("Trying to resolve null field for def named " + this.defName.ToStringSafe<string>(), false);
					return false;
				}
				Type type = this.overrideFieldType ?? this.fi.FieldType;
				this.resolvedDef = GenDefDatabase.GetDefSilentFail(type, this.defName, true);
				if (this.resolvedDef == null)
				{
					if (failReportMode == FailMode.LogErrors && !this.BadCrossRefAllowed)
					{
						Log.Error(string.Concat(new object[]
						{
							"Could not resolve cross-reference: No ",
							type,
							" named ",
							this.defName.ToStringSafe<string>(),
							" found to give to ",
							this.wanter.GetType(),
							" ",
							this.wanter.ToStringSafe<object>()
						}), false);
					}
					return false;
				}
				SoundDef soundDef = this.resolvedDef as SoundDef;
				if (soundDef != null && soundDef.isUndefined)
				{
					Log.Warning(string.Concat(new object[]
					{
						"Could not resolve cross-reference: No ",
						type,
						" named ",
						this.defName.ToStringSafe<string>(),
						" found to give to ",
						this.wanter.GetType(),
						" ",
						this.wanter.ToStringSafe<object>(),
						" (using undefined sound instead)"
					}), false);
				}
				this.fi.SetValue(this.wanter, this.resolvedDef);
				return true;
			}

			// Token: 0x04004DB1 RID: 19889
			public FieldInfo fi;

			// Token: 0x04004DB2 RID: 19890
			public string defName;

			// Token: 0x04004DB3 RID: 19891
			public Def resolvedDef;

			// Token: 0x04004DB4 RID: 19892
			public string mayRequireMod;

			// Token: 0x04004DB5 RID: 19893
			public Type overrideFieldType;
		}

		// Token: 0x0200147E RID: 5246
		private class WantedRefForList<T> : DirectXmlCrossRefLoader.WantedRef
		{
			// Token: 0x06007ADA RID: 31450 RVA: 0x002995BD File Offset: 0x002977BD
			public WantedRefForList(object wanter, object debugWanterInfo)
			{
				this.wanter = wanter;
				this.debugWanterInfo = debugWanterInfo;
			}

			// Token: 0x06007ADB RID: 31451 RVA: 0x002995E0 File Offset: 0x002977E0
			public void AddWantedListEntry(string newTargetDefName, string mayRequireMod = null)
			{
				if (!mayRequireMod.NullOrEmpty() && this.mayRequireMods == null)
				{
					this.mayRequireMods = new List<string>();
					for (int i = 0; i < this.defNames.Count; i++)
					{
						this.mayRequireMods.Add(null);
					}
				}
				this.defNames.Add(newTargetDefName);
				if (this.mayRequireMods != null)
				{
					this.mayRequireMods.Add(mayRequireMod);
				}
			}

			// Token: 0x06007ADC RID: 31452 RVA: 0x0029964C File Offset: 0x0029784C
			public override bool TryResolve(FailMode failReportMode)
			{
				bool flag = false;
				for (int i = 0; i < this.defNames.Count; i++)
				{
					bool flag2 = this.mayRequireMods != null && i < this.mayRequireMods.Count && !this.mayRequireMods[i].NullOrEmpty() && !ModsConfig.IsActive(this.mayRequireMods[i]);
					T t = DirectXmlCrossRefLoader.TryResolveDef<T>(this.defNames[i], flag2 ? FailMode.Silent : failReportMode, this.debugWanterInfo);
					if (t != null)
					{
						((List<T>)this.wanter).Add(t);
						this.defNames.RemoveAt(i);
						if (this.mayRequireMods != null && i < this.mayRequireMods.Count)
						{
							this.mayRequireMods.RemoveAt(i);
						}
						i--;
					}
					else
					{
						flag = true;
					}
				}
				return !flag;
			}

			// Token: 0x04004DB6 RID: 19894
			private List<string> defNames = new List<string>();

			// Token: 0x04004DB7 RID: 19895
			private List<string> mayRequireMods;

			// Token: 0x04004DB8 RID: 19896
			private object debugWanterInfo;
		}

		// Token: 0x0200147F RID: 5247
		private class WantedRefForDictionary<K, V> : DirectXmlCrossRefLoader.WantedRef
		{
			// Token: 0x06007ADD RID: 31453 RVA: 0x0029972A File Offset: 0x0029792A
			public WantedRefForDictionary(object wanter, object debugWanterInfo)
			{
				this.wanter = wanter;
				this.debugWanterInfo = debugWanterInfo;
			}

			// Token: 0x06007ADE RID: 31454 RVA: 0x00299756 File Offset: 0x00297956
			public void AddWantedDictEntry(XmlNode entryNode)
			{
				this.wantedDictRefs.Add(entryNode);
			}

			// Token: 0x06007ADF RID: 31455 RVA: 0x00299764 File Offset: 0x00297964
			public override bool TryResolve(FailMode failReportMode)
			{
				failReportMode = FailMode.LogErrors;
				bool flag = typeof(Def).IsAssignableFrom(typeof(K));
				bool flag2 = typeof(Def).IsAssignableFrom(typeof(V));
				foreach (XmlNode xmlNode in this.wantedDictRefs)
				{
					XmlNode xmlNode2 = xmlNode["key"];
					XmlNode xmlNode3 = xmlNode["value"];
					object first;
					if (flag)
					{
						first = DirectXmlCrossRefLoader.TryResolveDef<K>(xmlNode2.InnerText, failReportMode, this.debugWanterInfo);
					}
					else
					{
						first = xmlNode2;
					}
					object second;
					if (flag2)
					{
						second = DirectXmlCrossRefLoader.TryResolveDef<V>(xmlNode3.InnerText, failReportMode, this.debugWanterInfo);
					}
					else
					{
						second = xmlNode3;
					}
					this.makingData.Add(new Pair<object, object>(first, second));
				}
				return true;
			}

			// Token: 0x06007AE0 RID: 31456 RVA: 0x00299858 File Offset: 0x00297A58
			public override void Apply()
			{
				Dictionary<K, V> dictionary = (Dictionary<K, V>)this.wanter;
				dictionary.Clear();
				foreach (Pair<object, object> pair in this.makingData)
				{
					try
					{
						object obj = pair.First;
						object obj2 = pair.Second;
						if (obj is XmlNode)
						{
							obj = DirectXmlToObject.ObjectFromXml<K>(obj as XmlNode, true);
						}
						if (obj2 is XmlNode)
						{
							obj2 = DirectXmlToObject.ObjectFromXml<V>(obj2 as XmlNode, true);
						}
						dictionary.Add((K)((object)obj), (V)((object)obj2));
					}
					catch
					{
						Log.Error(string.Concat(new object[]
						{
							"Failed to load key/value pair: ",
							pair.First,
							", ",
							pair.Second
						}), false);
					}
				}
			}

			// Token: 0x04004DB9 RID: 19897
			private List<XmlNode> wantedDictRefs = new List<XmlNode>();

			// Token: 0x04004DBA RID: 19898
			private object debugWanterInfo;

			// Token: 0x04004DBB RID: 19899
			private List<Pair<object, object>> makingData = new List<Pair<object, object>>();
		}
	}
}
