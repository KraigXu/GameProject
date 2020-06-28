using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using RimWorld.Planet;
using Verse.Profile;

namespace Verse
{
	// Token: 0x0200010D RID: 269
	public class Game : IExposable
	{
		// Token: 0x17000197 RID: 407
		// (get) Token: 0x06000775 RID: 1909 RVA: 0x0002269C File Offset: 0x0002089C
		// (set) Token: 0x06000776 RID: 1910 RVA: 0x000226A4 File Offset: 0x000208A4
		public Scenario Scenario
		{
			get
			{
				return this.scenarioInt;
			}
			set
			{
				this.scenarioInt = value;
			}
		}

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x06000777 RID: 1911 RVA: 0x000226AD File Offset: 0x000208AD
		// (set) Token: 0x06000778 RID: 1912 RVA: 0x000226B5 File Offset: 0x000208B5
		public World World
		{
			get
			{
				return this.worldInt;
			}
			set
			{
				if (this.worldInt == value)
				{
					return;
				}
				this.worldInt = value;
			}
		}

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x06000779 RID: 1913 RVA: 0x000226C8 File Offset: 0x000208C8
		// (set) Token: 0x0600077A RID: 1914 RVA: 0x000226E8 File Offset: 0x000208E8
		public Map CurrentMap
		{
			get
			{
				if (this.currentMapIndex < 0)
				{
					return null;
				}
				return this.maps[(int)this.currentMapIndex];
			}
			set
			{
				int num;
				if (value == null)
				{
					num = -1;
				}
				else
				{
					num = this.maps.IndexOf(value);
					if (num < 0)
					{
						Log.Error("Could not set current map because it does not exist.", false);
						return;
					}
				}
				if ((int)this.currentMapIndex != num)
				{
					this.currentMapIndex = (sbyte)num;
					Find.MapUI.Notify_SwitchedMap();
					AmbientSoundManager.Notify_SwitchedMap();
				}
			}
		}

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x0600077B RID: 1915 RVA: 0x0002273C File Offset: 0x0002093C
		public Map AnyPlayerHomeMap
		{
			get
			{
				if (Faction.OfPlayerSilentFail == null)
				{
					return null;
				}
				for (int i = 0; i < this.maps.Count; i++)
				{
					Map map = this.maps[i];
					if (map.IsPlayerHome)
					{
						return map;
					}
				}
				return null;
			}
		}

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x0600077C RID: 1916 RVA: 0x00022780 File Offset: 0x00020980
		public Map RandomPlayerHomeMap
		{
			get
			{
				if (Faction.OfPlayerSilentFail == null)
				{
					return null;
				}
				Game.tmpPlayerHomeMaps.Clear();
				for (int i = 0; i < this.maps.Count; i++)
				{
					Map map = this.maps[i];
					if (map.IsPlayerHome)
					{
						Game.tmpPlayerHomeMaps.Add(map);
					}
				}
				if (Game.tmpPlayerHomeMaps.Any<Map>())
				{
					Map result = Game.tmpPlayerHomeMaps.RandomElement<Map>();
					Game.tmpPlayerHomeMaps.Clear();
					return result;
				}
				return null;
			}
		}

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x0600077D RID: 1917 RVA: 0x000227F8 File Offset: 0x000209F8
		public List<Map> Maps
		{
			get
			{
				return this.maps;
			}
		}

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x0600077E RID: 1918 RVA: 0x00022800 File Offset: 0x00020A00
		// (set) Token: 0x0600077F RID: 1919 RVA: 0x00022808 File Offset: 0x00020A08
		public GameInitData InitData
		{
			get
			{
				return this.initData;
			}
			set
			{
				this.initData = value;
			}
		}

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x06000780 RID: 1920 RVA: 0x00022811 File Offset: 0x00020A11
		public GameInfo Info
		{
			get
			{
				return this.info;
			}
		}

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x06000781 RID: 1921 RVA: 0x00022819 File Offset: 0x00020A19
		public GameRules Rules
		{
			get
			{
				return this.rules;
			}
		}

