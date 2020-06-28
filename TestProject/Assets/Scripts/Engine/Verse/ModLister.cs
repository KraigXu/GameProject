using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using RimWorld;
using Steamworks;
using Verse.Steam;

namespace Verse
{
	// Token: 0x020001F7 RID: 503
	public static class ModLister
	{
		// Token: 0x170002BB RID: 699
		// (get) Token: 0x06000E3E RID: 3646 RVA: 0x00051B1C File Offset: 0x0004FD1C
		public static IEnumerable<ModMetaData> AllInstalledMods
		{
			get
			{
				return ModLister.mods;
			}
		}

		// Token: 0x170002BC RID: 700
		// (get) Token: 0x06000E3F RID: 3647 RVA: 0x00051B24 File Offset: 0x0004FD24
		public static IEnumerable<DirectoryInfo> AllActiveModDirs
		{
			get
			{
				return from mod in ModLister.mods
				where mod.Active
				select mod.RootDir;
			}
		}

		// Token: 0x170002BD RID: 701
		// (get) Token: 0x06000E40 RID: 3648 RVA: 0x00051B80 File Offset: 0x0004FD80
		public static List<ExpansionDef> AllExpansions
		{
			get
			{
				if (ModLister.AllExpansionsCached.NullOrEmpty<ExpansionDef>())
				{
					ModLister.AllExpansionsCached = DefDatabase<ExpansionDef>.AllDefsListForReading.Where(delegate(ExpansionDef e)
					{
						ModMetaData modWithIdentifier = ModLister.GetModWithIdentifier(e.linkedMod, false);
						return modWithIdentifier == null || modWithIdentifier.Official;
					}).ToList<ExpansionDef>();
				}
				return ModLister.AllExpansionsCached;
			}
		}

		// Token: 0x170002BE RID: 702
		// (get) Token: 0x06000E41 RID: 3649 RVA: 0x00051BD1 File Offset: 0x0004FDD1
		public static bool RoyaltyInstalled
		{
			get
			{
				return ModLister.royaltyInstalled && !Prefs.SimulateNotOwningRoyalty;
			}
		}

		// Token: 0x170002BF RID: 703
		// (get) Token: 0x06000E42 RID: 3650 RVA: 0x00051BE4 File Offset: 0x0004FDE4
		public static bool ShouldLogIssues
		{
			get
			{
				return !ModLister.modListBuilt && !ModLister.nestedRebuildInProgress;
			}
		}

		// Token: 0x06000E43 RID: 3651 RVA: 0x00051BF7 File Offset: 0x0004FDF7
		static ModLister()
		{
			ModLister.RebuildModList();
			ModLister.modListBuilt = true;
		}

		// Token: 0x06000E44 RID: 3652 RVA: 0x00002681 File Offset: 0x00000881
		public static void EnsureInit()
		{
		}

		// Token: 0x06000E45 RID: 3653 RVA: 0x00051C10 File Offset: 0x0004FE10
		public static void RebuildModList()
		{
			ModLister.nestedRebuildInProgress = ModLister.rebuildingModList;
			ModLister.rebuildingModList = true;
			string s = "Rebuilding mods list";
			ModLister.mods.Clear();
			WorkshopItems.EnsureInit();
			s += "\nAdding official mods from content folder:";
			foreach (string localAbsPath in from d in new DirectoryInfo(GenFilePaths.OfficialModsFolderPath).GetDirectories()
			select d.FullName)
			{
				ModMetaData modMetaData = new ModMetaData(localAbsPath, true);
				if (ModLister.TryAddMod(modMetaData))
				{
					s = s + "\n  Adding " + modMetaData.ToStringLong();
				}
			}
			s += "\nAdding mods from mods folder:";
			foreach (string localAbsPath2 in from d in new DirectoryInfo(GenFilePaths.ModsFolderPath).GetDirectories()
			select d.FullName)
			{
				ModMetaData modMetaData2 = new ModMetaData(localAbsPath2, false);
				if (ModLister.TryAddMod(modMetaData2))
				{
					s = s + "\n  Adding " + modMetaData2.ToStringLong();
				}
			}
			s += "\nAdding mods from Steam:";
			foreach (WorkshopItem workshopItem in from it in WorkshopItems.AllSubscribedItems
			where it is WorkshopItem_Mod
			select it)
			{
				ModMetaData modMetaData3 = new ModMetaData(workshopItem);
				if (ModLister.TryAddMod(modMetaData3))
				{
					s = s + "\n  Adding " + modMetaData3.ToStringLong();
				}
			}
			s += "\nDeactivating not-installed mods:";
			ModsConfig.DeactivateNotInstalledMods(delegate(string log)
			{
				s = s + "\n   " + log;
			});
			if (Prefs.SimulateNotOwningRoyalty)
			{
				ModsConfig.SetActive(ModContentPack.RoyaltyModPackageId, false);
			}
			if (ModLister.mods.Count((ModMetaData m) => m.Active) == 0)
			{
				s += "\nThere are no active mods. Activating Core mod.";
				ModLister.mods.First((ModMetaData m) => m.IsCoreMod).Active = true;
			}
			ModLister.RecacheRoyaltyInstalled();
			if (Prefs.LogVerbose)
			{
				Log.Message(s, false);
			}
			ModLister.rebuildingModList = false;
			ModLister.nestedRebuildInProgress = false;
		}

