using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x0200046A RID: 1130
	public static class Prefs
	{
		// Token: 0x1700066E RID: 1646
		// (get) Token: 0x06002160 RID: 8544 RVA: 0x000CC7FA File Offset: 0x000CA9FA
		// (set) Token: 0x06002161 RID: 8545 RVA: 0x000CC806 File Offset: 0x000CAA06
		public static float VolumeGame
		{
			get
			{
				return Prefs.data.volumeGame;
			}
			set
			{
				if (Prefs.data.volumeGame == value)
				{
					return;
				}
				Prefs.data.volumeGame = value;
				Prefs.Apply();
			}
		}

		// Token: 0x1700066F RID: 1647
		// (get) Token: 0x06002162 RID: 8546 RVA: 0x000CC826 File Offset: 0x000CAA26
		// (set) Token: 0x06002163 RID: 8547 RVA: 0x000CC832 File Offset: 0x000CAA32
		public static float VolumeMusic
		{
			get
			{
				return Prefs.data.volumeMusic;
			}
			set
			{
				if (Prefs.data.volumeMusic == value)
				{
					return;
				}
				Prefs.data.volumeMusic = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000670 RID: 1648
		// (get) Token: 0x06002164 RID: 8548 RVA: 0x000CC852 File Offset: 0x000CAA52
		// (set) Token: 0x06002165 RID: 8549 RVA: 0x000CC85E File Offset: 0x000CAA5E
		public static float VolumeAmbient
		{
			get
			{
				return Prefs.data.volumeAmbient;
			}
			set
			{
				if (Prefs.data.volumeAmbient == value)
				{
					return;
				}
				Prefs.data.volumeAmbient = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000671 RID: 1649
		// (get) Token: 0x06002166 RID: 8550 RVA: 0x000CC87E File Offset: 0x000CAA7E
		// (set) Token: 0x06002167 RID: 8551 RVA: 0x000CC88A File Offset: 0x000CAA8A
		public static bool ExtremeDifficultyUnlocked
		{
			get
			{
				return Prefs.data.extremeDifficultyUnlocked;
			}
			set
			{
				if (Prefs.data.extremeDifficultyUnlocked == value)
				{
					return;
				}
				Prefs.data.extremeDifficultyUnlocked = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000672 RID: 1650
		// (get) Token: 0x06002168 RID: 8552 RVA: 0x000CC8AA File Offset: 0x000CAAAA
		// (set) Token: 0x06002169 RID: 8553 RVA: 0x000CC8B6 File Offset: 0x000CAAB6
		public static bool AdaptiveTrainingEnabled
		{
			get
			{
				return Prefs.data.adaptiveTrainingEnabled;
			}
			set
			{
				if (Prefs.data.adaptiveTrainingEnabled == value)
				{
					return;
				}
				Prefs.data.adaptiveTrainingEnabled = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000673 RID: 1651
		// (get) Token: 0x0600216A RID: 8554 RVA: 0x000CC8D6 File Offset: 0x000CAAD6
		// (set) Token: 0x0600216B RID: 8555 RVA: 0x000CC8E2 File Offset: 0x000CAAE2
		public static bool EdgeScreenScroll
		{
			get
			{
				return Prefs.data.edgeScreenScroll;
			}
			set
			{
				if (Prefs.data.edgeScreenScroll == value)
				{
					return;
				}
				Prefs.data.edgeScreenScroll = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000674 RID: 1652
		// (get) Token: 0x0600216C RID: 8556 RVA: 0x000CC902 File Offset: 0x000CAB02
		// (set) Token: 0x0600216D RID: 8557 RVA: 0x000CC90E File Offset: 0x000CAB0E
		public static bool RunInBackground
		{
			get
			{
				return Prefs.data.runInBackground;
			}
			set
			{
				if (Prefs.data.runInBackground == value)
				{
					return;
				}
				Prefs.data.runInBackground = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000675 RID: 1653
		// (get) Token: 0x0600216E RID: 8558 RVA: 0x000CC92E File Offset: 0x000CAB2E
		// (set) Token: 0x0600216F RID: 8559 RVA: 0x000CC93A File Offset: 0x000CAB3A
		public static TemperatureDisplayMode TemperatureMode
		{
			get
			{
				return Prefs.data.temperatureMode;
			}
			set
			{
				if (Prefs.data.temperatureMode == value)
				{
					return;
				}
				Prefs.data.temperatureMode = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000676 RID: 1654
		// (get) Token: 0x06002170 RID: 8560 RVA: 0x000CC95A File Offset: 0x000CAB5A
		// (set) Token: 0x06002171 RID: 8561 RVA: 0x000CC966 File Offset: 0x000CAB66
		public static float AutosaveIntervalDays
		{
			get
			{
				return Prefs.data.autosaveIntervalDays;
			}
			set
			{
				if (Prefs.data.autosaveIntervalDays == value)
				{
					return;
				}
				Prefs.data.autosaveIntervalDays = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000677 RID: 1655
		// (get) Token: 0x06002172 RID: 8562 RVA: 0x000CC986 File Offset: 0x000CAB86
		// (set) Token: 0x06002173 RID: 8563 RVA: 0x000CC992 File Offset: 0x000CAB92
		public static bool CustomCursorEnabled
		{
			get
			{
				return Prefs.data.customCursorEnabled;
			}
			set
			{
				if (Prefs.data.customCursorEnabled == value)
				{
					return;
				}
				Prefs.data.customCursorEnabled = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000678 RID: 1656
		// (get) Token: 0x06002174 RID: 8564 RVA: 0x000CC9B2 File Offset: 0x000CABB2
		// (set) Token: 0x06002175 RID: 8565 RVA: 0x000CC9BE File Offset: 0x000CABBE
		public static AnimalNameDisplayMode AnimalNameMode
		{
			get
			{
				return Prefs.data.animalNameMode;
			}
			set
			{
				if (Prefs.data.animalNameMode == value)
				{
					return;
				}
				Prefs.data.animalNameMode = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000679 RID: 1657
		// (get) Token: 0x06002176 RID: 8566 RVA: 0x000CC9DE File Offset: 0x000CABDE
		// (set) Token: 0x06002177 RID: 8567 RVA: 0x000CC9F4 File Offset: 0x000CABF4
		public static bool DevMode
		{
			get
			{
				return Prefs.data == null || Prefs.data.devMode;
			}
			set
			{
				if (Prefs.data.devMode == value)
				{
					return;
				}
				Prefs.data.devMode = value;
				if (!Prefs.data.devMode)
				{
					Prefs.data.logVerbose = false;
					Prefs.data.resetModsConfigOnCrash = true;
					DebugSettings.godMode = false;
				}
				Prefs.Apply();
			}
		}

		// Token: 0x1700067A RID: 1658
		// (get) Token: 0x06002178 RID: 8568 RVA: 0x000CCA47 File Offset: 0x000CAC47
		// (set) Token: 0x06002179 RID: 8569 RVA: 0x000CCA5C File Offset: 0x000CAC5C
		public static bool ResetModsConfigOnCrash
		{
			get
			{
				return Prefs.data == null || Prefs.data.resetModsConfigOnCrash;
			}
			set
			{
				if (Prefs.data.resetModsConfigOnCrash == value)
				{
					return;
				}
				Prefs.data.resetModsConfigOnCrash = value;
				Prefs.Apply();
			}
		}

		// Token: 0x1700067B RID: 1659
		// (get) Token: 0x0600217A RID: 8570 RVA: 0x000CCA7C File Offset: 0x000CAC7C
		// (set) Token: 0x0600217B RID: 8571 RVA: 0x000CCA91 File Offset: 0x000CAC91
		public static bool SimulateNotOwningRoyalty
		{
			get
			{
				return Prefs.data == null || Prefs.data.simulateNotOwningRoyalty;
			}
			set
			{
				if (Prefs.data.simulateNotOwningRoyalty == value)
				{
					return;
				}
				Prefs.data.simulateNotOwningRoyalty = value;
				Prefs.Apply();
			}
		}

		// Token: 0x1700067C RID: 1660
		// (get) Token: 0x0600217C RID: 8572 RVA: 0x000CCAB1 File Offset: 0x000CACB1
		// (set) Token: 0x0600217D RID: 8573 RVA: 0x000CCABD File Offset: 0x000CACBD
		public static List<string> PreferredNames
		{
			get
			{
				return Prefs.data.preferredNames;
			}
			set
			{
				if (Prefs.data.preferredNames == value)
				{
					return;
				}
				Prefs.data.preferredNames = value;
				Prefs.Apply();
			}
		}

		// Token: 0x1700067D RID: 1661
		// (get) Token: 0x0600217E RID: 8574 RVA: 0x000CCADD File Offset: 0x000CACDD
		// (set) Token: 0x0600217F RID: 8575 RVA: 0x000CCAE9 File Offset: 0x000CACE9
		public static string LangFolderName
		{
			get
			{
				return Prefs.data.langFolderName;
			}
			set
			{
				if (Prefs.data.langFolderName == value)
				{
					return;
				}
				Prefs.data.langFolderName = value;
				Prefs.Apply();
			}
		}

		// Token: 0x1700067E RID: 1662
		// (get) Token: 0x06002180 RID: 8576 RVA: 0x000CCB0E File Offset: 0x000CAD0E
		// (set) Token: 0x06002181 RID: 8577 RVA: 0x000CCB1A File Offset: 0x000CAD1A
		public static bool LogVerbose
		{
			get
			{
				return Prefs.data.logVerbose;
			}
			set
			{
				if (Prefs.data.logVerbose == value)
				{
					return;
				}
				Prefs.data.logVerbose = value;
				Prefs.Apply();
			}
		}

		// Token: 0x1700067F RID: 1663
		// (get) Token: 0x06002182 RID: 8578 RVA: 0x000CCB3A File Offset: 0x000CAD3A
		// (set) Token: 0x06002183 RID: 8579 RVA: 0x000CCB4F File Offset: 0x000CAD4F
		public static bool PauseOnError
		{
			get
			{
				return Prefs.data != null && Prefs.data.pauseOnError;
			}
			set
			{
				Prefs.data.pauseOnError = value;
			}
		}

		// Token: 0x17000680 RID: 1664
		// (get) Token: 0x06002184 RID: 8580 RVA: 0x000CCB5C File Offset: 0x000CAD5C
		// (set) Token: 0x06002185 RID: 8581 RVA: 0x000CCB68 File Offset: 0x000CAD68
		public static bool PauseOnLoad
		{
			get
			{
				return Prefs.data.pauseOnLoad;
			}
			set
			{
				Prefs.data.pauseOnLoad = value;
			}
		}

		// Token: 0x17000681 RID: 1665
		// (get) Token: 0x06002186 RID: 8582 RVA: 0x000CCB75 File Offset: 0x000CAD75
		// (set) Token: 0x06002187 RID: 8583 RVA: 0x000CCB81 File Offset: 0x000CAD81
		public static AutomaticPauseMode AutomaticPauseMode
		{
			get
			{
				return Prefs.data.automaticPauseMode;
			}
			set
			{
				Prefs.data.automaticPauseMode = value;
			}
		}

		// Token: 0x17000682 RID: 1666
		// (get) Token: 0x06002188 RID: 8584 RVA: 0x000CCB8E File Offset: 0x000CAD8E
		// (set) Token: 0x06002189 RID: 8585 RVA: 0x000CCB9A File Offset: 0x000CAD9A
		public static bool ShowRealtimeClock
		{
			get
			{
				return Prefs.data.showRealtimeClock;
			}
			set
			{
				Prefs.data.showRealtimeClock = value;
			}
		}

		// Token: 0x17000683 RID: 1667
		// (get) Token: 0x0600218A RID: 8586 RVA: 0x000CCBA7 File Offset: 0x000CADA7
		// (set) Token: 0x0600218B RID: 8587 RVA: 0x000CCBB3 File Offset: 0x000CADB3
		public static bool TestMapSizes
		{
			get
			{
				return Prefs.data.testMapSizes;
			}
			set
			{
				Prefs.data.testMapSizes = value;
			}
		}

		// Token: 0x17000684 RID: 1668
		// (get) Token: 0x0600218C RID: 8588 RVA: 0x000CCBC0 File Offset: 0x000CADC0
		// (set) Token: 0x0600218D RID: 8589 RVA: 0x000CCBCC File Offset: 0x000CADCC
		public static int MaxNumberOfPlayerSettlements
		{
			get
			{
				return Prefs.data.maxNumberOfPlayerSettlements;
			}
			set
			{
				Prefs.data.maxNumberOfPlayerSettlements = value;
			}
		}

		// Token: 0x17000685 RID: 1669
		// (get) Token: 0x0600218E RID: 8590 RVA: 0x000CCBD9 File Offset: 0x000CADD9
		// (set) Token: 0x0600218F RID: 8591 RVA: 0x000CCBE5 File Offset: 0x000CADE5
		public static bool PlantWindSway
		{
			get
			{
				return Prefs.data.plantWindSway;
			}
			set
			{
				Prefs.data.plantWindSway = value;
			}
		}

		// Token: 0x17000686 RID: 1670
		// (get) Token: 0x06002190 RID: 8592 RVA: 0x000CCBF2 File Offset: 0x000CADF2
		// (set) Token: 0x06002191 RID: 8593 RVA: 0x000CCBFE File Offset: 0x000CADFE
		public static bool ResourceReadoutCategorized
		{
			get
			{
				return Prefs.data.resourceReadoutCategorized;
			}
			set
			{
				if (value == Prefs.data.resourceReadoutCategorized)
				{
					return;
				}
				Prefs.data.resourceReadoutCategorized = value;
				Prefs.Save();
			}
		}

		// Token: 0x17000687 RID: 1671
		// (get) Token: 0x06002192 RID: 8594 RVA: 0x000CCC1E File Offset: 0x000CAE1E
		// (set) Token: 0x06002193 RID: 8595 RVA: 0x000CCC2A File Offset: 0x000CAE2A
		public static float UIScale
		{
			get
			{
				return Prefs.data.uiScale;
			}
			set
			{
				Prefs.data.uiScale = value;
			}
		}

		// Token: 0x17000688 RID: 1672
		// (get) Token: 0x06002194 RID: 8596 RVA: 0x000CCC37 File Offset: 0x000CAE37
		// (set) Token: 0x06002195 RID: 8597 RVA: 0x000CCC43 File Offset: 0x000CAE43
		public static int ScreenWidth
		{
			get
			{
				return Prefs.data.screenWidth;
			}
			set
			{
				Prefs.data.screenWidth = value;
			}
		}

		// Token: 0x17000689 RID: 1673
		// (get) Token: 0x06002196 RID: 8598 RVA: 0x000CCC50 File Offset: 0x000CAE50
		// (set) Token: 0x06002197 RID: 8599 RVA: 0x000CCC5C File Offset: 0x000CAE5C
		public static int ScreenHeight
		{
			get
			{
				return Prefs.data.screenHeight;
			}
			set
			{
				Prefs.data.screenHeight = value;
			}
		}

		// Token: 0x1700068A RID: 1674
		// (get) Token: 0x06002198 RID: 8600 RVA: 0x000CCC69 File Offset: 0x000CAE69
		// (set) Token: 0x06002199 RID: 8601 RVA: 0x000CCC75 File Offset: 0x000CAE75
		public static bool FullScreen
		{
			get
			{
				return Prefs.data.fullscreen;
			}
			set
			{
				Prefs.data.fullscreen = value;
			}
		}

		// Token: 0x1700068B RID: 1675
		// (get) Token: 0x0600219A RID: 8602 RVA: 0x000CCC82 File Offset: 0x000CAE82
		// (set) Token: 0x0600219B RID: 8603 RVA: 0x000CCC8E File Offset: 0x000CAE8E
		public static bool HatsOnlyOnMap
		{
			get
			{
				return Prefs.data.hatsOnlyOnMap;
			}
			set
			{
				if (Prefs.data.hatsOnlyOnMap == value)
				{
					return;
				}
				Prefs.data.hatsOnlyOnMap = value;
				Prefs.Apply();
			}
		}

		// Token: 0x1700068C RID: 1676
		// (get) Token: 0x0600219C RID: 8604 RVA: 0x000CCCAE File Offset: 0x000CAEAE
		// (set) Token: 0x0600219D RID: 8605 RVA: 0x000CCCBA File Offset: 0x000CAEBA
		public static float MapDragSensitivity
		{
			get
			{
				return Prefs.data.mapDragSensitivity;
			}
			set
			{
				Prefs.data.mapDragSensitivity = value;
			}
		}

		// Token: 0x0600219E RID: 8606 RVA: 0x000CCCC8 File Offset: 0x000CAEC8
		public static void Init()
		{
			bool flag = !new FileInfo(GenFilePaths.PrefsFilePath).Exists;
			Prefs.data = new PrefsData();
			Prefs.data = DirectXmlLoader.ItemFromXmlFile<PrefsData>(GenFilePaths.PrefsFilePath, true);
			BackCompatibility.PrefsDataPostLoad(Prefs.data);
			if (flag)
			{
				Prefs.data.langFolderName = LanguageDatabase.SystemLanguageFolderName();
				Prefs.data.uiScale = ResolutionUtility.GetRecommendedUIScale(Prefs.data.screenWidth, Prefs.data.screenHeight);
			}
			if (DevModePermanentlyDisabledUtility.Disabled)
			{
				Prefs.DevMode = false;
			}
			Prefs.Apply();
		}

		// Token: 0x0600219F RID: 8607 RVA: 0x000CCD54 File Offset: 0x000CAF54
		public static void Save()
		{
			try
			{
				XDocument xdocument = new XDocument();
				XElement content = DirectXmlSaver.XElementFromObject(Prefs.data, typeof(PrefsData));
				xdocument.Add(content);
				xdocument.Save(GenFilePaths.PrefsFilePath);
			}
			catch (Exception ex)
			{
				GenUI.ErrorDialog("ProblemSavingFile".Translate(GenFilePaths.PrefsFilePath, ex.ToString()));
				Log.Error("Exception saving prefs: " + ex, false);
			}
		}

		// Token: 0x060021A0 RID: 8608 RVA: 0x000CCDDC File Offset: 0x000CAFDC
		public static void Apply()
		{
			Prefs.data.Apply();
		}

		// Token: 0x060021A1 RID: 8609 RVA: 0x000CCDE8 File Offset: 0x000CAFE8
		public static NameTriple RandomPreferredName()
		{
			string rawName;
			if ((from name in Prefs.PreferredNames
			where !name.NullOrEmpty()
			select name).TryRandomElement(out rawName))
			{
				return NameTriple.FromString(rawName);
			}
			return null;
		}

		// Token: 0x0400146A RID: 5226
		private static PrefsData data;
	}
}