		// Token: 0x06000782 RID: 1922 RVA: 0x00022824 File Offset: 0x00020A24
		public Game()
		{
			this.FillComponents();
		}

		// Token: 0x06000783 RID: 1923 RVA: 0x0002294C File Offset: 0x00020B4C
		public void AddMap(Map map)
		{
			if (map == null)
			{
				Log.Error("Tried to add null map.", false);
				return;
			}
			if (this.maps.Contains(map))
			{
				Log.Error("Tried to add map but it's already here.", false);
				return;
			}
			if (this.maps.Count > 127)
			{
				Log.Error("Can't add map. Reached maps count limit (" + sbyte.MaxValue + ").", false);
				return;
			}
			this.maps.Add(map);
			Find.ColonistBar.MarkColonistsDirty();
		}

		// Token: 0x06000784 RID: 1924 RVA: 0x000229C4 File Offset: 0x00020BC4
		public Map FindMap(MapParent mapParent)
		{
			for (int i = 0; i < this.maps.Count; i++)
			{
				if (this.maps[i].info.parent == mapParent)
				{
					return this.maps[i];
				}
			}
			return null;
		}

		// Token: 0x06000785 RID: 1925 RVA: 0x00022A10 File Offset: 0x00020C10
		public Map FindMap(int tile)
		{
			for (int i = 0; i < this.maps.Count; i++)
			{
				if (this.maps[i].Tile == tile)
				{
					return this.maps[i];
				}
			}
			return null;
		}

