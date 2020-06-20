using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using Verse.AI;
using Verse.AI.Group;

namespace Verse
{
	// Token: 0x02000180 RID: 384
	public sealed class Map : IIncidentTarget, ILoadReferenceable, IThingHolder, IExposable
	{
		// Token: 0x17000224 RID: 548
		// (get) Token: 0x06000B09 RID: 2825 RVA: 0x0003AC98 File Offset: 0x00038E98
		public int Index
		{
			get
			{
				return Find.Maps.IndexOf(this);
			}
		}

		// Token: 0x17000225 RID: 549
		// (get) Token: 0x06000B0A RID: 2826 RVA: 0x0003ACA5 File Offset: 0x00038EA5
		public IntVec3 Size
		{
			get
			{
				return this.info.Size;
			}
		}

		// Token: 0x17000226 RID: 550
		// (get) Token: 0x06000B0B RID: 2827 RVA: 0x0003ACB2 File Offset: 0x00038EB2
		public IntVec3 Center
		{
			get
			{
				return new IntVec3(this.Size.x / 2, 0, this.Size.z / 2);
			}
		}

		// Token: 0x17000227 RID: 551
		// (get) Token: 0x06000B0C RID: 2828 RVA: 0x0003ACD4 File Offset: 0x00038ED4
		public Faction ParentFaction
		{
			get
			{
				return this.info.parent.Faction;
			}
		}

		// Token: 0x17000228 RID: 552
		// (get) Token: 0x06000B0D RID: 2829 RVA: 0x0003ACE6 File Offset: 0x00038EE6
		public int Area
		{
			get
			{
				return this.Size.x * this.Size.z;
			}
		}

		// Token: 0x17000229 RID: 553
		// (get) Token: 0x06000B0E RID: 2830 RVA: 0x0003ACFF File Offset: 0x00038EFF
		public IThingHolder ParentHolder
		{
			get
			{
				return this.info.parent;
			}
		}

		// Token: 0x1700022A RID: 554
		// (get) Token: 0x06000B0F RID: 2831 RVA: 0x0003AD0C File Offset: 0x00038F0C
		public IEnumerable<IntVec3> AllCells
		{
			get
			{
				int num;
				for (int z = 0; z < this.Size.z; z = num + 1)
				{
					for (int y = 0; y < this.Size.y; y = num + 1)
					{
						for (int x = 0; x < this.Size.x; x = num + 1)
						{
							yield return new IntVec3(x, y, z);
							num = x;
						}
						num = y;
					}
					num = z;
				}
				yield break;
			}
		}

		// Token: 0x1700022B RID: 555
		// (get) Token: 0x06000B10 RID: 2832 RVA: 0x0003AD1C File Offset: 0x00038F1C
		public bool IsPlayerHome
		{
			get
			{
				return this.info != null && this.info.parent.def.canBePlayerHome && this.info.parent.Faction == Faction.OfPlayer;
			}
		}

		// Token: 0x1700022C RID: 556
		// (get) Token: 0x06000B11 RID: 2833 RVA: 0x0003AD56 File Offset: 0x00038F56
		public bool IsTempIncidentMap
		{
			get
			{
				return this.info.parent.def.isTempIncidentMapOwner;
			}
		}

