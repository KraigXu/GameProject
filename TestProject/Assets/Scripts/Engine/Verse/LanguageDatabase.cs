using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using RimWorld.IO;
using Steamworks;
using UnityEngine;
using Verse.Steam;

namespace Verse
{
	// Token: 0x0200012E RID: 302
	public static class LanguageDatabase
	{
		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x06000886 RID: 2182 RVA: 0x0002BE26 File Offset: 0x0002A026
		public static IEnumerable<LoadedLanguage> AllLoadedLanguages
		{
			get
			{
				return LanguageDatabase.languages;
			}
		}

		// Token: 0x06000887 RID: 2183 RVA: 0x0002BE2D File Offset: 0x0002A02D
		public static void SelectLanguage(LoadedLanguage lang)
		{
			Prefs.LangFolderName = lang.folderName;
			LongEventHandler.QueueLongEvent(delegate
			{
				PlayDataLoader.ClearAllPlayData();
				PlayDataLoader.LoadAllPlayData(false);
			}, "LoadingLongEvent", true, null, true);
		}

		// Token: 0x06000888 RID: 2184 RVA: 0x0002BE66 File Offset: 0x0002A066
		public static void Clear()
		{
			LanguageDatabase.languages.Clear();
			LanguageDatabase.activeLanguage = null;
		}

		// Token: 0x06000889 RID: 2185 RVA: 0x0002BE78 File Offset: 0x0002A078
		public static void InitAllMetadata()
		{
			foreach (ModContentPack modContentPack in LoadedModManager.RunningMods)
			{
				HashSet<string> hashSet = new HashSet<string>();
				foreach (string path in modContentPack.foldersToLoadDescendingOrder)
				{
					string text = Path.Combine(path, "Languages");
					if (new DirectoryInfo(text).Exists)
					{
						foreach (VirtualDirectory virtualDirectory in AbstractFilesystem.GetDirectories(text, "*", SearchOption.TopDirectoryOnly, false))
						{
							if (!virtualDirectory.FullPath.StartsWith(text))
							{
								Log.Error("Failed to get a relative path for a file: " + virtualDirectory.FullPath + ", located in " + text, false);
							}
							else
							{
								string item = virtualDirectory.FullPath.Substring(text.Length);
								if (!hashSet.Contains(item))
								{
									LanguageDatabase.InitLanguageMetadataFrom(virtualDirectory);
									hashSet.Add(item);
								}
							}
						}
					}
				}
			}
			LanguageDatabase.languages.SortBy((LoadedLanguage l) => l.folderName);
			LanguageDatabase.defaultLanguage = LanguageDatabase.languages.FirstOrDefault((LoadedLanguage la) => la.folderName == LanguageDatabase.DefaultLangFolderName);
			LanguageDatabase.activeLanguage = LanguageDatabase.languages.FirstOrDefault((LoadedLanguage la) => la.folderName == Prefs.LangFolderName);
			if (LanguageDatabase.activeLanguage == null)
			{
				Prefs.LangFolderName = LanguageDatabase.DefaultLangFolderName;
				LanguageDatabase.activeLanguage = LanguageDatabase.languages.FirstOrDefault((LoadedLanguage la) => la.folderName == Prefs.LangFolderName);
			}
			if (LanguageDatabase.activeLanguage == null || LanguageDatabase.defaultLanguage == null)
			{
				Log.Error("No default language found!", false);
				LanguageDatabase.defaultLanguage = LanguageDatabase.languages[0];
				LanguageDatabase.activeLanguage = LanguageDatabase.languages[0];
			}
			LanguageDatabase.activeLanguage.LoadMetadata();
		}

		// Token: 0x0600088A RID: 2186 RVA: 0x0002C0F0 File Offset: 0x0002A2F0
		private static LoadedLanguage InitLanguageMetadataFrom(VirtualDirectory langDir)
		{
			LoadedLanguage loadedLanguage = LanguageDatabase.languages.FirstOrDefault((LoadedLanguage lib) => lib.folderName == langDir.Name || lib.LegacyFolderName == langDir.Name);
			if (loadedLanguage == null)
			{
				loadedLanguage = new LoadedLanguage(langDir.Name);
				LanguageDatabase.languages.Add(loadedLanguage);
			}
			if (loadedLanguage != null)
			{
				loadedLanguage.InitMetadata(langDir);
			}
			return loadedLanguage;
		}

		// Token: 0x0600088B RID: 2187 RVA: 0x0002C150 File Offset: 0x0002A350
		public static string SystemLanguageFolderName()
		{
			if (SteamManager.Initialized)
			{
				string text = SteamApps.GetCurrentGameLanguage().CapitalizeFirst();
				if (LanguageDatabase.SupportedAutoSelectLanguages.Contains(text))
				{
					return text;
				}
			}
			string text2 = Application.systemLanguage.ToString();
			if (LanguageDatabase.SupportedAutoSelectLanguages.Contains(text2))
			{
				return text2;
			}
			return LanguageDatabase.DefaultLangFolderName;
		}

		// Token: 0x0400075E RID: 1886
		private static List<LoadedLanguage> languages = new List<LoadedLanguage>();

		// Token: 0x0400075F RID: 1887
		public static LoadedLanguage activeLanguage;

		// Token: 0x04000760 RID: 1888
		public static LoadedLanguage defaultLanguage;

		// Token: 0x04000761 RID: 1889
		public static readonly string DefaultLangFolderName = "English";

		// Token: 0x04000762 RID: 1890
		private static readonly List<string> SupportedAutoSelectLanguages = new List<string>
		{
			"Arabic",
			"ChineseSimplified",
			"ChineseTraditional",
			"Czech",
			"Danish",
			"Dutch",
			"English",
			"Estonian",
			"Finnish",
			"French",
			"German",
			"Hungarian",
			"Italian",
			"Japanese",
			"Korean",
			"Norwegian",
			"Polish",
			"Portuguese",
			"PortugueseBrazilian",
			"Romanian",
			"Russian",
			"Slovak",
			"Spanish",
			"SpanishLatin",
			"Swedish",
			"Turkish",
			"Ukrainian"
		};
	}
}
