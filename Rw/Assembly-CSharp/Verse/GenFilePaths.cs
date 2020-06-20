using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000445 RID: 1093
	public static class GenFilePaths
	{
		// Token: 0x17000650 RID: 1616
		// (get) Token: 0x060020AE RID: 8366 RVA: 0x000C8024 File Offset: 0x000C6224
		public static string SaveDataFolderPath
		{
			get
			{
				if (GenFilePaths.saveDataPath == null)
				{
					string text;
					if (GenCommandLine.TryGetCommandLineArg("savedatafolder", out text))
					{
						text.TrimEnd(new char[]
						{
							'\\',
							'/'
						});
						if (text == "")
						{
							text = (Path.DirectorySeparatorChar.ToString() ?? "");
						}
						GenFilePaths.saveDataPath = text;
						Log.Message("Save data folder overridden to " + GenFilePaths.saveDataPath, false);
					}
					else
					{
						DirectoryInfo directoryInfo = new DirectoryInfo(UnityData.dataPath);
						if (UnityData.isEditor)
						{
							GenFilePaths.saveDataPath = Path.Combine(directoryInfo.Parent.ToString(), "SaveData");
						}
						else if (UnityData.platform == RuntimePlatform.OSXPlayer || UnityData.platform == RuntimePlatform.OSXEditor)
						{
							string path = Path.Combine(Directory.GetParent(UnityData.persistentDataPath).ToString(), "RimWorld");
							if (!Directory.Exists(path))
							{
								Directory.CreateDirectory(path);
							}
							GenFilePaths.saveDataPath = path;
						}
						else
						{
							GenFilePaths.saveDataPath = Application.persistentDataPath;
						}
					}
					DirectoryInfo directoryInfo2 = new DirectoryInfo(GenFilePaths.saveDataPath);
					if (!directoryInfo2.Exists)
					{
						directoryInfo2.Create();
					}
				}
				return GenFilePaths.saveDataPath;
			}
		}

		// Token: 0x17000651 RID: 1617
		// (get) Token: 0x060020AF RID: 8367 RVA: 0x000C813C File Offset: 0x000C633C
		public static string ScenarioPreviewImagePath
		{
			get
			{
				if (!UnityData.isEditor)
				{
					return Path.Combine(GenFilePaths.ExecutableDir.FullName, "ScenarioPreview.jpg");
				}
				return Path.Combine(Path.Combine(Path.Combine(GenFilePaths.ExecutableDir.FullName, "PlatformSpecific"), "All"), "ScenarioPreview.jpg");
			}
		}

		// Token: 0x17000652 RID: 1618
		// (get) Token: 0x060020B0 RID: 8368 RVA: 0x000C818D File Offset: 0x000C638D
		private static DirectoryInfo ExecutableDir
		{
			get
			{
				return new DirectoryInfo(UnityData.dataPath).Parent;
			}
		}

		// Token: 0x17000653 RID: 1619
		// (get) Token: 0x060020B1 RID: 8369 RVA: 0x000C819E File Offset: 0x000C639E
		public static string ModsFolderPath
		{
			get
			{
				if (GenFilePaths.modsFolderPath == null)
				{
					GenFilePaths.modsFolderPath = GenFilePaths.GetOrCreateModsFolder("Mods");
				}
				return GenFilePaths.modsFolderPath;
			}
		}

		// Token: 0x17000654 RID: 1620
		// (get) Token: 0x060020B2 RID: 8370 RVA: 0x000C81BB File Offset: 0x000C63BB
		public static string OfficialModsFolderPath
		{
			get
			{
				if (GenFilePaths.officialModsFolderPath == null)
				{
					GenFilePaths.officialModsFolderPath = GenFilePaths.GetOrCreateModsFolder("Data");
				}
				return GenFilePaths.officialModsFolderPath;
			}
		}

		// Token: 0x17000655 RID: 1621
		// (get) Token: 0x060020B3 RID: 8371 RVA: 0x000C81D8 File Offset: 0x000C63D8
		public static string ConfigFolderPath
		{
			get
			{
				return GenFilePaths.FolderUnderSaveData("Config");
			}
		}

		// Token: 0x17000656 RID: 1622
		// (get) Token: 0x060020B4 RID: 8372 RVA: 0x000C81E4 File Offset: 0x000C63E4
		private static string SavedGamesFolderPath
		{
			get
			{
				return GenFilePaths.FolderUnderSaveData("Saves");
			}
		}

		// Token: 0x17000657 RID: 1623
		// (get) Token: 0x060020B5 RID: 8373 RVA: 0x000C81F0 File Offset: 0x000C63F0
		private static string ScenariosFolderPath
		{
			get
			{
				return GenFilePaths.FolderUnderSaveData("Scenarios");
			}
		}

		// Token: 0x17000658 RID: 1624
		// (get) Token: 0x060020B6 RID: 8374 RVA: 0x000C81FC File Offset: 0x000C63FC
		private static string ExternalHistoryFolderPath
		{
			get
			{
				return GenFilePaths.FolderUnderSaveData("External");
			}
		}

		// Token: 0x17000659 RID: 1625
		// (get) Token: 0x060020B7 RID: 8375 RVA: 0x000C8208 File Offset: 0x000C6408
		public static string ScreenshotFolderPath
		{
			get
			{
				return GenFilePaths.FolderUnderSaveData("Screenshots");
			}
		}

		// Token: 0x1700065A RID: 1626
		// (get) Token: 0x060020B8 RID: 8376 RVA: 0x000C8214 File Offset: 0x000C6414
		public static string DevOutputFolderPath
		{
			get
			{
				return GenFilePaths.FolderUnderSaveData("DevOutput");
			}
		}

		// Token: 0x1700065B RID: 1627
		// (get) Token: 0x060020B9 RID: 8377 RVA: 0x000C8220 File Offset: 0x000C6420
		public static string ModsConfigFilePath
		{
			get
			{
				return Path.Combine(GenFilePaths.ConfigFolderPath, "ModsConfig.xml");
			}
		}

		// Token: 0x1700065C RID: 1628
		// (get) Token: 0x060020BA RID: 8378 RVA: 0x000C8231 File Offset: 0x000C6431
		public static string ConceptKnowledgeFilePath
		{
			get
			{
				return Path.Combine(GenFilePaths.ConfigFolderPath, "Knowledge.xml");
			}
		}

		// Token: 0x1700065D RID: 1629
		// (get) Token: 0x060020BB RID: 8379 RVA: 0x000C8242 File Offset: 0x000C6442
		public static string PrefsFilePath
		{
			get
			{
				return Path.Combine(GenFilePaths.ConfigFolderPath, "Prefs.xml");
			}
		}

		// Token: 0x1700065E RID: 1630
		// (get) Token: 0x060020BC RID: 8380 RVA: 0x000C8253 File Offset: 0x000C6453
		public static string KeyPrefsFilePath
		{
			get
			{
				return Path.Combine(GenFilePaths.ConfigFolderPath, "KeyPrefs.xml");
			}
		}

		// Token: 0x1700065F RID: 1631
		// (get) Token: 0x060020BD RID: 8381 RVA: 0x000C8264 File Offset: 0x000C6464
		public static string LastPlayedVersionFilePath
		{
			get
			{
				return Path.Combine(GenFilePaths.ConfigFolderPath, "LastPlayedVersion.txt");
			}
		}

		// Token: 0x17000660 RID: 1632
		// (get) Token: 0x060020BE RID: 8382 RVA: 0x000C8275 File Offset: 0x000C6475
		public static string DevModePermanentlyDisabledFilePath
		{
			get
			{
				return Path.Combine(GenFilePaths.ConfigFolderPath, "DevModeDisabled");
			}
		}

		// Token: 0x17000661 RID: 1633
		// (get) Token: 0x060020BF RID: 8383 RVA: 0x000C8286 File Offset: 0x000C6486
		public static string BackstoryOutputFilePath
		{
			get
			{
				return Path.Combine(GenFilePaths.DevOutputFolderPath, "Fresh_Backstories.xml");
			}
		}

		// Token: 0x17000662 RID: 1634
		// (get) Token: 0x060020C0 RID: 8384 RVA: 0x000C8297 File Offset: 0x000C6497
		public static string TempFolderPath
		{
			get
			{
				return Application.temporaryCachePath;
			}
		}

		// Token: 0x17000663 RID: 1635
		// (get) Token: 0x060020C1 RID: 8385 RVA: 0x000C82A0 File Offset: 0x000C64A0
		public static IEnumerable<FileInfo> AllSavedGameFiles
		{
			get
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(GenFilePaths.SavedGamesFolderPath);
				if (!directoryInfo.Exists)
				{
					directoryInfo.Create();
				}
				return from f in directoryInfo.GetFiles()
				where f.Extension == ".rws"
				orderby f.LastWriteTime descending
				select f;
			}
		}

		// Token: 0x17000664 RID: 1636
		// (get) Token: 0x060020C2 RID: 8386 RVA: 0x000C8314 File Offset: 0x000C6514
		public static IEnumerable<FileInfo> AllCustomScenarioFiles
		{
			get
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(GenFilePaths.ScenariosFolderPath);
				if (!directoryInfo.Exists)
				{
					directoryInfo.Create();
				}
				return from f in directoryInfo.GetFiles()
				where f.Extension == ".rsc"
				orderby f.LastWriteTime descending
				select f;
			}
		}

		// Token: 0x17000665 RID: 1637
		// (get) Token: 0x060020C3 RID: 8387 RVA: 0x000C8388 File Offset: 0x000C6588
		public static IEnumerable<FileInfo> AllExternalHistoryFiles
		{
			get
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(GenFilePaths.ExternalHistoryFolderPath);
				if (!directoryInfo.Exists)
				{
					directoryInfo.Create();
				}
				return from f in directoryInfo.GetFiles()
				where f.Extension == ".rwh"
				orderby f.LastWriteTime descending
				select f;
			}
		}

		// Token: 0x060020C4 RID: 8388 RVA: 0x000C83FC File Offset: 0x000C65FC
		private static string FolderUnderSaveData(string folderName)
		{
			string text = Path.Combine(GenFilePaths.SaveDataFolderPath, folderName);
			DirectoryInfo directoryInfo = new DirectoryInfo(text);
			if (!directoryInfo.Exists)
			{
				directoryInfo.Create();
			}
			return text;
		}

		// Token: 0x060020C5 RID: 8389 RVA: 0x000C8429 File Offset: 0x000C6629
		public static string FilePathForSavedGame(string gameName)
		{
			return Path.Combine(GenFilePaths.SavedGamesFolderPath, gameName + ".rws");
		}

		// Token: 0x060020C6 RID: 8390 RVA: 0x000C8440 File Offset: 0x000C6640
		public static string AbsPathForScenario(string scenarioName)
		{
			return Path.Combine(GenFilePaths.ScenariosFolderPath, scenarioName + ".rsc");
		}

		// Token: 0x060020C7 RID: 8391 RVA: 0x000C8458 File Offset: 0x000C6658
		public static string ContentPath<T>()
		{
			if (typeof(T) == typeof(AudioClip))
			{
				return "Sounds/";
			}
			if (typeof(T) == typeof(Texture2D))
			{
				return "Textures/";
			}
			if (typeof(T) == typeof(string))
			{
				return "Strings/";
			}
			throw new ArgumentException();
		}

		// Token: 0x060020C8 RID: 8392 RVA: 0x000C84D0 File Offset: 0x000C66D0
		private static string GetOrCreateModsFolder(string folderName)
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(UnityData.dataPath);
			DirectoryInfo directoryInfo2;
			if (UnityData.isEditor)
			{
				directoryInfo2 = directoryInfo;
			}
			else
			{
				directoryInfo2 = directoryInfo.Parent;
			}
			string text = Path.Combine(directoryInfo2.ToString(), folderName);
			DirectoryInfo directoryInfo3 = new DirectoryInfo(text);
			if (!directoryInfo3.Exists)
			{
				directoryInfo3.Create();
			}
			return text;
		}

		// Token: 0x060020C9 RID: 8393 RVA: 0x000C8520 File Offset: 0x000C6720
		public static string SafeURIForUnityWWWFromPath(string rawPath)
		{
			string text = rawPath;
			for (int i = 0; i < GenFilePaths.FilePathRaw.Length; i++)
			{
				text = text.Replace(GenFilePaths.FilePathRaw[i], GenFilePaths.FilePathSafe[i]);
			}
			return "file:///" + text;
		}

		// Token: 0x040013F5 RID: 5109
		private static string saveDataPath = null;

		// Token: 0x040013F6 RID: 5110
		private static string modsFolderPath = null;

		// Token: 0x040013F7 RID: 5111
		private static string officialModsFolderPath = null;

		// Token: 0x040013F8 RID: 5112
		public const string SoundsFolder = "Sounds/";

		// Token: 0x040013F9 RID: 5113
		public const string SoundsFolderName = "Sounds";

		// Token: 0x040013FA RID: 5114
		public const string TexturesFolder = "Textures/";

		// Token: 0x040013FB RID: 5115
		public const string TexturesFolderName = "Textures";

		// Token: 0x040013FC RID: 5116
		public const string StringsFolder = "Strings/";

		// Token: 0x040013FD RID: 5117
		public const string DefsFolder = "Defs/";

		// Token: 0x040013FE RID: 5118
		public const string PatchesFolder = "Patches/";

		// Token: 0x040013FF RID: 5119
		public const string AssetBundlesFolderName = "AssetBundles";

		// Token: 0x04001400 RID: 5120
		public const string AssetsFolderName = "Assets";

		// Token: 0x04001401 RID: 5121
		public const string ResourcesFolderName = "Resources";

		// Token: 0x04001402 RID: 5122
		public const string ModsFolderName = "Mods";

		// Token: 0x04001403 RID: 5123
		public const string AssembliesFolder = "Assemblies/";

		// Token: 0x04001404 RID: 5124
		public const string OfficialModsFolderName = "Data";

		// Token: 0x04001405 RID: 5125
		public const string CoreFolderName = "Core";

		// Token: 0x04001406 RID: 5126
		public const string BackstoriesPath = "Backstories";

		// Token: 0x04001407 RID: 5127
		public const string SavedGameExtension = ".rws";

		// Token: 0x04001408 RID: 5128
		public const string ScenarioExtension = ".rsc";

		// Token: 0x04001409 RID: 5129
		public const string ExternalHistoryFileExtension = ".rwh";

		// Token: 0x0400140A RID: 5130
		private const string SaveDataFolderCommand = "savedatafolder";

		// Token: 0x0400140B RID: 5131
		private static readonly string[] FilePathRaw = new string[]
		{
			"Ž",
			"ž",
			"Ÿ",
			"¡",
			"¢",
			"£",
			"¤",
			"¥",
			"¦",
			"§",
			"¨",
			"©",
			"ª",
			"À",
			"Á",
			"Â",
			"Ã",
			"Ä",
			"Å",
			"Æ",
			"Ç",
			"È",
			"É",
			"Ê",
			"Ë",
			"Ì",
			"Í",
			"Î",
			"Ï",
			"Ð",
			"Ñ",
			"Ò",
			"Ó",
			"Ô",
			"Õ",
			"Ö",
			"Ù",
			"Ú",
			"Û",
			"Ü",
			"Ý",
			"Þ",
			"ß",
			"à",
			"á",
			"â",
			"ã",
			"ä",
			"å",
			"æ",
			"ç",
			"è",
			"é",
			"ê",
			"ë",
			"ì",
			"í",
			"î",
			"ï",
			"ð",
			"ñ",
			"ò",
			"ó",
			"ô",
			"õ",
			"ö",
			"ù",
			"ú",
			"û",
			"ü",
			"ý",
			"þ",
			"ÿ"
		};

		// Token: 0x0400140C RID: 5132
		private static readonly string[] FilePathSafe = new string[]
		{
			"%8E",
			"%9E",
			"%9F",
			"%A1",
			"%A2",
			"%A3",
			"%A4",
			"%A5",
			"%A6",
			"%A7",
			"%A8",
			"%A9",
			"%AA",
			"%C0",
			"%C1",
			"%C2",
			"%C3",
			"%C4",
			"%C5",
			"%C6",
			"%C7",
			"%C8",
			"%C9",
			"%CA",
			"%CB",
			"%CC",
			"%CD",
			"%CE",
			"%CF",
			"%D0",
			"%D1",
			"%D2",
			"%D3",
			"%D4",
			"%D5",
			"%D6",
			"%D9",
			"%DA",
			"%DB",
			"%DC",
			"%DD",
			"%DE",
			"%DF",
			"%E0",
			"%E1",
			"%E2",
			"%E3",
			"%E4",
			"%E5",
			"%E6",
			"%E7",
			"%E8",
			"%E9",
			"%EA",
			"%EB",
			"%EC",
			"%ED",
			"%EE",
			"%EF",
			"%F0",
			"%F1",
			"%F2",
			"%F3",
			"%F4",
			"%F5",
			"%F6",
			"%F9",
			"%FA",
			"%FB",
			"%FC",
			"%FD",
			"%FE",
			"%FF"
		};
	}
}
