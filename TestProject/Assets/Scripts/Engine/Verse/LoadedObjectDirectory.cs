using System;
using System.Collections.Generic;

namespace Verse
{
	
	public class LoadedObjectDirectory
	{
		
		public void Clear()
		{
			this.allObjectsByLoadID.Clear();
		}

		
		public void RegisterLoaded(ILoadReferenceable reffable)
		{
			if (Prefs.DevMode)
			{
				string text = "[excepted]";
				try
				{
					text = reffable.GetUniqueLoadID();
				}
				catch (Exception)
				{
				}
				string text2 = "[excepted]";
				try
				{
					text2 = reffable.ToString();
				}
				catch (Exception)
				{
				}
				ILoadReferenceable loadReferenceable;
				if (this.allObjectsByLoadID.TryGetValue(text, out loadReferenceable))
				{
					string text3 = "";
					Log.Error(string.Concat(new object[]
					{
						"Cannot register ",
						reffable.GetType(),
						" ",
						text2,
						", (id=",
						text,
						" in loaded object directory. Id already used by ",
						loadReferenceable.GetType(),
						" ",
						loadReferenceable.ToStringSafe<ILoadReferenceable>(),
						".",
						text3
					}), false);
					return;
				}
			}
			try
			{
				this.allObjectsByLoadID.Add(reffable.GetUniqueLoadID(), reffable);
			}
			catch (Exception ex)
			{
				string text4 = "[excepted]";
				try
				{
					text4 = reffable.GetUniqueLoadID();
				}
				catch (Exception)
				{
				}
				string text5 = "[excepted]";
				try
				{
					text5 = reffable.ToString();
				}
				catch (Exception)
				{
				}
				Log.Error(string.Concat(new object[]
				{
					"Exception registering ",
					reffable.GetType(),
					" ",
					text5,
					" in loaded object directory with unique load ID ",
					text4,
					": ",
					ex
				}), false);
			}
		}

		
		public T ObjectWithLoadID<T>(string loadID)
		{
			if (loadID.NullOrEmpty() || loadID == "null")
			{
				T result = default(T);
				return result;
			}
			ILoadReferenceable loadReferenceable;
			if (this.allObjectsByLoadID.TryGetValue(loadID, out loadReferenceable))
			{
				if (loadReferenceable == null)
				{
					T result = default(T);
					return result;
				}
				try
				{
					return (T)((object)loadReferenceable);
				}
				catch (Exception ex)
				{
					Log.Error(string.Concat(new object[]
					{
						"Exception getting object with load id ",
						loadID,
						" of type ",
						typeof(T),
						". What we loaded was ",
						loadReferenceable.ToStringSafe<ILoadReferenceable>(),
						". Exception:\n",
						ex
					}), false);
					return default(T);
				}
			}
			Log.Warning(string.Concat(new object[]
			{
				"Could not resolve reference to object with loadID ",
				loadID,
				" of type ",
				typeof(T),
				". Was it compressed away, destroyed, had no ID number, or not saved/loaded right? curParent=",
				Scribe.loader.curParent.ToStringSafe<IExposable>(),
				" curPathRelToParent=",
				Scribe.loader.curPathRelToParent
			}), false);
			return default(T);
		}

		
		private Dictionary<string, ILoadReferenceable> allObjectsByLoadID = new Dictionary<string, ILoadReferenceable>();
	}
}