		// Token: 0x06000786 RID: 1926 RVA: 0x00022A58 File Offset: 0x00020C58
		public void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				Log.Error("You must use special LoadData method to load Game.", false);
				return;
			}
			Scribe_Values.Look<sbyte>(ref this.currentMapIndex, "currentMapIndex", -1, false);
			this.ExposeSmallComponents();
			Scribe_Deep.Look<World>(ref this.worldInt, "world", Array.Empty<object>());
			Scribe_Collections.Look<Map>(ref this.maps, "maps", LookMode.Deep, Array.Empty<object>());
			Find.CameraDriver.Expose();
		}

		// Token: 0x06000787 RID: 1927 RVA: 0x00022AC8 File Offset: 0x00020CC8
		private void ExposeSmallComponents()
		{
			Scribe_Deep.Look<GameInfo>(ref this.info, "info", Array.Empty<object>());
			Scribe_Deep.Look<GameRules>(ref this.rules, "rules", Array.Empty<object>());
			Scribe_Deep.Look<Scenario>(ref this.scenarioInt, "scenario", Array.Empty<object>());
			Scribe_Deep.Look<TickManager>(ref this.tickManager, "tickManager", Array.Empty<object>());
			Scribe_Deep.Look<PlaySettings>(ref this.playSettings, "playSettings", Array.Empty<object>());
			Scribe_Deep.Look<StoryWatcher>(ref this.storyWatcher, "storyWatcher", Array.Empty<object>());
			Scribe_Deep.Look<GameEnder>(ref this.gameEnder, "gameEnder", Array.Empty<object>());
			Scribe_Deep.Look<LetterStack>(ref this.letterStack, "letterStack", Array.Empty<object>());
			Scribe_Deep.Look<ResearchManager>(ref this.researchManager, "researchManager", Array.Empty<object>());
			Scribe_Deep.Look<Storyteller>(ref this.storyteller, "storyteller", Array.Empty<object>());
			Scribe_Deep.Look<History>(ref this.history, "history", Array.Empty<object>());
			Scribe_Deep.Look<TaleManager>(ref this.taleManager, "taleManager", Array.Empty<object>());
			Scribe_Deep.Look<PlayLog>(ref this.playLog, "playLog", Array.Empty<object>());
			Scribe_Deep.Look<BattleLog>(ref this.battleLog, "battleLog", Array.Empty<object>());
			Scribe_Deep.Look<OutfitDatabase>(ref this.outfitDatabase, "outfitDatabase", Array.Empty<object>());
			Scribe_Deep.Look<DrugPolicyDatabase>(ref this.drugPolicyDatabase, "drugPolicyDatabase", Array.Empty<object>());
			Scribe_Deep.Look<FoodRestrictionDatabase>(ref this.foodRestrictionDatabase, "foodRestrictionDatabase", Array.Empty<object>());
			Scribe_Deep.Look<Tutor>(ref this.tutor, "tutor", Array.Empty<object>());
			Scribe_Deep.Look<DateNotifier>(ref this.dateNotifier, "dateNotifier", Array.Empty<object>());
			Scribe_Deep.Look<UniqueIDsManager>(ref this.uniqueIDsManager, "uniqueIDsManager", Array.Empty<object>());
			Scribe_Deep.Look<QuestManager>(ref this.questManager, "questManager", Array.Empty<object>());
			Scribe_Collections.Look<GameComponent>(ref this.components, "components", LookMode.Deep, new object[]
			{
				this
			});
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.FillComponents();
				if (this.rules == null)
				{
					Log.Warning("Save game was missing rules. Replacing with a blank GameRules.", false);
					this.rules = new GameRules();
				}
			}
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x06000788 RID: 1928 RVA: 0x00022CDC File Offset: 0x00020EDC
		private void FillComponents()
		{
			this.components.RemoveAll((GameComponent component) => component == null);
			foreach (Type type in typeof(GameComponent).AllSubclassesNonAbstract())
			{
				if (this.GetComponent(type) == null)
				{
					try
					{
						GameComponent item = (GameComponent)Activator.CreateInstance(type, new object[]
						{
							this
						});
						this.components.Add(item);
					}
					catch (Exception ex)
					{
						Log.Error(string.Concat(new object[]
						{
							"Could not instantiate a GameComponent of type ",
							type,
							": ",
							ex
						}), false);
					}
				}
			}
		}

		// Token: 0x06000789 RID: 1929 RVA: 0x00022DBC File Offset: 0x00020FBC
		public void InitNewGame()
		{
			string str = (from mod in LoadedModManager.RunningMods
			select mod.PackageIdPlayerFacing).ToLineList("  - ", false);
			Log.Message("Initializing new game with mods:\n" + str, false);
			if (this.maps.Any<Map>())
			{
				Log.Error("Called InitNewGame() but there already is a map. There should be 0 maps...", false);
				return;
			}
			if (this.initData == null)
			{
				Log.Error("Called InitNewGame() but init data is null. Create it first.", false);
				return;
			}
			MemoryUtility.UnloadUnusedUnityAssets();
			DeepProfiler.Start("InitNewGame");
			try
			{
				Current.ProgramState = ProgramState.MapInitializing;
				IntVec3 intVec = new IntVec3(this.initData.mapSize, 1, this.initData.mapSize);
				Settlement settlement = null;
				List<Settlement> settlements = Find.WorldObjects.Settlements;
				for (int i = 0; i < settlements.Count; i++)
				{
					if (settlements[i].Faction == Faction.OfPlayer)
					{
						settlement = settlements[i];
						break;
					}
				}
				if (settlement == null)
				{
					Log.Error("Could not generate starting map because there is no any player faction base.", false);
				}
				this.tickManager.gameStartAbsTick = GenTicks.ConfiguredTicksAbsAtGameStart;
				Map currentMap = MapGenerator.GenerateMap(intVec, settlement, settlement.MapGeneratorDef, settlement.ExtraGenStepDefs, null);
				this.worldInt.info.initialMapSize = intVec;
				if (this.initData.permadeath)
				{
					this.info.permadeathMode = true;
					this.info.permadeathModeUniqueName = PermadeathModeUtility.GeneratePermadeathSaveName();
				}
				PawnUtility.GiveAllStartingPlayerPawnsThought(ThoughtDefOf.NewColonyOptimism);
				this.FinalizeInit();
				Current.Game.CurrentMap = currentMap;
				Find.CameraDriver.JumpToCurrentMapLoc(MapGenerator.PlayerStartSpot);
				Find.CameraDriver.ResetSize();
				if (Prefs.PauseOnLoad && this.initData.startedFromEntry)
				{
					LongEventHandler.ExecuteWhenFinished(delegate
					{
						this.tickManager.DoSingleTick();
						this.tickManager.CurTimeSpeed = TimeSpeed.Paused;
					});
				}
				Find.Scenario.PostGameStart();
				if (Faction.OfPlayer.def.startingResearchTags != null)
				{
					foreach (ResearchProjectTagDef tag in Faction.OfPlayer.def.startingResearchTags)
					{
						foreach (ResearchProjectDef researchProjectDef in DefDatabase<ResearchProjectDef>.AllDefs)
						{
							if (researchProjectDef.HasTag(tag))
							{
								this.researchManager.FinishProject(researchProjectDef, false, null);
							}
						}
					}
				}
				if (Faction.OfPlayer.def.startingTechprintsResearchTags != null)
				{
					foreach (ResearchProjectTagDef tag2 in Faction.OfPlayer.def.startingTechprintsResearchTags)
					{
						foreach (ResearchProjectDef researchProjectDef2 in DefDatabase<ResearchProjectDef>.AllDefs)
						{
							if (researchProjectDef2.HasTag(tag2))
							{
								int techprints = this.researchManager.GetTechprints(researchProjectDef2);
								if (techprints < researchProjectDef2.techprintCount)
								{
									this.researchManager.AddTechprints(researchProjectDef2, researchProjectDef2.techprintCount - techprints);
								}
							}
						}
					}
				}
				GameComponentUtility.StartedNewGame();
				this.initData = null;
			}
			finally
			{
				DeepProfiler.End();
			}
		}

		// Token: 0x0600078A RID: 1930 RVA: 0x00023160 File Offset: 0x00021360
		public void LoadGame()
		{
			if (this.maps.Any<Map>())
			{
				Log.Error("Called LoadGame() but there already is a map. There should be 0 maps...", false);
				return;
			}
			MemoryUtility.UnloadUnusedUnityAssets();
			BackCompatibility.PreLoadSavegame(ScribeMetaHeaderUtility.loadedGameVersion);
			Current.ProgramState = ProgramState.MapInitializing;
			this.ExposeSmallComponents();
			LongEventHandler.SetCurrentEventText("LoadingWorld".Translate());
			if (Scribe.EnterNode("world"))
			{
				try
				{
					this.World = new World();
					this.World.ExposeData();
					goto IL_7E;
				}
				finally
				{
					Scribe.ExitNode();
				}
				goto IL_72;
				IL_7E:
				this.World.FinalizeInit();
				LongEventHandler.SetCurrentEventText("LoadingMap".Translate());
				Scribe_Collections.Look<Map>(ref this.maps, "maps", LookMode.Deep, Array.Empty<object>());
				if (this.maps.RemoveAll((Map x) => x == null) != 0)
				{
					Log.Warning("Some maps were null after loading.", false);
				}
				int num = -1;
				Scribe_Values.Look<int>(ref num, "currentMapIndex", -1, false);
				if (num < 0 && this.maps.Any<Map>())
				{
					Log.Error("Current map is null after loading but there are maps available. Setting current map to [0].", false);
					num = 0;
				}
				if (num >= this.maps.Count)
				{
					Log.Error("Current map index out of bounds after loading.", false);
					if (this.maps.Any<Map>())
					{
						num = 0;
					}
					else
					{
						num = -1;
					}
				}
				this.currentMapIndex = sbyte.MinValue;
				this.CurrentMap = ((num >= 0) ? this.maps[num] : null);
				LongEventHandler.SetCurrentEventText("InitializingGame".Translate());
				Find.CameraDriver.Expose();
				DeepProfiler.Start("FinalizeLoading");
				Scribe.loader.FinalizeLoading();
				DeepProfiler.End();
				LongEventHandler.SetCurrentEventText("SpawningAllThings".Translate());
				for (int i = 0; i < this.maps.Count; i++)
				{
					try
					{
						this.maps[i].FinalizeLoading();
					}
					catch (Exception arg)
					{
						Log.Error("Error in Map.FinalizeLoading(): " + arg, false);
					}
					try
					{
						this.maps[i].Parent.FinalizeLoading();
					}
					catch (Exception arg2)
					{
						Log.Error("Error in MapParent.FinalizeLoading(): " + arg2, false);
					}
				}
				this.FinalizeInit();
				if (Prefs.PauseOnLoad)
				{
					LongEventHandler.ExecuteWhenFinished(delegate
					{
						Find.TickManager.DoSingleTick();
						Find.TickManager.CurTimeSpeed = TimeSpeed.Paused;
					});
				}
				GameComponentUtility.LoadedGame();
				BackCompatibility.PostLoadSavegame(ScribeMetaHeaderUtility.loadedGameVersion);
				return;
			}
			IL_72:
			Log.Error("Could not find world XML node.", false);
		}

		// Token: 0x0600078B RID: 1931 RVA: 0x000233F0 File Offset: 0x000215F0
		public void UpdateEntry()
		{
			GameComponentUtility.GameComponentUpdate();
		}

		// Token: 0x0600078C RID: 1932 RVA: 0x000233F8 File Offset: 0x000215F8
		public void UpdatePlay()
		{
			this.tickManager.TickManagerUpdate();
			this.letterStack.LetterStackUpdate();
			this.World.WorldUpdate();
			for (int i = 0; i < this.maps.Count; i++)
			{
				this.maps[i].MapUpdate();
			}
			this.Info.GameInfoUpdate();
			GameComponentUtility.GameComponentUpdate();
			this.signalManager.SignalManagerUpdate();
		}

		// Token: 0x0600078D RID: 1933 RVA: 0x00023468 File Offset: 0x00021668
		public T GetComponent<T>() where T : GameComponent
		{
			for (int i = 0; i < this.components.Count; i++)
			{
				T t = this.components[i] as T;
				if (t != null)
				{
					return t;
				}
			}
			return default(T);
		}

		// Token: 0x0600078E RID: 1934 RVA: 0x000234B8 File Offset: 0x000216B8
		public GameComponent GetComponent(Type type)
		{
			for (int i = 0; i < this.components.Count; i++)
			{
				if (type.IsAssignableFrom(this.components[i].GetType()))
				{
					return this.components[i];
				}
			}
			return null;
		}

		// Token: 0x0600078F RID: 1935 RVA: 0x00023502 File Offset: 0x00021702
		public void FinalizeInit()
		{
			LogSimple.FlushToFileAndOpen();
			this.researchManager.ReapplyAllMods();
			MessagesRepeatAvoider.Reset();
			GameComponentUtility.FinalizeInit();
			this.history.FinalizeInit();
			Current.ProgramState = ProgramState.Playing;
		}

		// Token: 0x06000790 RID: 1936 RVA: 0x00023530 File Offset: 0x00021730
		public void DeinitAndRemoveMap(Map map)
		{
			if (map == null)
			{
				Log.Error("Tried to remove null map.", false);
				return;
			}
			if (!this.maps.Contains(map))
			{
				Log.Error("Tried to remove map " + map + " but it's not here.", false);
				return;
			}
			if (map.Parent != null)
			{
				map.Parent.Notify_MyMapAboutToBeRemoved();
			}
			Map currentMap = this.CurrentMap;
			MapDeiniter.Deinit(map);
			this.maps.Remove(map);
			if (currentMap != null)
			{
				sbyte b = (sbyte)this.maps.IndexOf(currentMap);
				if (b < 0)
				{
					if (this.maps.Any<Map>())
					{
						this.CurrentMap = this.maps[0];
					}
					else
					{
						this.CurrentMap = null;
					}
					Find.World.renderer.wantedMode = WorldRenderMode.Planet;
				}
				else
				{
					this.currentMapIndex = b;
				}
			}
			if (Current.ProgramState == ProgramState.Playing)
			{
				Find.ColonistBar.MarkColonistsDirty();
			}
			MapComponentUtility.MapRemoved(map);
			if (map.Parent != null)
			{
				map.Parent.Notify_MyMapRemoved(map);
			}
		}

		// Token: 0x06000791 RID: 1937 RVA: 0x00023620 File Offset: 0x00021820
		public string DebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Game debug data:");
			stringBuilder.AppendLine("initData:");
			if (this.initData == null)
			{
				stringBuilder.AppendLine("   null");
			}
			else
			{
				stringBuilder.AppendLine(this.initData.ToString());
			}
			stringBuilder.AppendLine("Scenario:");
			if (this.scenarioInt == null)
			{
				stringBuilder.AppendLine("   null");
			}
			else
			{
				stringBuilder.AppendLine("   " + this.scenarioInt.ToString());
			}
			stringBuilder.AppendLine("World:");
			if (this.worldInt == null)
			{
				stringBuilder.AppendLine("   null");
			}
			else
			{
				stringBuilder.AppendLine("   name: " + this.worldInt.info.name);
			}
			stringBuilder.AppendLine("Maps count: " + this.maps.Count);
			for (int i = 0; i < this.maps.Count; i++)
			{
				stringBuilder.AppendLine("   Map " + this.maps[i].Index + ":");
				stringBuilder.AppendLine("      tile: " + this.maps[i].TileInfo);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x040006CA RID: 1738
		private GameInitData initData;

		// Token: 0x040006CB RID: 1739
		public sbyte currentMapIndex = -1;

		// Token: 0x040006CC RID: 1740
		private GameInfo info = new GameInfo();

		// Token: 0x040006CD RID: 1741
		public List<GameComponent> components = new List<GameComponent>();

		// Token: 0x040006CE RID: 1742
		private GameRules rules = new GameRules();

		// Token: 0x040006CF RID: 1743
		private Scenario scenarioInt;

		// Token: 0x040006D0 RID: 1744
		private World worldInt;

		// Token: 0x040006D1 RID: 1745
		private List<Map> maps = new List<Map>();

		// Token: 0x040006D2 RID: 1746
		public PlaySettings playSettings = new PlaySettings();

		// Token: 0x040006D3 RID: 1747
		public StoryWatcher storyWatcher = new StoryWatcher();

		// Token: 0x040006D4 RID: 1748
		public LetterStack letterStack = new LetterStack();

		// Token: 0x040006D5 RID: 1749
		public ResearchManager researchManager = new ResearchManager();

		// Token: 0x040006D6 RID: 1750
		public GameEnder gameEnder = new GameEnder();

		// Token: 0x040006D7 RID: 1751
		public Storyteller storyteller = new Storyteller();

		// Token: 0x040006D8 RID: 1752
		public History history = new History();

		// Token: 0x040006D9 RID: 1753
		public TaleManager taleManager = new TaleManager();

		// Token: 0x040006DA RID: 1754
		public PlayLog playLog = new PlayLog();

		// Token: 0x040006DB RID: 1755
		public BattleLog battleLog = new BattleLog();

		// Token: 0x040006DC RID: 1756
		public OutfitDatabase outfitDatabase = new OutfitDatabase();

		// Token: 0x040006DD RID: 1757
		public DrugPolicyDatabase drugPolicyDatabase = new DrugPolicyDatabase();

		// Token: 0x040006DE RID: 1758
		public FoodRestrictionDatabase foodRestrictionDatabase = new FoodRestrictionDatabase();

		// Token: 0x040006DF RID: 1759
		public TickManager tickManager = new TickManager();

		// Token: 0x040006E0 RID: 1760
		public Tutor tutor = new Tutor();

		// Token: 0x040006E1 RID: 1761
		public Autosaver autosaver = new Autosaver();

		// Token: 0x040006E2 RID: 1762
		public DateNotifier dateNotifier = new DateNotifier();

		// Token: 0x040006E3 RID: 1763
		public SignalManager signalManager = new SignalManager();

		// Token: 0x040006E4 RID: 1764
		public UniqueIDsManager uniqueIDsManager = new UniqueIDsManager();

		// Token: 0x040006E5 RID: 1765
		public QuestManager questManager = new QuestManager();

		// Token: 0x040006E6 RID: 1766
		private static List<Map> tmpPlayerHomeMaps = new List<Map>();
	}
}
