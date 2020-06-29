using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	
	public static class NamePlayerFactionDialogUtility
	{
		
		public static bool IsValidName(string s)
		{
			return s.Length != 0 && s.Length <= 64 && GenText.IsValidFilename(s) && !GrammarResolver.ContainsSpecialChars(s);
		}

		
		public static void Named(string s)
		{
			Faction.OfPlayer.Name = s;
			if (Find.GameInfo.permadeathMode)
			{
				string oldSavefileName = Find.GameInfo.permadeathModeUniqueName;
				string newSavefileName = PermadeathModeUtility.GeneratePermadeathSaveNameBasedOnPlayerInput(s, oldSavefileName);
				if (oldSavefileName != newSavefileName)
				{
					
					LongEventHandler.QueueLongEvent(delegate
					{
						Find.GameInfo.permadeathModeUniqueName = newSavefileName;
						Find.Autosaver.DoAutosave();
						IEnumerable<FileInfo> allSavedGameFiles = GenFilePaths.AllSavedGameFiles;
						Func<FileInfo, bool> predicate;
						if ((predicate=default ) == null)
						{
							predicate = ( ((FileInfo x) => Path.GetFileNameWithoutExtension(x.Name) == oldSavefileName));
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
