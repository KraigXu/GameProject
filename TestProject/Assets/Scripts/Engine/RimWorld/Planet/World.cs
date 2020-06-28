using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.Noise;

namespace RimWorld.Planet
{
	public sealed class World : IThingHolder, IExposable, IIncidentTarget, ILoadReferenceable
	{
		public float PlanetCoverage
		{
			get
			{
				return this.info.planetCoverage;
			}
		}

		public IThingHolder ParentHolder
		{
			get
			{
				return null;
			}
		}

		public int Tile
		{
			get
			{
				return -1;
			}
		}

		public StoryState StoryState
		{
			get
			{
				return this.storyState;
			}
		}

		public GameConditionManager GameConditionManager
		{
			get
			{
				return this.gameConditionManager;
			}
		}

		// Token: 0x170011E2 RID: 4578
		// (get) Token: 0x06006AAE RID: 27310 RVA: 0x00253234 File Offset: 0x00251434
		public float PlayerWealthForStoryteller
		{
			get
			{
				float num = 0f;
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					num += maps[i].PlayerWealthForStoryteller;
				}
				List<Caravan> caravans = Find.WorldObjects.Caravans;
				for (int j = 0; j < caravans.Count; j++)
				{
					num += caravans[j].PlayerWealthForStoryteller;
				}
				return num;
			}
		}

		// Token: 0x170011E3 RID: 4579
		// (get) Token: 0x06006AAF RID: 27311 RVA: 0x0025329E File Offset: 0x0025149E
		public IEnumerable<Pawn> PlayerPawnsForStoryteller
		{
			get
			{
				return PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_OfPlayerFaction;
			}
		}

		// Token: 0x170011E4 RID: 4580
		// (get) Token: 0x06006AB0 RID: 27312 RVA: 0x0003AE82 File Offset: 0x00039082
		public FloatRange IncidentPointsRandomFactorRange
		{
			get
			{
				return FloatRange.One;
			}
		}

		// Token: 0x06006AB1 RID: 27313 RVA: 0x002532A5 File Offset: 0x002514A5
		public IEnumerable<IncidentTargetTagDef> IncidentTargetTags()
		{
			yield return IncidentTargetTagDefOf.World;
			yield break;
		}

		// Token: 0x06006AB2 RID: 27314 RVA: 0x002532B0 File Offset: 0x002514B0
		public void ExposeData()
		{
			Scribe_Deep.Look<WorldInfo>(ref this.info, "info", Array.Empty<object>());
			Scribe_Deep.Look<WorldGrid>(ref this.grid, "grid", Array.Empty<object>());
			if (Scribe.mode != LoadSaveMode.LoadingVars)
			{
				this.ExposeComponents();
				return;
			}
			if (this.grid == null || !this.grid.HasWorldData)
			{
				WorldGenerator.GenerateWithoutWorldData(this.info.seedString);
				return;
			}
			WorldGenerator.GenerateFromScribe(this.info.seedString);
		}

