using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	
	public static class DefDatabase<T> where T : Def
	{
		
		// (get) Token: 0x0600042F RID: 1071 RVA: 0x00015989 File Offset: 0x00013B89
		public static IEnumerable<T> AllDefs
		{
			get
			{
				return DefDatabase<T>.defsList;
			}
		}

		
		// (get) Token: 0x06000430 RID: 1072 RVA: 0x00015989 File Offset: 0x00013B89
		public static List<T> AllDefsListForReading
		{
			get
			{
				return DefDatabase<T>.defsList;
			}
		}

		
		// (get) Token: 0x06000431 RID: 1073 RVA: 0x00015990 File Offset: 0x00013B90
		public static int DefCount
		{
			get
			{
				return DefDatabase<T>.defsList.Count;
			}
		}

		
		public static void AddAllInMods()
		{
			HashSet<string> hashSet = new HashSet<string>();
			foreach (ModContentPack modContentPack in (from m in LoadedModManager.RunningMods
			orderby m.OverwritePriority
			select m).ThenBy((ModContentPack x) => LoadedModManager.RunningModsListForReading.IndexOf(x)))
			{
				hashSet.Clear();
				foreach (T t in GenDefDatabase.DefsToGoInDatabase<T>(modContentPack))
				{
					if (!hashSet.Add(t.defName))
					{
						Log.Error(string.Concat(new object[]
						{
							"Mod ",
							modContentPack,
							" has multiple ",
							typeof(T),
							"s named ",
							t.defName,
							". Skipping."
						}), false);
					}
					else
					{
						//DefDatabase<T>.<AddAllInMods>g__AddDef|8_0(t, modContentPack.ToString());
					}
				}
			}
			foreach (T def in LoadedModManager.PatchedDefsForReading.OfType<T>())
			{
				//DefDatabase<T>.<AddAllInMods>g__AddDef|8_0(def, "Patches");
			}
		}

		
		public static void Add(IEnumerable<T> defs)
		{
			foreach (T def in defs)
			{
				DefDatabase<T>.Add(def);
			}
		}

		
		public static void Add(T def)
		{
			while (DefDatabase<T>.defsByName.ContainsKey(def.defName))
			{
				Log.Error(string.Concat(new object[]
				{
					"Adding duplicate ",
					typeof(T),
					" name: ",
					def.defName
				}), false);
				T t = def;
				t.defName += Mathf.RoundToInt(Rand.Value * 1000f);
			}
			DefDatabase<T>.defsList.Add(def);
			DefDatabase<T>.defsByName.Add(def.defName, def);
			if (DefDatabase<T>.defsList.Count > 65535)
			{
				Log.Error(string.Concat(new object[]
				{
					"Too many ",
					typeof(T),
					"; over ",
					ushort.MaxValue
				}), false);
			}
			def.index = (ushort)(DefDatabase<T>.defsList.Count - 1);
		}

		
		private static void Remove(T def)
		{
			DefDatabase<T>.defsByName.Remove(def.defName);
			DefDatabase<T>.defsList.Remove(def);
			DefDatabase<T>.SetIndices();
		}

		
		public static void Clear()
		{
			DefDatabase<T>.defsList.Clear();
			DefDatabase<T>.defsByName.Clear();
		}

		
		public static void ClearCachedData()
		{
			for (int i = 0; i < DefDatabase<T>.defsList.Count; i++)
			{
				DefDatabase<T>.defsList[i].ClearCachedData();
			}
		}

		
		public static void ResolveAllReferences(bool onlyExactlyMyType = true, bool parallel = false)
		{
			DeepProfiler.Start("SetIndices");
			try
			{
				DefDatabase<T>.SetIndices();
			}
			finally
			{
				DeepProfiler.End();
			}
			DeepProfiler.Start("ResolveAllReferences " + typeof(T).FullName);
			try
			{
				Action<T> action = delegate(T def)
				{
					if (onlyExactlyMyType && def.GetType() != typeof(T))
					{
						return;
					}
					DeepProfiler.Start("Resolver call");
					try
					{
						def.ResolveReferences();
					}
					catch (Exception ex)
					{
						Log.Error(string.Concat(new object[]
						{
							"Error while resolving references for def ",
							def,
							": ",
							ex
						}), false);
					}
					finally
					{
						DeepProfiler.End();
					}
				};
				if (parallel)
				{
					GenThreading.ParallelForEach<T>(DefDatabase<T>.defsList, action, -1);
				}
				else
				{
					for (int i = 0; i < DefDatabase<T>.defsList.Count; i++)
					{
						action(DefDatabase<T>.defsList[i]);
					}
				}
			}
			finally
			{
				DeepProfiler.End();
			}
			DeepProfiler.Start("SetIndices");
			try
			{
				DefDatabase<T>.SetIndices();
			}
			finally
			{
				DeepProfiler.End();
			}
		}

		
		private static void SetIndices()
		{
			for (int i = 0; i < DefDatabase<T>.defsList.Count; i++)
			{
				DefDatabase<T>.defsList[i].index = (ushort)i;
			}
		}

		
		public static void ErrorCheckAllDefs()
		{
			foreach (T t in DefDatabase<T>.AllDefs)
			{
				try
				{
					if (!t.ignoreConfigErrors)
					{
						foreach (string text in t.ConfigErrors())
						{
							Log.Error(string.Concat(new object[]
							{
								"Config error in ",
								t,
								": ",
								text
							}), false);
						}
					}
				}
				catch (Exception ex)
				{
					Log.Error(string.Concat(new object[]
					{
						"Exception in ConfigErrors() of ",
						t.defName,
						": ",
						ex
					}), false);
				}
			}
		}

		
		public static T GetNamed(string defName, bool errorOnFail = true)
		{
			if (errorOnFail)
			{
				T result;
				if (DefDatabase<T>.defsByName.TryGetValue(defName, out result))
				{
					return result;
				}
				Log.Error(string.Concat(new object[]
				{
					"Failed to find ",
					typeof(T),
					" named ",
					defName,
					". There are ",
					DefDatabase<T>.defsList.Count,
					" defs of this type loaded."
				}), false);
				return default(T);
			}
			else
			{
				T result2;
				if (DefDatabase<T>.defsByName.TryGetValue(defName, out result2))
				{
					return result2;
				}
				return default(T);
			}
		}

		
		public static T GetNamedSilentFail(string defName)
		{
			return DefDatabase<T>.GetNamed(defName, false);
		}

		
		public static T GetByShortHash(ushort shortHash)
		{
			for (int i = 0; i < DefDatabase<T>.defsList.Count; i++)
			{
				if (DefDatabase<T>.defsList[i].shortHash == shortHash)
				{
					return DefDatabase<T>.defsList[i];
				}
			}
			return default(T);
		}

		
		public static T GetRandom()
		{
			return DefDatabase<T>.defsList.RandomElement<T>();
		}

		
		private static List<T> defsList = new List<T>();

		
		private static Dictionary<string, T> defsByName = new Dictionary<string, T>();
	}
}