		// Token: 0x06000B12 RID: 2834 RVA: 0x0003AD6D File Offset: 0x00038F6D
		public IEnumerator<IntVec3> GetEnumerator()
		{
			foreach (IntVec3 intVec in this.AllCells)
			{
				yield return intVec;
			}
			IEnumerator<IntVec3> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x1700022D RID: 557
		// (get) Token: 0x06000B13 RID: 2835 RVA: 0x0003AD7C File Offset: 0x00038F7C
		public int Tile
		{
			get
			{
				return this.info.Tile;
			}
		}

		// Token: 0x1700022E RID: 558
		// (get) Token: 0x06000B14 RID: 2836 RVA: 0x0003AD89 File Offset: 0x00038F89
		public Tile TileInfo
		{
			get
			{
				return Find.WorldGrid[this.Tile];
			}
		}

		// Token: 0x1700022F RID: 559
		// (get) Token: 0x06000B15 RID: 2837 RVA: 0x0003AD9B File Offset: 0x00038F9B
		public BiomeDef Biome
		{
			get
			{
				return this.TileInfo.biome;
			}
		}

		// Token: 0x17000230 RID: 560
		// (get) Token: 0x06000B16 RID: 2838 RVA: 0x0003ADA8 File Offset: 0x00038FA8
		public StoryState StoryState
		{
			get
			{
				return this.storyState;
			}
		}

		// Token: 0x17000231 RID: 561
		// (get) Token: 0x06000B17 RID: 2839 RVA: 0x0003ADB0 File Offset: 0x00038FB0
		public GameConditionManager GameConditionManager
		{
			get
			{
				return this.gameConditionManager;
			}
		}

		// Token: 0x17000232 RID: 562
		// (get) Token: 0x06000B18 RID: 2840 RVA: 0x0003ADB8 File Offset: 0x00038FB8
		public float PlayerWealthForStoryteller
		{
			get
			{
				if (this.IsPlayerHome)
				{
					return this.wealthWatcher.WealthItems + this.wealthWatcher.WealthBuildings * 0.5f + this.wealthWatcher.WealthPawns;
				}
				float num = 0f;
				foreach (Pawn pawn in this.mapPawns.PawnsInFaction(Faction.OfPlayer))
				{
					if (pawn.IsFreeColonist)
					{
						num += WealthWatcher.GetEquipmentApparelAndInventoryWealth(pawn);
					}
					if (pawn.RaceProps.Animal)
					{
						num += pawn.MarketValue;
					}
				}
				return num;
			}
		}

		// Token: 0x17000233 RID: 563
		// (get) Token: 0x06000B19 RID: 2841 RVA: 0x0003AE70 File Offset: 0x00039070
		public IEnumerable<Pawn> PlayerPawnsForStoryteller
		{
			get
			{
				return this.mapPawns.PawnsInFaction(Faction.OfPlayer);
			}
		}

		// Token: 0x17000234 RID: 564
		// (get) Token: 0x06000B1A RID: 2842 RVA: 0x0003AE82 File Offset: 0x00039082
		public FloatRange IncidentPointsRandomFactorRange
		{
			get
			{
				return FloatRange.One;
			}
		}

		// Token: 0x17000235 RID: 565
		// (get) Token: 0x06000B1B RID: 2843 RVA: 0x0003ACFF File Offset: 0x00038EFF
		public MapParent Parent
		{
			get
			{
				return this.info.parent;
			}
		}

		// Token: 0x06000B1C RID: 2844 RVA: 0x0003AE89 File Offset: 0x00039089
		public IEnumerable<IncidentTargetTagDef> IncidentTargetTags()
		{
			return this.info.parent.IncidentTargetTags();
		}

		public void ConstructComponents()
		{
			this.spawnedThings = new ThingOwner<Thing>(this);
			this.cellIndices = new CellIndices(this);
			this.listerThings = new ListerThings(ListerThingsUse.Global);
			this.listerBuildings = new ListerBuildings();
			this.mapPawns = new MapPawns(this);
			this.dynamicDrawManager = new DynamicDrawManager(this);
			this.mapDrawer = new MapDrawer(this);
			this.tooltipGiverList = new TooltipGiverList();
			this.pawnDestinationReservationManager = new PawnDestinationReservationManager();
			this.reservationManager = new ReservationManager(this);
			this.physicalInteractionReservationManager = new PhysicalInteractionReservationManager();
			this.designationManager = new DesignationManager(this);
			this.lordManager = new LordManager(this);
			this.debugDrawer = new DebugCellDrawer();
			this.passingShipManager = new PassingShipManager(this);
			this.haulDestinationManager = new HaulDestinationManager(this);
			this.gameConditionManager = new GameConditionManager(this);
			this.weatherManager = new WeatherManager(this);
			this.zoneManager = new ZoneManager(this);
			this.resourceCounter = new ResourceCounter(this);
			this.mapTemperature = new MapTemperature(this);
			this.temperatureCache = new TemperatureCache(this);
			this.areaManager = new AreaManager(this);
			this.attackTargetsCache = new AttackTargetsCache(this);
			this.attackTargetReservationManager = new AttackTargetReservationManager(this);
			this.lordsStarter = new VoluntarilyJoinableLordsStarter(this);
			this.thingGrid = new ThingGrid(this);
			this.coverGrid = new CoverGrid(this);
			this.edificeGrid = new EdificeGrid(this);
			this.blueprintGrid = new BlueprintGrid(this);
			this.fogGrid = new FogGrid(this);
			this.glowGrid = new GlowGrid(this);
			this.regionGrid = new RegionGrid(this);
			this.terrainGrid = new TerrainGrid(this);
			this.pathGrid = new PathGrid(this);
			this.roofGrid = new RoofGrid(this);
			this.fertilityGrid = new FertilityGrid(this);
			this.snowGrid = new SnowGrid(this);
			this.deepResourceGrid = new DeepResourceGrid(this);
			this.exitMapGrid = new ExitMapGrid(this);
			this.avoidGrid = new AvoidGrid(this);
			this.linkGrid = new LinkGrid(this);
			this.glowFlooder = new GlowFlooder(this);
			this.powerNetManager = new PowerNetManager(this);
			this.powerNetGrid = new PowerNetGrid(this);
			this.regionMaker = new RegionMaker(this);
			this.pathFinder = new PathFinder(this);
			this.pawnPathPool = new PawnPathPool(this);
			this.regionAndRoomUpdater = new RegionAndRoomUpdater(this);
			this.regionLinkDatabase = new RegionLinkDatabase();
			this.moteCounter = new MoteCounter();
			this.gatherSpotLister = new GatherSpotLister();
			this.windManager = new WindManager(this);
			this.listerBuildingsRepairable = new ListerBuildingsRepairable();
			this.listerHaulables = new ListerHaulables(this);
			this.listerMergeables = new ListerMergeables(this);
			this.listerFilthInHomeArea = new ListerFilthInHomeArea(this);
			this.listerArtificialBuildingsForMeditation = new ListerArtificialBuildingsForMeditation(this);
			this.reachability = new Reachability(this);
			this.itemAvailability = new ItemAvailability(this);
			this.autoBuildRoofAreaSetter = new AutoBuildRoofAreaSetter(this);
			this.roofCollapseBufferResolver = new RoofCollapseBufferResolver(this);
			this.roofCollapseBuffer = new RoofCollapseBuffer();
			this.wildAnimalSpawner = new WildAnimalSpawner(this);
			this.wildPlantSpawner = new WildPlantSpawner(this);
			this.steadyEnvironmentEffects = new SteadyEnvironmentEffects(this);
			this.skyManager = new SkyManager(this);
			this.overlayDrawer = new OverlayDrawer();
			this.floodFiller = new FloodFiller(this);
			this.weatherDecider = new WeatherDecider(this);
			this.fireWatcher = new FireWatcher(this);
			this.dangerWatcher = new DangerWatcher(this);
			this.damageWatcher = new DamageWatcher();
			this.strengthWatcher = new StrengthWatcher(this);
			this.wealthWatcher = new WealthWatcher(this);
			this.regionDirtyer = new RegionDirtyer(this);
			this.cellsInRandomOrder = new MapCellsInRandomOrder(this);
			this.rememberedCameraPos = new RememberedCameraPos(this);
			this.mineStrikeManager = new MineStrikeManager();
			this.storyState = new StoryState(this);
			this.retainedCaravanData = new RetainedCaravanData(this);
			this.components.Clear();
			this.FillComponents();
		}
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.uniqueID, "uniqueID", -1, false);
			Scribe_Values.Look<int>(ref this.generationTick, "generationTick", 0, false);
			Scribe_Deep.Look<MapInfo>(ref this.info, "mapInfo", Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.compressor = new MapFileCompressor(this);
				this.compressor.BuildCompressedString();
				this.ExposeComponents();
				this.compressor.ExposeData();
				HashSet<string> hashSet = new HashSet<string>();
				if (Scribe.EnterNode("things"))
				{
					try
					{
						using (List<Thing>.Enumerator enumerator = this.listerThings.AllThings.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								Thing thing = enumerator.Current;
								try
								{
									if (thing.def.isSaveable && !thing.IsSaveCompressible())
									{
										if (hashSet.Contains(thing.ThingID))
										{
											Log.Error("Saving Thing with already-used ID " + thing.ThingID, false);
										}
										else
										{
											hashSet.Add(thing.ThingID);
										}
										Thing thing2 = thing;
										Scribe_Deep.Look<Thing>(ref thing2, "thing", Array.Empty<object>());
									}
								}
								catch (OutOfMemoryException)
								{
									throw;
								}
								catch (Exception ex)
								{
									Log.Error(string.Concat(new object[]
									{
										"Exception saving ",
										thing,
										": ",
										ex
									}), false);
								}
							}
							goto IL_15A;
						}
					}
					finally
					{
						Scribe.ExitNode();
					}
				}
				Log.Error("Could not enter the things node while saving.", false);
				IL_15A:
				this.compressor = null;
				return;
			}
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.ConstructComponents();
				this.regionAndRoomUpdater.Enabled = false;
				this.compressor = new MapFileCompressor(this);
			}
			this.ExposeComponents();
			DeepProfiler.Start("Load compressed things");
			this.compressor.ExposeData();
			DeepProfiler.End();
			DeepProfiler.Start("Load non-compressed things");
			Scribe_Collections.Look<Thing>(ref this.loadedFullThings, "things", LookMode.Deep, Array.Empty<object>());
			DeepProfiler.End();
		}

		// Token: 0x06000B1F RID: 2847 RVA: 0x0003B48C File Offset: 0x0003968C
		private void FillComponents()
		{
			this.components.RemoveAll((MapComponent component) => component == null);
			foreach (Type type in typeof(MapComponent).AllSubclassesNonAbstract())
			{
				if (this.GetComponent(type) == null)
				{
					try
					{
						MapComponent item = (MapComponent)Activator.CreateInstance(type, new object[]
						{
							this
						});
						this.components.Add(item);
					}
					catch (Exception ex)
					{
						Log.Error(string.Concat(new object[]
						{
							"Could not instantiate a MapComponent of type ",
							type,
							": ",
							ex
						}), false);
					}
				}
			}
			this.roadInfo = this.GetComponent<RoadInfo>();
			this.waterInfo = this.GetComponent<WaterInfo>();
		}

		// Token: 0x06000B20 RID: 2848 RVA: 0x0003B584 File Offset: 0x00039784
		public void FinalizeLoading()
		{
			List<Thing> list = this.compressor.ThingsToSpawnAfterLoad().ToList<Thing>();
			this.compressor = null;
			DeepProfiler.Start("Merge compressed and non-compressed thing lists");
			List<Thing> list2 = new List<Thing>(this.loadedFullThings.Count + list.Count);
			foreach (Thing item in this.loadedFullThings.Concat(list))
			{
				list2.Add(item);
			}
			this.loadedFullThings.Clear();
			DeepProfiler.End();
			DeepProfiler.Start("Spawn everything into the map");
			BackCompatibility.PreCheckSpawnBackCompatibleThingAfterLoading(this);
			foreach (Thing thing in list2)
			{
				if (!(thing is Building))
				{
					try
					{
						if (!BackCompatibility.CheckSpawnBackCompatibleThingAfterLoading(thing, this))
						{
							GenSpawn.Spawn(thing, thing.Position, this, thing.Rotation, WipeMode.FullRefund, true);
						}
					}
					catch (Exception ex)
					{
						Log.Error(string.Concat(new object[]
						{
							"Exception spawning loaded thing ",
							thing.ToStringSafe<Thing>(),
							": ",
							ex
						}), false);
					}
				}
			}
			foreach (Building building in from t in list2.OfType<Building>()
			orderby t.def.size.Magnitude
			select t)
			{
				try
				{
					GenSpawn.SpawnBuildingAsPossible(building, this, true);
				}
				catch (Exception ex2)
				{
					Log.Error(string.Concat(new object[]
					{
						"Exception spawning loaded thing ",
						building.ToStringSafe<Building>(),
						": ",
						ex2
					}), false);
				}
			}
			BackCompatibility.PostCheckSpawnBackCompatibleThingAfterLoading(this);
			DeepProfiler.End();
			this.FinalizeInit();
		}

		// Token: 0x06000B21 RID: 2849 RVA: 0x0003B798 File Offset: 0x00039998
		public void FinalizeInit()
		{
			this.pathGrid.RecalculateAllPerceivedPathCosts();
			this.regionAndRoomUpdater.Enabled = true;
			this.regionAndRoomUpdater.RebuildAllRegionsAndRooms();
			this.powerNetManager.UpdatePowerNetsAndConnections_First();
			this.temperatureCache.temperatureSaveLoad.ApplyLoadedDataToRegions();
			this.avoidGrid.Regenerate();
			foreach (Thing thing in this.listerThings.AllThings.ToList<Thing>())
			{
				try
				{
					thing.PostMapInit();
				}
				catch (Exception ex)
				{
					Log.Error(string.Concat(new object[]
					{
						"Error in PostMapInit() for ",
						thing.ToStringSafe<Thing>(),
						": ",
						ex
					}), false);
				}
			}
			this.listerFilthInHomeArea.RebuildAll();
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				this.mapDrawer.RegenerateEverythingNow();
			});
			this.resourceCounter.UpdateResourceCounts();
			this.wealthWatcher.ForceRecount(true);
			MapComponentUtility.FinalizeInit(this);
		}

		// Token: 0x06000B22 RID: 2850 RVA: 0x0003B8B4 File Offset: 0x00039AB4
		private void ExposeComponents()
		{
			Scribe_Deep.Look<WeatherManager>(ref this.weatherManager, "weatherManager", new object[]
			{
				this
			});
			Scribe_Deep.Look<ReservationManager>(ref this.reservationManager, "reservationManager", new object[]
			{
				this
			});
			Scribe_Deep.Look<PhysicalInteractionReservationManager>(ref this.physicalInteractionReservationManager, "physicalInteractionReservationManager", Array.Empty<object>());
			Scribe_Deep.Look<DesignationManager>(ref this.designationManager, "designationManager", new object[]
			{
				this
			});
			Scribe_Deep.Look<PawnDestinationReservationManager>(ref this.pawnDestinationReservationManager, "pawnDestinationReservationManager", Array.Empty<object>());
			Scribe_Deep.Look<LordManager>(ref this.lordManager, "lordManager", new object[]
			{
				this
			});
			Scribe_Deep.Look<PassingShipManager>(ref this.passingShipManager, "visitorManager", new object[]
			{
				this
			});
			Scribe_Deep.Look<GameConditionManager>(ref this.gameConditionManager, "gameConditionManager", new object[]
			{
				this
			});
			Scribe_Deep.Look<FogGrid>(ref this.fogGrid, "fogGrid", new object[]
			{
				this
			});
			Scribe_Deep.Look<RoofGrid>(ref this.roofGrid, "roofGrid", new object[]
			{
				this
			});
			Scribe_Deep.Look<TerrainGrid>(ref this.terrainGrid, "terrainGrid", new object[]
			{
				this
			});
			Scribe_Deep.Look<ZoneManager>(ref this.zoneManager, "zoneManager", new object[]
			{
				this
			});
			Scribe_Deep.Look<TemperatureCache>(ref this.temperatureCache, "temperatureCache", new object[]
			{
				this
			});
			Scribe_Deep.Look<SnowGrid>(ref this.snowGrid, "snowGrid", new object[]
			{
				this
			});
			Scribe_Deep.Look<AreaManager>(ref this.areaManager, "areaManager", new object[]
			{
				this
			});
			Scribe_Deep.Look<VoluntarilyJoinableLordsStarter>(ref this.lordsStarter, "lordsStarter", new object[]
			{
				this
			});
			Scribe_Deep.Look<AttackTargetReservationManager>(ref this.attackTargetReservationManager, "attackTargetReservationManager", new object[]
			{
				this
			});
			Scribe_Deep.Look<DeepResourceGrid>(ref this.deepResourceGrid, "deepResourceGrid", new object[]
			{
				this
			});
			Scribe_Deep.Look<WeatherDecider>(ref this.weatherDecider, "weatherDecider", new object[]
			{
				this
			});
			Scribe_Deep.Look<DamageWatcher>(ref this.damageWatcher, "damageWatcher", Array.Empty<object>());
			Scribe_Deep.Look<RememberedCameraPos>(ref this.rememberedCameraPos, "rememberedCameraPos", new object[]
			{
				this
			});
			Scribe_Deep.Look<MineStrikeManager>(ref this.mineStrikeManager, "mineStrikeManager", Array.Empty<object>());
			Scribe_Deep.Look<RetainedCaravanData>(ref this.retainedCaravanData, "retainedCaravanData", new object[]
			{
				this
			});
			Scribe_Deep.Look<StoryState>(ref this.storyState, "storyState", new object[]
			{
				this
			});
			Scribe_Deep.Look<WildPlantSpawner>(ref this.wildPlantSpawner, "wildPlantSpawner", new object[]
			{
				this
			});
			Scribe_Collections.Look<MapComponent>(ref this.components, "components", LookMode.Deep, new object[]
			{
				this
			});
			this.FillComponents();
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x06000B23 RID: 2851 RVA: 0x0003BB60 File Offset: 0x00039D60
		public void MapPreTick()
		{
			this.itemAvailability.Tick();
			this.listerHaulables.ListerHaulablesTick();
			try
			{
				this.autoBuildRoofAreaSetter.AutoBuildRoofAreaSetterTick_First();
			}
			catch (Exception ex)
			{
				Log.Error(ex.ToString(), false);
			}
			this.roofCollapseBufferResolver.CollapseRoofsMarkedToCollapse();
			this.windManager.WindManagerTick();
			try
			{
				this.mapTemperature.MapTemperatureTick();
			}
			catch (Exception ex2)
			{
				Log.Error(ex2.ToString(), false);
			}
		}

		// Token: 0x06000B24 RID: 2852 RVA: 0x0003BBEC File Offset: 0x00039DEC
		public void MapPostTick()
		{
			try
			{
				this.wildAnimalSpawner.WildAnimalSpawnerTick();
			}
			catch (Exception ex)
			{
				Log.Error(ex.ToString(), false);
			}
			try
			{
				this.wildPlantSpawner.WildPlantSpawnerTick();
			}
			catch (Exception ex2)
			{
				Log.Error(ex2.ToString(), false);
			}
			try
			{
				this.powerNetManager.PowerNetsTick();
			}
			catch (Exception ex3)
			{
				Log.Error(ex3.ToString(), false);
			}
			try
			{
				this.steadyEnvironmentEffects.SteadyEnvironmentEffectsTick();
			}
			catch (Exception ex4)
			{
				Log.Error(ex4.ToString(), false);
			}
			try
			{
				this.lordManager.LordManagerTick();
			}
			catch (Exception ex5)
			{
				Log.Error(ex5.ToString(), false);
			}
			try
			{
				this.passingShipManager.PassingShipManagerTick();
			}
			catch (Exception ex6)
			{
				Log.Error(ex6.ToString(), false);
			}
			try
			{
				this.debugDrawer.DebugDrawerTick();
			}
			catch (Exception ex7)
			{
				Log.Error(ex7.ToString(), false);
			}
			try
			{
				this.lordsStarter.VoluntarilyJoinableLordsStarterTick();
			}
			catch (Exception ex8)
			{
				Log.Error(ex8.ToString(), false);
			}
			try
			{
				this.gameConditionManager.GameConditionManagerTick();
			}
			catch (Exception ex9)
			{
				Log.Error(ex9.ToString(), false);
			}
			try
			{
				this.weatherManager.WeatherManagerTick();
			}
			catch (Exception ex10)
			{
				Log.Error(ex10.ToString(), false);
			}
			try
			{
				this.resourceCounter.ResourceCounterTick();
			}
			catch (Exception ex11)
			{
				Log.Error(ex11.ToString(), false);
			}
			try
			{
				this.weatherDecider.WeatherDeciderTick();
			}
			catch (Exception ex12)
			{
				Log.Error(ex12.ToString(), false);
			}
			try
			{
				this.fireWatcher.FireWatcherTick();
			}
			catch (Exception ex13)
			{
				Log.Error(ex13.ToString(), false);
			}
			MapComponentUtility.MapComponentTick(this);
		}

		// Token: 0x06000B25 RID: 2853 RVA: 0x0003BE00 File Offset: 0x0003A000
		public void MapUpdate()
		{
			bool worldRenderedNow = WorldRendererUtility.WorldRenderedNow;
			this.skyManager.SkyManagerUpdate();
			this.powerNetManager.UpdatePowerNetsAndConnections_First();
			this.regionGrid.UpdateClean();
			this.regionAndRoomUpdater.TryRebuildDirtyRegionsAndRooms();
			this.glowGrid.GlowGridUpdate_First();
			this.lordManager.LordManagerUpdate();
			if (!worldRenderedNow && Find.CurrentMap == this)
			{
				if (Map.AlwaysRedrawShadows)
				{
					this.mapDrawer.WholeMapChanged(MapMeshFlag.Things);
				}
				PlantFallColors.SetFallShaderGlobals(this);
				this.waterInfo.SetTextures();
				this.avoidGrid.DebugDrawOnMap();
				this.mapDrawer.MapMeshDrawerUpdate_First();
				this.powerNetGrid.DrawDebugPowerNetGrid();
				DoorsDebugDrawer.DrawDebug();
				this.mapDrawer.DrawMapMesh();
				this.dynamicDrawManager.DrawDynamicThings();
				this.gameConditionManager.GameConditionManagerDraw(this);
				MapEdgeClipDrawer.DrawClippers(this);
				this.designationManager.DrawDesignations();
				this.overlayDrawer.DrawAllOverlays();
			}
			try
			{
				this.areaManager.AreaManagerUpdate();
			}
			catch (Exception ex)
			{
				Log.Error(ex.ToString(), false);
			}
			this.weatherManager.WeatherManagerUpdate();
			MapComponentUtility.MapComponentUpdate(this);
		}

		// Token: 0x06000B26 RID: 2854 RVA: 0x0003BF28 File Offset: 0x0003A128
		public T GetComponent<T>() where T : MapComponent
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

		// Token: 0x06000B27 RID: 2855 RVA: 0x0003BF78 File Offset: 0x0003A178
		public MapComponent GetComponent(Type type)
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

		// Token: 0x17000236 RID: 566
		// (get) Token: 0x06000B28 RID: 2856 RVA: 0x0003BFC2 File Offset: 0x0003A1C2
		public int ConstantRandSeed
		{
			get
			{
				return this.uniqueID ^ 16622162;
			}
		}

		// Token: 0x06000B29 RID: 2857 RVA: 0x0003BFD0 File Offset: 0x0003A1D0
		public string GetUniqueLoadID()
		{
			return "Map_" + this.uniqueID;
		}

		// Token: 0x06000B2A RID: 2858 RVA: 0x0003BFE8 File Offset: 0x0003A1E8
		public override string ToString()
		{
			string text = "Map-" + this.uniqueID;
			if (this.IsPlayerHome)
			{
				text += "-PlayerHome";
			}
			return text;
		}

		// Token: 0x06000B2B RID: 2859 RVA: 0x0003C020 File Offset: 0x0003A220
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.spawnedThings;
		}

		// Token: 0x06000B2C RID: 2860 RVA: 0x0003C028 File Offset: 0x0003A228
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.listerThings.ThingsInGroup(ThingRequestGroup.ThingHolder));
			List<PassingShip> passingShips = this.passingShipManager.passingShips;
			for (int i = 0; i < passingShips.Count; i++)
			{
				IThingHolder thingHolder = passingShips[i] as IThingHolder;
				if (thingHolder != null)
				{
					outChildren.Add(thingHolder);
				}
			}
			for (int j = 0; j < this.components.Count; j++)
			{
				IThingHolder thingHolder2 = this.components[j] as IThingHolder;
				if (thingHolder2 != null)
				{
					outChildren.Add(thingHolder2);
				}
			}
		}

		// Token: 0x040008A7 RID: 2215
		public MapFileCompressor compressor;

		// Token: 0x040008A8 RID: 2216
		private List<Thing> loadedFullThings;

		// Token: 0x040008A9 RID: 2217
		public int uniqueID = -1;

		// Token: 0x040008AA RID: 2218
		public int generationTick;

		// Token: 0x040008AB RID: 2219
		public MapInfo info = new MapInfo();

		// Token: 0x040008AC RID: 2220
		public List<MapComponent> components = new List<MapComponent>();

		// Token: 0x040008AD RID: 2221
		public ThingOwner spawnedThings;

		// Token: 0x040008AE RID: 2222
		public CellIndices cellIndices;

		// Token: 0x040008AF RID: 2223
		public ListerThings listerThings;

		// Token: 0x040008B0 RID: 2224
		public ListerBuildings listerBuildings;

		// Token: 0x040008B1 RID: 2225
		public MapPawns mapPawns;

		// Token: 0x040008B2 RID: 2226
		public DynamicDrawManager dynamicDrawManager;

		// Token: 0x040008B3 RID: 2227
		public MapDrawer mapDrawer;

		// Token: 0x040008B4 RID: 2228
		public PawnDestinationReservationManager pawnDestinationReservationManager;

		// Token: 0x040008B5 RID: 2229
		public TooltipGiverList tooltipGiverList;

		// Token: 0x040008B6 RID: 2230
		public ReservationManager reservationManager;

		// Token: 0x040008B7 RID: 2231
		public PhysicalInteractionReservationManager physicalInteractionReservationManager;

		// Token: 0x040008B8 RID: 2232
		public DesignationManager designationManager;

		// Token: 0x040008B9 RID: 2233
		public LordManager lordManager;

		// Token: 0x040008BA RID: 2234
		public PassingShipManager passingShipManager;

		// Token: 0x040008BB RID: 2235
		public HaulDestinationManager haulDestinationManager;

		// Token: 0x040008BC RID: 2236
		public DebugCellDrawer debugDrawer;

		// Token: 0x040008BD RID: 2237
		public GameConditionManager gameConditionManager;

		// Token: 0x040008BE RID: 2238
		public WeatherManager weatherManager;

		// Token: 0x040008BF RID: 2239
		public ZoneManager zoneManager;

		// Token: 0x040008C0 RID: 2240
		public ResourceCounter resourceCounter;

		// Token: 0x040008C1 RID: 2241
		public MapTemperature mapTemperature;

		// Token: 0x040008C2 RID: 2242
		public TemperatureCache temperatureCache;

		// Token: 0x040008C3 RID: 2243
		public AreaManager areaManager;

		// Token: 0x040008C4 RID: 2244
		public AttackTargetsCache attackTargetsCache;

		// Token: 0x040008C5 RID: 2245
		public AttackTargetReservationManager attackTargetReservationManager;

		// Token: 0x040008C6 RID: 2246
		public VoluntarilyJoinableLordsStarter lordsStarter;

		// Token: 0x040008C7 RID: 2247
		public ThingGrid thingGrid;

		// Token: 0x040008C8 RID: 2248
		public CoverGrid coverGrid;

		// Token: 0x040008C9 RID: 2249
		public EdificeGrid edificeGrid;

		// Token: 0x040008CA RID: 2250
		public BlueprintGrid blueprintGrid;

		// Token: 0x040008CB RID: 2251
		public FogGrid fogGrid;

		// Token: 0x040008CC RID: 2252
		public RegionGrid regionGrid;

		// Token: 0x040008CD RID: 2253
		public GlowGrid glowGrid;

		// Token: 0x040008CE RID: 2254
		public TerrainGrid terrainGrid;

		// Token: 0x040008CF RID: 2255
		public PathGrid pathGrid;

		// Token: 0x040008D0 RID: 2256
		public RoofGrid roofGrid;

		// Token: 0x040008D1 RID: 2257
		public FertilityGrid fertilityGrid;

		// Token: 0x040008D2 RID: 2258
		public SnowGrid snowGrid;

		// Token: 0x040008D3 RID: 2259
		public DeepResourceGrid deepResourceGrid;

		// Token: 0x040008D4 RID: 2260
		public ExitMapGrid exitMapGrid;

		// Token: 0x040008D5 RID: 2261
		public AvoidGrid avoidGrid;

		// Token: 0x040008D6 RID: 2262
		public LinkGrid linkGrid;

		// Token: 0x040008D7 RID: 2263
		public GlowFlooder glowFlooder;

		// Token: 0x040008D8 RID: 2264
		public PowerNetManager powerNetManager;

		// Token: 0x040008D9 RID: 2265
		public PowerNetGrid powerNetGrid;

		// Token: 0x040008DA RID: 2266
		public RegionMaker regionMaker;

		// Token: 0x040008DB RID: 2267
		public PathFinder pathFinder;

		// Token: 0x040008DC RID: 2268
		public PawnPathPool pawnPathPool;

		// Token: 0x040008DD RID: 2269
		public RegionAndRoomUpdater regionAndRoomUpdater;

		// Token: 0x040008DE RID: 2270
		public RegionLinkDatabase regionLinkDatabase;

		// Token: 0x040008DF RID: 2271
		public MoteCounter moteCounter;

		// Token: 0x040008E0 RID: 2272
		public GatherSpotLister gatherSpotLister;

		// Token: 0x040008E1 RID: 2273
		public WindManager windManager;

		// Token: 0x040008E2 RID: 2274
		public ListerBuildingsRepairable listerBuildingsRepairable;

		// Token: 0x040008E3 RID: 2275
		public ListerHaulables listerHaulables;

		// Token: 0x040008E4 RID: 2276
		public ListerMergeables listerMergeables;

		// Token: 0x040008E5 RID: 2277
		public ListerArtificialBuildingsForMeditation listerArtificialBuildingsForMeditation;

		// Token: 0x040008E6 RID: 2278
		public ListerFilthInHomeArea listerFilthInHomeArea;

		// Token: 0x040008E7 RID: 2279
		public Reachability reachability;

		// Token: 0x040008E8 RID: 2280
		public ItemAvailability itemAvailability;

		// Token: 0x040008E9 RID: 2281
		public AutoBuildRoofAreaSetter autoBuildRoofAreaSetter;

		// Token: 0x040008EA RID: 2282
		public RoofCollapseBufferResolver roofCollapseBufferResolver;

		// Token: 0x040008EB RID: 2283
		public RoofCollapseBuffer roofCollapseBuffer;

		// Token: 0x040008EC RID: 2284
		public WildAnimalSpawner wildAnimalSpawner;

		// Token: 0x040008ED RID: 2285
		public WildPlantSpawner wildPlantSpawner;

		// Token: 0x040008EE RID: 2286
		public SteadyEnvironmentEffects steadyEnvironmentEffects;

		// Token: 0x040008EF RID: 2287
		public SkyManager skyManager;

		// Token: 0x040008F0 RID: 2288
		public OverlayDrawer overlayDrawer;

		// Token: 0x040008F1 RID: 2289
		public FloodFiller floodFiller;

		// Token: 0x040008F2 RID: 2290
		public WeatherDecider weatherDecider;

		// Token: 0x040008F3 RID: 2291
		public FireWatcher fireWatcher;

		// Token: 0x040008F4 RID: 2292
		public DangerWatcher dangerWatcher;

		// Token: 0x040008F5 RID: 2293
		public DamageWatcher damageWatcher;

		// Token: 0x040008F6 RID: 2294
		public StrengthWatcher strengthWatcher;

		// Token: 0x040008F7 RID: 2295
		public WealthWatcher wealthWatcher;

		// Token: 0x040008F8 RID: 2296
		public RegionDirtyer regionDirtyer;

		// Token: 0x040008F9 RID: 2297
		public MapCellsInRandomOrder cellsInRandomOrder;

		// Token: 0x040008FA RID: 2298
		public RememberedCameraPos rememberedCameraPos;

		// Token: 0x040008FB RID: 2299
		public MineStrikeManager mineStrikeManager;

		// Token: 0x040008FC RID: 2300
		public StoryState storyState;

		// Token: 0x040008FD RID: 2301
		public RoadInfo roadInfo;

		// Token: 0x040008FE RID: 2302
		public WaterInfo waterInfo;

		// Token: 0x040008FF RID: 2303
		public RetainedCaravanData retainedCaravanData;

		// Token: 0x04000900 RID: 2304
		public const string ThingSaveKey = "thing";

		// Token: 0x04000901 RID: 2305
		[TweakValue("Graphics_Shadow", 0f, 100f)]
		private static bool AlwaysRedrawShadows;
	}
}