		// Token: 0x06006AB3 RID: 27315 RVA: 0x0025332C File Offset: 0x0025152C
		public void ExposeComponents()
		{
			Scribe_Deep.Look<FactionManager>(ref this.factionManager, "factionManager", Array.Empty<object>());
			Scribe_Deep.Look<WorldPawns>(ref this.worldPawns, "worldPawns", Array.Empty<object>());
			Scribe_Deep.Look<WorldObjectsHolder>(ref this.worldObjects, "worldObjects", Array.Empty<object>());
			Scribe_Deep.Look<GameConditionManager>(ref this.gameConditionManager, "gameConditionManager", new object[]
			{
				this
			});
			Scribe_Deep.Look<StoryState>(ref this.storyState, "storyState", new object[]
			{
				this
			});
			Scribe_Deep.Look<WorldFeatures>(ref this.features, "features", Array.Empty<object>());
			Scribe_Collections.Look<WorldComponent>(ref this.components, "components", LookMode.Deep, new object[]
			{
				this
			});
			this.FillComponents();
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x06006AB4 RID: 27316 RVA: 0x002533E8 File Offset: 0x002515E8
		public void ConstructComponents()
		{
			this.worldObjects = new WorldObjectsHolder();
			this.factionManager = new FactionManager();
			this.worldPawns = new WorldPawns();
			this.gameConditionManager = new GameConditionManager(this);
			this.storyState = new StoryState(this);
			this.renderer = new WorldRenderer();
			this.UI = new WorldInterface();
			this.debugDrawer = new WorldDebugDrawer();
			this.dynamicDrawManager = new WorldDynamicDrawManager();
			this.pathFinder = new WorldPathFinder();
			this.pathPool = new WorldPathPool();
			this.reachability = new WorldReachability();
			this.floodFiller = new WorldFloodFiller();
			this.ticksAbsCache = new ConfiguredTicksAbsAtGameStartCache();
			this.components.Clear();
			this.FillComponents();
		}

		// Token: 0x06006AB5 RID: 27317 RVA: 0x002534A4 File Offset: 0x002516A4
		private void FillComponents()
		{
			this.components.RemoveAll((WorldComponent component) => component == null);
			foreach (Type type in typeof(WorldComponent).AllSubclassesNonAbstract())
			{
				if (this.GetComponent(type) == null)
				{
					try
					{
						WorldComponent item = (WorldComponent)Activator.CreateInstance(type, new object[]
						{
							this
						});
						this.components.Add(item);
					}
					catch (Exception ex)
					{
						Log.Error(string.Concat(new object[]
						{
							"Could not instantiate a WorldComponent of type ",
							type,
							": ",
							ex
						}), false);
					}
				}
			}
			this.tileTemperatures = this.GetComponent<TileTemperaturesComp>();
			this.genData = this.GetComponent<WorldGenData>();
		}

		// Token: 0x06006AB6 RID: 27318 RVA: 0x0025359C File Offset: 0x0025179C
		public void FinalizeInit()
		{
			this.pathGrid.RecalculateAllPerceivedPathCosts();
			AmbientSoundManager.EnsureWorldAmbientSoundCreated();
			WorldComponentUtility.FinalizeInit(this);
		}

		// Token: 0x06006AB7 RID: 27319 RVA: 0x002535B4 File Offset: 0x002517B4
		public void WorldTick()
		{
			this.worldPawns.WorldPawnsTick();
			this.factionManager.FactionManagerTick();
			this.worldObjects.WorldObjectsHolderTick();
			this.debugDrawer.WorldDebugDrawerTick();
			this.pathGrid.WorldPathGridTick();
			WorldComponentUtility.WorldComponentTick(this);
		}

		// Token: 0x06006AB8 RID: 27320 RVA: 0x002535F4 File Offset: 0x002517F4
		public void WorldPostTick()
		{
			try
			{
				this.gameConditionManager.GameConditionManagerTick();
			}
			catch (Exception ex)
			{
				Log.Error(ex.ToString(), false);
			}
		}

		// Token: 0x06006AB9 RID: 27321 RVA: 0x0025362C File Offset: 0x0025182C
		public void WorldUpdate()
		{
			bool worldRenderedNow = WorldRendererUtility.WorldRenderedNow;
			this.renderer.CheckActivateWorldCamera();
			if (worldRenderedNow)
			{
				ExpandableWorldObjectsUtility.ExpandableWorldObjectsUpdate();
				this.renderer.DrawWorldLayers();
				this.dynamicDrawManager.DrawDynamicWorldObjects();
				this.features.UpdateFeatures();
				NoiseDebugUI.RenderPlanetNoise();
			}
			WorldComponentUtility.WorldComponentUpdate(this);
		}

		// Token: 0x06006ABA RID: 27322 RVA: 0x0025367C File Offset: 0x0025187C
		public T GetComponent<T>() where T : WorldComponent
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

		// Token: 0x06006ABB RID: 27323 RVA: 0x002536CC File Offset: 0x002518CC
		public WorldComponent GetComponent(Type type)
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

		// Token: 0x06006ABC RID: 27324 RVA: 0x00253718 File Offset: 0x00251918
		public Rot4 CoastDirectionAt(int tileID)
		{
			if (!this.grid[tileID].biome.canBuildBase)
			{
				return Rot4.Invalid;
			}
			World.tmpOceanDirs.Clear();
			this.grid.GetTileNeighbors(tileID, World.tmpNeighbors);
			int i = 0;
			int count = World.tmpNeighbors.Count;
			while (i < count)
			{
				if (this.grid[World.tmpNeighbors[i]].biome == BiomeDefOf.Ocean)
				{
					Rot4 rotFromTo = this.grid.GetRotFromTo(tileID, World.tmpNeighbors[i]);
					if (!World.tmpOceanDirs.Contains(rotFromTo))
					{
						World.tmpOceanDirs.Add(rotFromTo);
					}
				}
				i++;
			}
			if (World.tmpOceanDirs.Count == 0)
			{
				return Rot4.Invalid;
			}
			Rand.PushState();
			Rand.Seed = tileID;
			int index = Rand.Range(0, World.tmpOceanDirs.Count);
			Rand.PopState();
			return World.tmpOceanDirs[index];
		}

		// Token: 0x06006ABD RID: 27325 RVA: 0x00253808 File Offset: 0x00251A08
		public bool HasCaves(int tile)
		{
			Tile tile2 = this.grid[tile];
			float chance;
			if (tile2.hilliness >= Hilliness.Mountainous)
			{
				chance = 0.5f;
			}
			else
			{
				if (tile2.hilliness < Hilliness.LargeHills)
				{
					return false;
				}
				chance = 0.25f;
			}
			return Rand.ChanceSeeded(chance, Gen.HashCombineInt(Find.World.info.Seed, tile));
		}

		// Token: 0x06006ABE RID: 27326 RVA: 0x00253864 File Offset: 0x00251A64
		public IEnumerable<ThingDef> NaturalRockTypesIn(int tile)
		{
			Rand.PushState();
			Rand.Seed = tile;
			if (this.allNaturalRockDefs == null)
			{
				this.allNaturalRockDefs = (from d in DefDatabase<ThingDef>.AllDefs
				where d.IsNonResourceNaturalRock
				select d).ToList<ThingDef>();
			}
			int num = Rand.RangeInclusive(2, 3);
			if (num > this.allNaturalRockDefs.Count)
			{
				num = this.allNaturalRockDefs.Count;
			}
			World.tmpNaturalRockDefs.Clear();
			World.tmpNaturalRockDefs.AddRange(this.allNaturalRockDefs);
			List<ThingDef> list = new List<ThingDef>();
			for (int i = 0; i < num; i++)
			{
				ThingDef item = World.tmpNaturalRockDefs.RandomElement<ThingDef>();
				World.tmpNaturalRockDefs.Remove(item);
				list.Add(item);
			}
			Rand.PopState();
			return list;
		}

		// Token: 0x06006ABF RID: 27327 RVA: 0x0025392A File Offset: 0x00251B2A
		public bool Impassable(int tileID)
		{
			return !this.pathGrid.Passable(tileID);
		}

		// Token: 0x06006AC0 RID: 27328 RVA: 0x00019EA1 File Offset: 0x000180A1
		public ThingOwner GetDirectlyHeldThings()
		{
			return null;
		}

		// Token: 0x06006AC1 RID: 27329 RVA: 0x0025393C File Offset: 0x00251B3C
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
			List<WorldObject> allWorldObjects = this.worldObjects.AllWorldObjects;
			for (int i = 0; i < allWorldObjects.Count; i++)
			{
				IThingHolder thingHolder = allWorldObjects[i] as IThingHolder;
				if (thingHolder != null)
				{
					outChildren.Add(thingHolder);
				}
				List<WorldObjectComp> allComps = allWorldObjects[i].AllComps;
				for (int j = 0; j < allComps.Count; j++)
				{
					IThingHolder thingHolder2 = allComps[j] as IThingHolder;
					if (thingHolder2 != null)
					{
						outChildren.Add(thingHolder2);
					}
				}
			}
			for (int k = 0; k < this.components.Count; k++)
			{
				IThingHolder thingHolder3 = this.components[k] as IThingHolder;
				if (thingHolder3 != null)
				{
					outChildren.Add(thingHolder3);
				}
			}
		}

