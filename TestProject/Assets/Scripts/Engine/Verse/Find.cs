using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using UnityStandardAssets.ImageEffects;
using Verse.Sound;

namespace Verse
{
	
	public static class Find
	{
		
		// (get) Token: 0x06002008 RID: 8200 RVA: 0x000C3AA4 File Offset: 0x000C1CA4
		public static Root Root
		{
			get
			{
				return Current.Root;
			}
		}

		
		// (get) Token: 0x06002009 RID: 8201 RVA: 0x000C3AAB File Offset: 0x000C1CAB
		public static SoundRoot SoundRoot
		{
			get
			{
				return Current.Root.soundRoot;
			}
		}

		
		// (get) Token: 0x0600200A RID: 8202 RVA: 0x000C3AB7 File Offset: 0x000C1CB7
		public static UIRoot UIRoot
		{
			get
			{
				if (!(Current.Root != null))
				{
					return null;
				}
				return Current.Root.uiRoot;
			}
		}

		
		// (get) Token: 0x0600200B RID: 8203 RVA: 0x000C3AD2 File Offset: 0x000C1CD2
		public static MusicManagerEntry MusicManagerEntry
		{
			get
			{
				return ((Root_Entry)Current.Root).musicManagerEntry;
			}
		}

		
		// (get) Token: 0x0600200C RID: 8204 RVA: 0x000C3AE3 File Offset: 0x000C1CE3
		public static MusicManagerPlay MusicManagerPlay
		{
			get
			{
				return ((Root_Play)Current.Root).musicManagerPlay;
			}
		}

		
		// (get) Token: 0x0600200D RID: 8205 RVA: 0x000C3AF4 File Offset: 0x000C1CF4
		public static LanguageWorker ActiveLanguageWorker
		{
			get
			{
				return LanguageDatabase.activeLanguage.Worker;
			}
		}

		
		// (get) Token: 0x0600200E RID: 8206 RVA: 0x000C3B00 File Offset: 0x000C1D00
		public static Camera Camera
		{
			get
			{
				return Current.Camera;
			}
		}

		
		// (get) Token: 0x0600200F RID: 8207 RVA: 0x000C3B07 File Offset: 0x000C1D07
		public static CameraDriver CameraDriver
		{
			get
			{
				return Current.CameraDriver;
			}
		}

		
		// (get) Token: 0x06002010 RID: 8208 RVA: 0x000C3B0E File Offset: 0x000C1D0E
		public static ColorCorrectionCurves CameraColor
		{
			get
			{
				return Current.ColorCorrectionCurves;
			}
		}

		
		// (get) Token: 0x06002011 RID: 8209 RVA: 0x000C3B15 File Offset: 0x000C1D15
		public static Camera PortraitCamera
		{
			get
			{
				return PortraitCameraManager.PortraitCamera;
			}
		}

		
		// (get) Token: 0x06002012 RID: 8210 RVA: 0x000C3B1C File Offset: 0x000C1D1C
		public static PortraitRenderer PortraitRenderer
		{
			get
			{
				return PortraitCameraManager.PortraitRenderer;
			}
		}

		
		// (get) Token: 0x06002013 RID: 8211 RVA: 0x000C3B23 File Offset: 0x000C1D23
		public static Camera WorldCamera
		{
			get
			{
				return WorldCameraManager.WorldCamera;
			}
		}


		public static WorldCameraDriver WorldCameraDriver
		{
			get
			{
				return WorldCameraManager.WorldCameraDriver;
			}
		}


		public static WindowStack WindowStack
		{
			get
			{
				if (Find.UIRoot == null)
				{
					return null;
				}
				return Find.UIRoot.windows;
			}
		}


		public static ScreenshotModeHandler ScreenshotModeHandler
		{
			get
			{
				return Find.UIRoot.screenshotMode;
			}
		}


		public static MainButtonsRoot MainButtonsRoot
		{
			get
			{
				return ((UIRoot_Play)Find.UIRoot).mainButtonsRoot;
			}
		}


		public static MainTabsRoot MainTabsRoot
		{
			get
			{
				return Find.MainButtonsRoot.tabs;
			}
		}


