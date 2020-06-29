using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Verse
{
	
	public class DebugActionsMods
	{
		
		[DebugAction("Mods", null, allowedGameStates = AllowedGameStates.Entry)]
		private static void LoadedFilesForMod()
		{
			//List<DebugMenuOption> list = new List<DebugMenuOption>();
			//using (List<ModContentPack>.Enumerator enumerator = LoadedModManager.RunningModsListForReading.GetEnumerator())
			//{
			//	while (enumerator.MoveNext())
			//	{
			//		DebugActionsMods.c__DisplayClass0_0 c__DisplayClass0_ = new DebugActionsMods.c__DisplayClass0_0();
			//		c__DisplayClass0_.mod = enumerator.Current;
			//		list.Add(new DebugMenuOption(c__DisplayClass0_.mod.Name, DebugMenuOptionMode.Action, delegate
			//		{
			//			ModMetaData metaData = ModLister.GetModWithIdentifier(c__DisplayClass0_.mod.PackageId, false);
			//			if (metaData.loadFolders != null && metaData.loadFolders.DefinedVersions().Count != 0)
			//			{
			//				Find.WindowStack.Add(new Dialog_DebugOptionListLister(from ver in metaData.loadFolders.DefinedVersions()
			//				select new DebugMenuOption(ver, DebugMenuOptionMode.Action, delegate
			//				{
			//					DebugActionsMods.c__DisplayClass0_0 cs$8__locals = c__DisplayClass0_;
			//					IEnumerable<LoadFolder> source = metaData.loadFolders.FoldersForVersion(ver);
			//					Func<LoadFolder, string> selector;
			//					if ((selector = c__DisplayClass0_.9__13) == null)
			//					{
			//						selector = (c__DisplayClass0_.9__13 = ((LoadFolder f) => Path.Combine(c__DisplayClass0_.mod.RootDir, f.folderName)));
			//					}
			//					cs$8__locals.<LoadedFilesForMod>g__ShowTable|1(source.Select(selector).Reverse<string>().ToList<string>());
			//				})));
			//				return;
			//			}
			//			c__DisplayClass0_.<LoadedFilesForMod>g__ShowTable|1(null);
			//		}));
			//	}
			//}
			//Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}
	}
}
