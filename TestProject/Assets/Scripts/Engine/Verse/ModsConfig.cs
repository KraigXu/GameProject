using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	
	public static class ModsConfig
	{
		private static ModsConfig.ModsConfigData data;


		private static bool royaltyActive;


		private static HashSet<string> activeModsHashSet = new HashSet<string>();


		private static List<ModMetaData> activeModsInLoadOrderCached = new List<ModMetaData>();


		private static bool activeModsInLoadOrderCachedDirty;


		private class ModsConfigData
		{

			[LoadAlias("buildNumber")]
			public string version;
			public List<string> activeMods = new List<string>();
			public List<string> knownExpansions = new List<string>();
		}

		public static IEnumerable<ModMetaData> ActiveModsInLoadOrder
		{
			get
			{
				ModLister.EnsureInit();
				if (ModsConfig.activeModsInLoadOrderCachedDirty)
				{
					ModsConfig.activeModsInLoadOrderCached.Clear();
					for (int i = 0; i < ModsConfig.data.activeMods.Count; i++)
					{
						ModsConfig.activeModsInLoadOrderCached.Add(ModLister.GetModWithIdentifier(ModsConfig.data.activeMods[i], false));
					}
					ModsConfig.activeModsInLoadOrderCachedDirty = false;
				}
				return ModsConfig.activeModsInLoadOrderCached;
			}
		}

		
		
		public static bool RoyaltyActive
		{
			get
			{
				return ModsConfig.royaltyActive;
			}
		}

		
		static ModsConfig()
		{
			bool flag = false;
			bool flag2 = false;
			ModsConfig.data = DirectXmlLoader.ItemFromXmlFile<ModsConfig.ModsConfigData>(GenFilePaths.ModsConfigFilePath, true);
			if (ModsConfig.data.version != null)
			{
				bool flag3 = false;
				int num2;
				if (ModsConfig.data.version.Contains("."))
				{
					int num = VersionControl.MinorFromVersionString(ModsConfig.data.version);
					if (VersionControl.MajorFromVersionString(ModsConfig.data.version) != VersionControl.CurrentMajor || num != VersionControl.CurrentMinor)
					{
						flag3 = true;
					}
				}
				else if (ModsConfig.data.version.Length > 0 && ModsConfig.data.version.All((char x) => char.IsNumber(x)) && int.TryParse(ModsConfig.data.version, out num2) && num2 <= 2009)
				{
					flag3 = true;
				}
				if (flag3)
				{
					Log.Message(string.Concat(new string[]
					{
						"Mods config data is from version ",
						ModsConfig.data.version,
						" while we are running ",
						VersionControl.CurrentVersionStringWithRev,
						". Resetting."
					}), false);
					ModsConfig.data = new ModsConfig.ModsConfigData();
					flag = true;
				}
			}
			for (int i = 0; i < ModsConfig.data.activeMods.Count; i++)
			{
				string packageId = ModsConfig.data.activeMods[i];
				if (ModLister.GetModWithIdentifier(packageId, false) == null)
				{
					ModMetaData modMetaData = ModLister.AllInstalledMods.FirstOrDefault((ModMetaData m) => m.FolderName == packageId);
					if (modMetaData != null)
					{
						ModsConfig.data.activeMods[i] = modMetaData.PackageId;
						flag2 = true;
					}
					string text;
					if (ModsConfig.TryGetPackageIdWithoutExtraSteamPostfix(packageId, out text) && ModLister.GetModWithIdentifier(text, false) != null)
					{
						ModsConfig.data.activeMods[i] = text;
					}
				}
			}
			HashSet<string> hashSet = new HashSet<string>();
			foreach (ModMetaData modMetaData2 in ModLister.AllInstalledMods)
			{
				if (modMetaData2.Active)
				{
					if (hashSet.Contains(modMetaData2.PackageIdNonUnique))
					{
						modMetaData2.Active = false;
						Debug.LogWarning("There was more than one enabled instance of mod with PackageID: " + modMetaData2.PackageIdNonUnique + ". Disabling the duplicates.");
						continue;
					}
					hashSet.Add(modMetaData2.PackageIdNonUnique);
				}
				if (!modMetaData2.IsCoreMod && modMetaData2.Official && ModsConfig.IsExpansionNew(modMetaData2.PackageId))
				{
					ModsConfig.SetActive(modMetaData2.PackageId, true);
					ModsConfig.AddKnownExpansion(modMetaData2.PackageId);
					flag2 = true;
				}
			}
			if (!File.Exists(GenFilePaths.ModsConfigFilePath) || flag)
			{
				ModsConfig.Reset();
			}
			else if (flag2)
			{
				ModsConfig.Save();
			}
			ModsConfig.RecacheActiveMods();
		}

		
		public static bool TryGetPackageIdWithoutExtraSteamPostfix(string packageId, out string nonSteamPackageId)
		{
			if (packageId.EndsWith(ModMetaData.SteamModPostfix))
			{
				nonSteamPackageId = packageId.Substring(0, packageId.Length - ModMetaData.SteamModPostfix.Length);
				return true;
			}
			nonSteamPackageId = null;
			return false;
		}

		
		public static void DeactivateNotInstalledMods(Action<string> logCallback = null)
		{
			for (int i = ModsConfig.data.activeMods.Count - 1; i >= 0; i--)
			{
				ModMetaData modWithIdentifier = ModLister.GetModWithIdentifier(ModsConfig.data.activeMods[i], false);
				string identifier;
				if (modWithIdentifier == null && ModsConfig.TryGetPackageIdWithoutExtraSteamPostfix(ModsConfig.data.activeMods[i], out identifier))
				{
					modWithIdentifier = ModLister.GetModWithIdentifier(identifier, false);
				}
				if (modWithIdentifier == null)
				{
					if (logCallback != null)
					{
						logCallback("Deactivating " + ModsConfig.data.activeMods[i]);
					}
					ModsConfig.data.activeMods.RemoveAt(i);
				}
			}
			ModsConfig.RecacheActiveMods();
		}

		
		public static void Reset()
		{
			ModsConfig.data.activeMods.Clear();
			ModsConfig.data.activeMods.Add(ModContentPack.CoreModPackageId);
			foreach (ModMetaData modMetaData in ModLister.AllInstalledMods)
			{
				if (modMetaData.Official && !modMetaData.IsCoreMod && modMetaData.VersionCompatible)
				{
					ModsConfig.data.activeMods.Add(modMetaData.PackageId);
				}
			}
			ModsConfig.Save();
			ModsConfig.RecacheActiveMods();
		}

		
		public static void Reorder(int modIndex, int newIndex)
		{
			if (modIndex == newIndex)
			{
				return;
			}
			ModsConfig.data.activeMods.Insert(newIndex, ModsConfig.data.activeMods[modIndex]);
			ModsConfig.data.activeMods.RemoveAt((modIndex < newIndex) ? modIndex : (modIndex + 1));
			ModsConfig.activeModsInLoadOrderCachedDirty = true;
		}

		
		public static void Reorder(List<int> newIndices)
		{
			List<string> list = new List<string>();
			foreach (int index in newIndices)
			{
				list.Add(ModsConfig.data.activeMods[index]);
			}
			ModsConfig.data.activeMods = list;
			ModsConfig.activeModsInLoadOrderCachedDirty = true;
		}

		
		public static bool IsActive(ModMetaData mod)
		{
			return ModsConfig.IsActive(mod.PackageId);
		}

		
		public static bool IsActive(string id)
		{
			return ModsConfig.activeModsHashSet.Contains(id.ToLower());
		}

		
		public static void SetActive(ModMetaData mod, bool active)
		{
			ModsConfig.SetActive(mod.PackageId, active);
		}

		
		public static void SetActive(string modIdentifier, bool active)
		{
			string item = modIdentifier.ToLower();
			if (active)
			{
				if (!ModsConfig.data.activeMods.Contains(item))
				{
					ModsConfig.data.activeMods.Add(item);
				}
			}
			else if (ModsConfig.data.activeMods.Contains(item))
			{
				ModsConfig.data.activeMods.Remove(item);
			}
			ModsConfig.RecacheActiveMods();
		}

		
		public static void SetActiveToList(List<string> mods)
		{
			ModsConfig.data.activeMods = (from mod in mods
			where ModLister.GetModWithIdentifier(mod, false) != null
			select mod).ToList<string>();
			ModsConfig.RecacheActiveMods();
		}

		
		public static bool IsExpansionNew(string id)
		{
			return !ModsConfig.data.knownExpansions.Contains(id.ToLower());
		}

		
		public static void AddKnownExpansion(string id)
		{
			if (ModsConfig.IsExpansionNew(id))
			{
				ModsConfig.data.knownExpansions.Add(id.ToLower());
			}
		}

		
		public static void Save()
		{
			ModsConfig.data.version = VersionControl.CurrentVersionStringWithRev;
			DirectXmlSaver.SaveDataObject(ModsConfig.data, GenFilePaths.ModsConfigFilePath);
		}

		
		public static void SaveFromList(List<string> mods)
		{
			DirectXmlSaver.SaveDataObject(new ModsConfig.ModsConfigData
			{
				version = VersionControl.CurrentVersionStringWithRev,
				activeMods = mods,
				knownExpansions = ModsConfig.data.knownExpansions
			}, GenFilePaths.ModsConfigFilePath);
		}

		
		public static void RestartFromChangedMods()
		{
			Find.WindowStack.Add(new Dialog_MessageBox("ModsChanged".Translate(), null, delegate
			{
				GenCommandLine.Restart();
			}, null, null, null, false, null, null));
		}

		
		public static List<string> GetModWarnings()
		{
			List<string> list = new List<string>();
			List<ModMetaData> mods = ModsConfig.ActiveModsInLoadOrder.ToList<ModMetaData>();
			for (int i = 0; i < mods.Count; i++)
			{
				int index = i;
				ModMetaData modMetaData = mods[index];
				StringBuilder stringBuilder = new StringBuilder("");
				for (int j = 0; j < mods.Count; j++)
				{
					if (i != j && mods[j].PackageId != "" && mods[j].SamePackageId(mods[i].PackageId, false))
					{
						stringBuilder.AppendLine("ModWithSameIdAlreadyActive".Translate(mods[j].Name));
					}
				}
				List<string> list2 = ModsConfig.FindConflicts(mods, modMetaData.IncompatibleWith, null);
				if (list2.Any<string>())
				{
					stringBuilder.AppendLine("ModIncompatibleWithTip".Translate(list2.ToCommaList(true)));
				}
				List<string> list3 = ModsConfig.FindConflicts(mods, modMetaData.LoadBefore, (ModMetaData beforeMod) => mods.IndexOf(beforeMod) < index);
				if (list3.Any<string>())
				{
					stringBuilder.AppendLine("ModMustLoadBefore".Translate(list3.ToCommaList(true)));
				}
				List<string> list4 = ModsConfig.FindConflicts(mods, modMetaData.LoadAfter, (ModMetaData afterMod) => mods.IndexOf(afterMod) > index);
				if (list4.Any<string>())
				{
					stringBuilder.AppendLine("ModMustLoadAfter".Translate(list4.ToCommaList(true)));
				}
				if (modMetaData.Dependencies.Any<ModDependency>())
				{
					List<string> list5 = modMetaData.UnsatisfiedDependencies();
					if (list5.Any<string>())
					{
						stringBuilder.AppendLine("ModUnsatisfiedDependency".Translate(list5.ToCommaList(true)));
					}
				}
				list.Add(stringBuilder.ToString().TrimEndNewlines());
			}
			return list;
		}

		
		public static bool ModHasAnyOrderingIssues(ModMetaData mod)
		{
			List<ModMetaData> mods = ModsConfig.ActiveModsInLoadOrder.ToList<ModMetaData>();
			int index = mods.IndexOf(mod);
			return index != -1 && (ModsConfig.FindConflicts(mods, mod.LoadBefore, (ModMetaData beforeMod) => mods.IndexOf(beforeMod) < index).Count > 0 || ModsConfig.FindConflicts(mods, mod.LoadAfter, (ModMetaData afterMod) => mods.IndexOf(afterMod) > index).Count > 0);
		}

		
		private static List<string> FindConflicts(List<ModMetaData> allMods, List<string> modsToCheck, Func<ModMetaData, bool> predicate)
		{
			List<string> list = new List<string>();
			List<string>.Enumerator enumerator = modsToCheck.GetEnumerator();
			{
				while (enumerator.MoveNext())
				{
					string modId = enumerator.Current;
					ModMetaData modMetaData = allMods.FirstOrDefault((ModMetaData m) => m.SamePackageId(modId, true));
					if (modMetaData != null && (predicate == null || predicate(modMetaData)))
					{
						list.Add(modMetaData.Name);
					}
				}
			}
			return list;
		}

		
		public static void TrySortMods()
		{
			List<ModMetaData> list = ModsConfig.ActiveModsInLoadOrder.ToList<ModMetaData>();
			DirectedAcyclicGraph directedAcyclicGraph = new DirectedAcyclicGraph(list.Count);
			for (int i = 0; i < list.Count; i++)
			{
				ModMetaData modMetaData = list[i];
				List<string>.Enumerator enumerator = modMetaData.LoadBefore.GetEnumerator();
				{
					while (enumerator.MoveNext())
					{
						string before = enumerator.Current;
						ModMetaData modMetaData2 = list.FirstOrDefault((ModMetaData m) => m.SamePackageId(before, true));
						if (modMetaData2 != null)
						{
							directedAcyclicGraph.AddEdge(list.IndexOf(modMetaData2), i);
						}
					}
				}
				List<string>.Enumerator enumerator1 = modMetaData.LoadAfter.GetEnumerator();
				{
					while (enumerator1.MoveNext())
					{
						string after = enumerator1.Current;
						ModMetaData modMetaData3 = list.FirstOrDefault((ModMetaData m) => m.SamePackageId(after, true));
						if (modMetaData3 != null)
						{
							directedAcyclicGraph.AddEdge(i, list.IndexOf(modMetaData3));
						}
					}
				}
			}
			int num = directedAcyclicGraph.FindCycle();
			if (num != -1)
			{
				Find.WindowStack.Add(new Dialog_MessageBox("ModCyclicDependency".Translate(list[num].Name), null, null, null, null, null, false, null, null));
				return;
			}
			ModsConfig.Reorder(directedAcyclicGraph.TopologicalSort());
		}

		
		private static void RecacheActiveMods()
		{
			ModsConfig.activeModsHashSet.Clear();
			foreach (string item in ModsConfig.data.activeMods)
			{
				ModsConfig.activeModsHashSet.Add(item);
			}
			ModsConfig.royaltyActive = ModsConfig.IsActive(ModContentPack.RoyaltyModPackageId);
			ModsConfig.activeModsInLoadOrderCachedDirty = true;
		}

		

	}
}