		// Token: 0x06000E46 RID: 3654 RVA: 0x00051F04 File Offset: 0x00050104
		public static int InstalledModsListHash(bool activeOnly)
		{
			int num = 17;
			int num2 = 0;
			foreach (ModMetaData modMetaData in ModsConfig.ActiveModsInLoadOrder)
			{
				if (!activeOnly || ModsConfig.IsActive(modMetaData.PackageId))
				{
					num = num * 31 + modMetaData.GetHashCode();
					num = num * 31 + num2 * 2654241;
					num2++;
				}
			}
			return num;
		}

		// Token: 0x06000E47 RID: 3655 RVA: 0x00051F7C File Offset: 0x0005017C
		public static ModMetaData GetModWithIdentifier(string identifier, bool ignorePostfix = false)
		{
			for (int i = 0; i < ModLister.mods.Count; i++)
			{
				if (ModLister.mods[i].SamePackageId(identifier, ignorePostfix))
				{
					return ModLister.mods[i];
				}
			}
			return null;
		}

		// Token: 0x06000E48 RID: 3656 RVA: 0x00051FC0 File Offset: 0x000501C0
		public static ModMetaData GetActiveModWithIdentifier(string identifier)
		{
			for (int i = 0; i < ModLister.mods.Count; i++)
			{
				if (ModLister.mods[i].SamePackageId(identifier, true) && ModLister.mods[i].Active)
				{
					return ModLister.mods[i];
				}
			}
			return null;
		}

		// Token: 0x06000E49 RID: 3657 RVA: 0x00052018 File Offset: 0x00050218
		public static ExpansionDef GetExpansionWithIdentifier(string packageId)
		{
			for (int i = 0; i < ModLister.AllExpansions.Count; i++)
			{
				if (ModLister.AllExpansions[i].linkedMod == packageId)
				{
					return ModLister.AllExpansions[i];
				}
			}
			return null;
		}

		// Token: 0x06000E4A RID: 3658 RVA: 0x00052060 File Offset: 0x00050260
		public static bool HasActiveModWithName(string name)
		{
			for (int i = 0; i < ModLister.mods.Count; i++)
			{
				if (ModLister.mods[i].Active && ModLister.mods[i].Name == name)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000E4B RID: 3659 RVA: 0x000520B0 File Offset: 0x000502B0
		public static bool AnyFromListActive(List<string> mods)
		{
			using (List<string>.Enumerator enumerator = mods.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (ModLister.GetActiveModWithIdentifier(enumerator.Current) != null)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000E4C RID: 3660 RVA: 0x00052104 File Offset: 0x00050304
		private static void RecacheRoyaltyInstalled()
		{
			for (int i = 0; i < ModLister.mods.Count; i++)
			{
				if (ModLister.mods[i].SamePackageId(ModContentPack.RoyaltyModPackageId, false))
				{
					ModLister.royaltyInstalled = true;
					return;
				}
			}
			ModLister.royaltyInstalled = false;
		}

		// Token: 0x06000E4D RID: 3661 RVA: 0x0005214C File Offset: 0x0005034C
		private static bool TryAddMod(ModMetaData mod)
		{
			if (mod.Official && !mod.IsCoreMod && SteamManager.Initialized && mod.SteamAppId != 0)
			{
				bool flag = true;
				try
				{
					flag = SteamApps.BIsDlcInstalled(new AppId_t((uint)mod.SteamAppId));
				}
				catch (Exception arg)
				{
					Log.Error("Could not determine if a DLC is installed: " + arg, false);
				}
				if (!flag)
				{
					return false;
				}
			}
			ModMetaData modWithIdentifier = ModLister.GetModWithIdentifier(mod.PackageId, false);
			if (modWithIdentifier == null)
			{
				ModLister.mods.Add(mod);
				return true;
			}
			if (mod.RootDir.FullName != modWithIdentifier.RootDir.FullName)
			{
				if (mod.OnSteamWorkshop != modWithIdentifier.OnSteamWorkshop)
				{
					ModMetaData modMetaData = mod.OnSteamWorkshop ? mod : modWithIdentifier;
					if (!modMetaData.appendPackageIdSteamPostfix)
					{
						modMetaData.appendPackageIdSteamPostfix = true;
						return ModLister.TryAddMod(mod);
					}
				}
				Log.Error(string.Concat(new string[]
				{
					"Tried loading mod with the same packageId multiple times: ",
					mod.PackageIdPlayerFacing,
					". Ignoring the duplicates.\n",
					mod.RootDir.FullName,
					"\n",
					modWithIdentifier.RootDir.FullName
				}), false);
				return false;
			}
			return false;
		}

		// Token: 0x04000AC2 RID: 2754
		private static List<ModMetaData> mods = new List<ModMetaData>();

		// Token: 0x04000AC3 RID: 2755
		private static bool royaltyInstalled;

		// Token: 0x04000AC4 RID: 2756
		private static bool modListBuilt;

		// Token: 0x04000AC5 RID: 2757
		private static bool rebuildingModList;

		// Token: 0x04000AC6 RID: 2758
		private static bool nestedRebuildInProgress;

		// Token: 0x04000AC7 RID: 2759
		private static List<ExpansionDef> AllExpansionsCached;
	}
}
