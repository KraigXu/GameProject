using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02000E5E RID: 3678
	public static class NamePlayerFactionDialogUtility
	{
		// Token: 0x0600592C RID: 22828 RVA: 0x001DC65D File Offset: 0x001DA85D
		public static bool IsValidName(string s)
		{
			return s.Length != 0 && s.Length <= 64 && GenText.IsValidFilename(s) && !GrammarResolver.ContainsSpecialChars(s);
		}

		// Token: 0x0600592D RID: 22829 RVA: 0x001DC68C File Offset: 0x001DA88C
		public static void Named(string s)
		{
			Faction.OfPlayer.Name = s;
			if (Find.GameInfo.permadeathMode)
			{
				string oldSavefileName = Find.GameInfo.permadeathModeUniqueName;
				string newSavefileName = PermadeathModeUtility.GeneratePermadeathSaveNameBasedOnPlayerInput(s, oldSavefileName);
				if (oldSavefileName != newSavefileName)
				{
					Func<FileInfo, bool> <>9__1;
					LongEventHandler.QueueLongEvent(delegate
					{
						Find.GameInfo.permadeathModeUniqueName = newSavefileName;
						Find.Autosaver.DoAutosave();
						IEnumerable<FileInfo> allSavedGameFiles = GenFilePaths.AllSavedGameFiles;
						Func<FileInfo, bool> predicate;
						if ((predicate = <>9__1) == null)
						{
							predicate = (<>9__1 = ((FileInfo x) => Path.GetFileNameWithoutExtension(x.Name) == oldSavefileName));
						}
						FileInfo fileInfo = allSavedGameFiles.FirstOrDefault(predicate);
						if (fileInfo != null)
						{
							fileInfo.Delete();
						}
					}, "Autosaving", false, null, true);
				}
			}
		}
	}
}
