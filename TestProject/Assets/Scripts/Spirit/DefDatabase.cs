using Spirit;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{

	public static class DefDatabase<T> where T : Def
	{
		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x0600042F RID: 1071 RVA: 0x00015989 File Offset: 0x00013B89
		public static IEnumerable<T> AllDefs
		{
			get
			{
				return DefDatabase<T>.defsList;
			}
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x06000430 RID: 1072 RVA: 0x00015989 File Offset: 0x00013B89
		public static List<T> AllDefsListForReading
		{
			get
			{
				return DefDatabase<T>.defsList;
			}
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x06000431 RID: 1073 RVA: 0x00015990 File Offset: 0x00013B90
		public static int DefCount
		{
			get
			{
				return DefDatabase<T>.defsList.Count;
			}
		}

		// Token: 0x06000432 RID: 1074 RVA: 0x0001599C File Offset: 0x00013B9C
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
						DefDatabase<T>.< AddAllInMods > g__AddDef | 8_0(t, modContentPack.ToString());
					}
				}
			}
			foreach (T def in LoadedModManager.PatchedDefsForReading.OfType<T>())
			{
				DefDatabase<T>.< AddAllInMods > g__AddDef | 8_0(def, "Patches");
			}
		}

		// Token: 0x06000433 RID: 1075 RVA: 0x00015B2C File Offset: 0x00013D2C
		public static void Add(IEnumerable<T> defs)
		{
			foreach (T def in defs)
			{
				DefDatabase<T>.Add(def);
			}
		}

		// Token: 0x06000434 RID: 1076 RVA: 0x00015B74 File Offset: 0x00013D74
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

		// Token: 0x06000435 RID: 1077 RVA: 0x00015C86 File Offset: 0x00013E86
		private static void Remove(T def)
		{
			DefDatabase<T>.defsByName.Remove(def.defName);
			DefDatabase<T>.defsList.Remove(def);
			DefDatabase<T>.SetIndices();
		}

		// Token: 0x06000436 RID: 1078 RVA: 0x00015CAF File Offset: 0x00013EAF
		public static void Clear()
		{
			DefDatabase<T>.defsList.Clear();
			DefDatabase<T>.defsByName.Clear();
		}

		// Token: 0x06000437 RID: 1079 RVA: 0x00015CC8 File Offset: 0x00013EC8
		public static void ClearCachedData()
		{
			for (int i = 0; i < DefDatabase<T>.defsList.Count; i++)
			{
				DefDatabase<T>.defsList[i].ClearCachedData();
			}
		}

		// Token: 0x06000438 RID: 1080 RVA: 0x00015D00 File Offset: 0x00013F00
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
				Action<T> action = delegate (T def)
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

		// Token: 0x06000439 RID: 1081 RVA: 0x00015DDC File Offset: 0x00013FDC
		private static void SetIndices()
		{
			for (int i = 0; i < DefDatabase<T>.defsList.Count; i++)
			{
				DefDatabase<T>.defsList[i].index = (ushort)i;
			}
		}

		// Token: 0x0600043A RID: 1082 RVA: 0x00015E18 File Offset: 0x00014018
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

		// Token: 0x0600043B RID: 1083 RVA: 0x00015F20 File Offset: 0x00014120
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

		// Token: 0x0600043C RID: 1084 RVA: 0x00015FB9 File Offset: 0x000141B9
		public static T GetNamedSilentFail(string defName)
		{
			return DefDatabase<T>.GetNamed(defName, false);
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x00015FC4 File Offset: 0x000141C4
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

		// Token: 0x0600043E RID: 1086 RVA: 0x00016013 File Offset: 0x00014213
		public static T GetRandom()
		{
			return DefDatabase<T>.defsList.RandomElement<T>();
		}

		// Token: 0x04000166 RID: 358
		private static List<T> defsList = new List<T>();

		// Token: 0x04000167 RID: 359
		private static Dictionary<string, T> defsByName = new Dictionary<string, T>();
	}
}