		// Token: 0x06006AC2 RID: 27330 RVA: 0x00253A00 File Offset: 0x00251C00
		public string GetUniqueLoadID()
		{
			return "World";
		}

		// Token: 0x170011E5 RID: 4581
		// (get) Token: 0x06006AC3 RID: 27331 RVA: 0x00253A07 File Offset: 0x00251C07
		public int ConstantRandSeed
		{
			get
			{
				return this.info.persistentRandomValue;
			}
		}

		// Token: 0x06006AC4 RID: 27332 RVA: 0x00253A14 File Offset: 0x00251C14
		public override string ToString()
		{
			return "World-" + this.info.name;
		}

		// Token: 0x0400428A RID: 17034
		public WorldInfo info = new WorldInfo();

		// Token: 0x0400428B RID: 17035
		public List<WorldComponent> components = new List<WorldComponent>();

		// Token: 0x0400428C RID: 17036
		public FactionManager factionManager;

		// Token: 0x0400428D RID: 17037
		public WorldPawns worldPawns;

		// Token: 0x0400428E RID: 17038
		public WorldObjectsHolder worldObjects;

		// Token: 0x0400428F RID: 17039
		public GameConditionManager gameConditionManager;

		// Token: 0x04004290 RID: 17040
		public StoryState storyState;

		// Token: 0x04004291 RID: 17041
		public WorldFeatures features;

		// Token: 0x04004292 RID: 17042
		public WorldGrid grid;

		// Token: 0x04004293 RID: 17043
		public WorldPathGrid pathGrid;

		// Token: 0x04004294 RID: 17044
		public WorldRenderer renderer;

		// Token: 0x04004295 RID: 17045
		public WorldInterface UI;

		// Token: 0x04004296 RID: 17046
		public WorldDebugDrawer debugDrawer;

		// Token: 0x04004297 RID: 17047
		public WorldDynamicDrawManager dynamicDrawManager;

		// Token: 0x04004298 RID: 17048
		public WorldPathFinder pathFinder;

		// Token: 0x04004299 RID: 17049
		public WorldPathPool pathPool;

		// Token: 0x0400429A RID: 17050
		public WorldReachability reachability;

		// Token: 0x0400429B RID: 17051
		public WorldFloodFiller floodFiller;

		// Token: 0x0400429C RID: 17052
		public ConfiguredTicksAbsAtGameStartCache ticksAbsCache;

		// Token: 0x0400429D RID: 17053
		public TileTemperaturesComp tileTemperatures;

		// Token: 0x0400429E RID: 17054
		public WorldGenData genData;

		// Token: 0x0400429F RID: 17055
		private List<ThingDef> allNaturalRockDefs;

		// Token: 0x040042A0 RID: 17056
		private static List<ThingDef> tmpNaturalRockDefs = new List<ThingDef>();

		// Token: 0x040042A1 RID: 17057
		private static List<int> tmpNeighbors = new List<int>();

		// Token: 0x040042A2 RID: 17058
		private static List<Rot4> tmpOceanDirs = new List<Rot4>();
	}
}
