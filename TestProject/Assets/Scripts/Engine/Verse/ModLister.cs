using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using RimWorld;
using Steamworks;
using Verse.Steam;

namespace Verse
{

    public static class ModLister
    {

        private static List<ModMetaData> mods = new List<ModMetaData>();
        private static bool royaltyInstalled;
        private static bool modListBuilt;
        private static bool rebuildingModList;
        private static bool nestedRebuildInProgress;
        private static List<ExpansionDef> AllExpansionsCached;

        public static IEnumerable<ModMetaData> AllInstalledMods
        {
            get
            {
                return ModLister.mods;
            }
        }



        public static IEnumerable<DirectoryInfo> AllActiveModDirs
        {
            get
            {
                return from mod in ModLister.mods
                       where mod.Active
                       select mod.RootDir;
            }
        }



        public static List<ExpansionDef> AllExpansions
        {
            get
            {
                if (ModLister.AllExpansionsCached.NullOrEmpty<ExpansionDef>())
                {
                    ModLister.AllExpansionsCached = DefDatabase<ExpansionDef>.AllDefsListForReading.Where(delegate (ExpansionDef e)
                    {
                        ModMetaData modWithIdentifier = ModLister.GetModWithIdentifier(e.linkedMod, false);
                        return modWithIdentifier == null || modWithIdentifier.Official;
                    }).ToList<ExpansionDef>();
                }
                return ModLister.AllExpansionsCached;
            }
        }



        public static bool RoyaltyInstalled
        {
            get
            {
                return ModLister.royaltyInstalled && !Prefs.SimulateNotOwningRoyalty;
            }
        }



        public static bool ShouldLogIssues
        {
            get
            {
                return !ModLister.modListBuilt && !ModLister.nestedRebuildInProgress;
            }
        }


        static ModLister()
        {
            ModLister.RebuildModList();
            ModLister.modListBuilt = true;
        }


        public static void EnsureInit()
        {
        }

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
            ModsConfig.DeactivateNotInstalledMods(delegate (string log)
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


        public static bool AnyFromListActive(List<string> mods)
        {
            List<string>.Enumerator enumerator = mods.GetEnumerator();
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



    }
}