		public static MapInterface MapUI
		{
			get
			{
				return ((UIRoot_Play)Find.UIRoot).mapUI;
			}
		}

		public static Selector Selector
		{
			get
			{
				return Find.MapUI.selector;
			}
		}

		public static Targeter Targeter
		{
			get
			{
				return Find.MapUI.targeter;
			}
		}

	
		public static ColonistBar ColonistBar
		{
			get
			{
				return Find.MapUI.colonistBar;
			}
		}

		
		// (get) Token: 0x0600201D RID: 8221 RVA: 0x000C3BA4 File Offset: 0x000C1DA4
		public static DesignatorManager DesignatorManager
		{
			get
			{
				return Find.MapUI.designatorManager;
			}
		}

		
		// (get) Token: 0x0600201E RID: 8222 RVA: 0x000C3BB0 File Offset: 0x000C1DB0
		public static ReverseDesignatorDatabase ReverseDesignatorDatabase
		{
			get
			{
				return Find.MapUI.reverseDesignatorDatabase;
			}
		}


		public static GameInitData GameInitData
		{
			get
			{
				if (Current.Game == null)
				{
					return null;
				}
				return Current.Game.InitData;
			}
		}

		public static GameInfo GameInfo
		{
			get
			{
				return Current.Game.Info;
			}
		}

		public static Scenario Scenario
		{
			get
			{
				if (Current.Game != null && Current.Game.Scenario != null)
				{
					return Current.Game.Scenario;
				}
				if (ScenarioMaker.GeneratingScenario != null)
				{
					return ScenarioMaker.GeneratingScenario;
				}
				if (Find.UIRoot != null)
				{
					Page_ScenarioEditor page_ScenarioEditor = Find.WindowStack.WindowOfType<Page_ScenarioEditor>();
					if (page_ScenarioEditor != null)
					{
						return page_ScenarioEditor.EditingScenario;
					}
				}
				return null;
			}
		}

		
		// (get) Token: 0x06002022 RID: 8226 RVA: 0x000C3C35 File Offset: 0x000C1E35
		public static World World
		{
			get
			{
				if (Current.Game == null || Current.Game.World == null)
				{
					return Current.CreatingWorld;
				}
				return Current.Game.World;
			}
		}

		
		// (get) Token: 0x06002023 RID: 8227 RVA: 0x000C3C5A File Offset: 0x000C1E5A
		public static List<Map> Maps
		{
			get
			{
				if (Current.Game == null)
				{
					return null;
				}
				return Current.Game.Maps;
			}
		}

		
		// (get) Token: 0x06002024 RID: 8228 RVA: 0x000C3C6F File Offset: 0x000C1E6F
		public static Map CurrentMap
		{
			get
			{
				if (Current.Game == null)
				{
					return null;
				}
				return Current.Game.CurrentMap;
			}
		}

		
		// (get) Token: 0x06002025 RID: 8229 RVA: 0x000C3C84 File Offset: 0x000C1E84
		public static Map AnyPlayerHomeMap
		{
			get
			{
				return Current.Game.AnyPlayerHomeMap;
			}
		}

		
		// (get) Token: 0x06002026 RID: 8230 RVA: 0x000C3C90 File Offset: 0x000C1E90
		public static Map RandomPlayerHomeMap
		{
			get
			{
				return Current.Game.RandomPlayerHomeMap;
			}
		}

		
		// (get) Token: 0x06002027 RID: 8231 RVA: 0x000C3C9C File Offset: 0x000C1E9C
		public static StoryWatcher StoryWatcher
		{
			get
			{
				return Current.Game.storyWatcher;
			}
		}

		
		// (get) Token: 0x06002028 RID: 8232 RVA: 0x000C3CA8 File Offset: 0x000C1EA8
		public static ResearchManager ResearchManager
		{
			get
			{
				return Current.Game.researchManager;
			}
		}

		
		// (get) Token: 0x06002029 RID: 8233 RVA: 0x000C3CB4 File Offset: 0x000C1EB4
		public static Storyteller Storyteller
		{
			get
			{
				if (Current.Game == null)
				{
					return null;
				}
				return Current.Game.storyteller;
			}
		}

		
		// (get) Token: 0x0600202A RID: 8234 RVA: 0x000C3CC9 File Offset: 0x000C1EC9
		public static GameEnder GameEnder
		{
			get
			{
				return Current.Game.gameEnder;
			}
		}

		
		// (get) Token: 0x0600202B RID: 8235 RVA: 0x000C3CD5 File Offset: 0x000C1ED5
		public static LetterStack LetterStack
		{
			get
			{
				return Current.Game.letterStack;
			}
		}

		
		// (get) Token: 0x0600202C RID: 8236 RVA: 0x000C3CE1 File Offset: 0x000C1EE1
		public static Archive Archive
		{
			get
			{
				if (Find.History == null)
				{
					return null;
				}
				return Find.History.archive;
			}
		}

		
		// (get) Token: 0x0600202D RID: 8237 RVA: 0x000C3CF6 File Offset: 0x000C1EF6
		public static PlaySettings PlaySettings
		{
			get
			{
				return Current.Game.playSettings;
			}
		}

		
		// (get) Token: 0x0600202E RID: 8238 RVA: 0x000C3D02 File Offset: 0x000C1F02
		public static History History
		{
			get
			{
				if (Current.Game == null)
				{
					return null;
				}
				return Current.Game.history;
			}
		}

		
		// (get) Token: 0x0600202F RID: 8239 RVA: 0x000C3D17 File Offset: 0x000C1F17
		public static TaleManager TaleManager
		{
			get
			{
				return Current.Game.taleManager;
			}
		}

		
		// (get) Token: 0x06002030 RID: 8240 RVA: 0x000C3D23 File Offset: 0x000C1F23
		public static PlayLog PlayLog
		{
			get
			{
				return Current.Game.playLog;
			}
		}

		
		// (get) Token: 0x06002031 RID: 8241 RVA: 0x000C3D2F File Offset: 0x000C1F2F
		public static BattleLog BattleLog
		{
			get
			{
				return Current.Game.battleLog;
			}
		}

		
		// (get) Token: 0x06002032 RID: 8242 RVA: 0x000C3D3B File Offset: 0x000C1F3B
		public static TickManager TickManager
		{
			get
			{
				return Current.Game.tickManager;
			}
		}

		
		// (get) Token: 0x06002033 RID: 8243 RVA: 0x000C3D47 File Offset: 0x000C1F47
		public static Tutor Tutor
		{
			get
			{
				if (Current.Game == null)
				{
					return null;
				}
				return Current.Game.tutor;
			}
		}

		
		// (get) Token: 0x06002034 RID: 8244 RVA: 0x000C3D5C File Offset: 0x000C1F5C
		public static TutorialState TutorialState
		{
			get
			{
				return Current.Game.tutor.tutorialState;
			}
		}

		
		// (get) Token: 0x06002035 RID: 8245 RVA: 0x000C3D6D File Offset: 0x000C1F6D
		public static ActiveLessonHandler ActiveLesson
		{
			get
			{
				if (Current.Game == null)
				{
					return null;
				}
				return Current.Game.tutor.activeLesson;
			}
		}

		
		// (get) Token: 0x06002036 RID: 8246 RVA: 0x000C3D87 File Offset: 0x000C1F87
		public static Autosaver Autosaver
		{
			get
			{
				return Current.Game.autosaver;
			}
		}

		
		// (get) Token: 0x06002037 RID: 8247 RVA: 0x000C3D93 File Offset: 0x000C1F93
		public static DateNotifier DateNotifier
		{
			get
			{
				return Current.Game.dateNotifier;
			}
		}

		
		// (get) Token: 0x06002038 RID: 8248 RVA: 0x000C3D9F File Offset: 0x000C1F9F
		public static SignalManager SignalManager
		{
			get
			{
				return Current.Game.signalManager;
			}
		}

		
		// (get) Token: 0x06002039 RID: 8249 RVA: 0x000C3DAB File Offset: 0x000C1FAB
		public static UniqueIDsManager UniqueIDsManager
		{
			get
			{
				if (Current.Game == null)
				{
					return null;
				}
				return Current.Game.uniqueIDsManager;
			}
		}

		
		// (get) Token: 0x0600203A RID: 8250 RVA: 0x000C3DC0 File Offset: 0x000C1FC0
		public static QuestManager QuestManager
		{
			get
			{
				return Current.Game.questManager;
			}
		}

		
		// (get) Token: 0x0600203B RID: 8251 RVA: 0x000C3DCC File Offset: 0x000C1FCC
		public static FactionManager FactionManager
		{
			get
			{
				return Find.World.factionManager;
			}
		}

		
		// (get) Token: 0x0600203C RID: 8252 RVA: 0x000C3DD8 File Offset: 0x000C1FD8
		public static WorldPawns WorldPawns
		{
			get
			{
				return Find.World.worldPawns;
			}
		}

		
		// (get) Token: 0x0600203D RID: 8253 RVA: 0x000C3DE4 File Offset: 0x000C1FE4
		public static WorldObjectsHolder WorldObjects
		{
			get
			{
				return Find.World.worldObjects;
			}
		}

		
		// (get) Token: 0x0600203E RID: 8254 RVA: 0x000C3DF0 File Offset: 0x000C1FF0
		public static WorldGrid WorldGrid
		{
			get
			{
				return Find.World.grid;
			}
		}

		
		// (get) Token: 0x0600203F RID: 8255 RVA: 0x000C3DFC File Offset: 0x000C1FFC
		public static WorldDebugDrawer WorldDebugDrawer
		{
			get
			{
				return Find.World.debugDrawer;
			}
		}

		
		// (get) Token: 0x06002040 RID: 8256 RVA: 0x000C3E08 File Offset: 0x000C2008
		public static WorldPathGrid WorldPathGrid
		{
			get
			{
				return Find.World.pathGrid;
			}
		}

		
		// (get) Token: 0x06002041 RID: 8257 RVA: 0x000C3E14 File Offset: 0x000C2014
		public static WorldDynamicDrawManager WorldDynamicDrawManager
		{
			get
			{
				return Find.World.dynamicDrawManager;
			}
		}

		
		// (get) Token: 0x06002042 RID: 8258 RVA: 0x000C3E20 File Offset: 0x000C2020
		public static WorldPathFinder WorldPathFinder
		{
			get
			{
				return Find.World.pathFinder;
			}
		}

		
		// (get) Token: 0x06002043 RID: 8259 RVA: 0x000C3E2C File Offset: 0x000C202C
		public static WorldPathPool WorldPathPool
		{
			get
			{
				return Find.World.pathPool;
			}
		}

		
		// (get) Token: 0x06002044 RID: 8260 RVA: 0x000C3E38 File Offset: 0x000C2038
		public static WorldReachability WorldReachability
		{
			get
			{
				return Find.World.reachability;
			}
		}

		
		// (get) Token: 0x06002045 RID: 8261 RVA: 0x000C3E44 File Offset: 0x000C2044
		public static WorldFloodFiller WorldFloodFiller
		{
			get
			{
				return Find.World.floodFiller;
			}
		}

		
		// (get) Token: 0x06002046 RID: 8262 RVA: 0x000C3E50 File Offset: 0x000C2050
		public static WorldFeatures WorldFeatures
		{
			get
			{
				return Find.World.features;
			}
		}
		public static WorldInterface WorldInterface
		{
			get
			{
				return Find.World.UI;
			}
		}

		public static WorldSelector WorldSelector
		{
			get
			{
				return Find.WorldInterface.selector;
			}
		}

		
		// (get) Token: 0x06002049 RID: 8265 RVA: 0x000C3E74 File Offset: 0x000C2074
		public static WorldTargeter WorldTargeter
		{
			get
			{
				return Find.WorldInterface.targeter;
			}
		}


		public static WorldRoutePlanner WorldRoutePlanner
		{
			get
			{
				return Find.WorldInterface.routePlanner;
			}
		}
	}
}
