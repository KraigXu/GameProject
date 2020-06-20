using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x020001AA RID: 426
	public static class MapGenerator
	{
		// Token: 0x1700024F RID: 591
		// (get) Token: 0x06000BE8 RID: 3048 RVA: 0x000439E8 File Offset: 0x00041BE8
		public static MapGenFloatGrid Elevation
		{
			get
			{
				return MapGenerator.FloatGridNamed("Elevation");
			}
		}

		// Token: 0x17000250 RID: 592
		// (get) Token: 0x06000BE9 RID: 3049 RVA: 0x000439F4 File Offset: 0x00041BF4
		public static MapGenFloatGrid Fertility
		{
			get
			{
				return MapGenerator.FloatGridNamed("Fertility");
			}
		}

		// Token: 0x17000251 RID: 593
		// (get) Token: 0x06000BEA RID: 3050 RVA: 0x00043A00 File Offset: 0x00041C00
		public static MapGenFloatGrid Caves
		{
			get
			{
				return MapGenerator.FloatGridNamed("Caves");
			}
		}

		// Token: 0x17000252 RID: 594
		// (get) Token: 0x06000BEB RID: 3051 RVA: 0x00043A0C File Offset: 0x00041C0C
		// (set) Token: 0x06000BEC RID: 3052 RVA: 0x00043A30 File Offset: 0x00041C30
		public static IntVec3 PlayerStartSpot
		{
			get
			{
				if (!MapGenerator.playerStartSpotInt.IsValid)
				{
					Log.Error("Accessing player start spot before setting it.", false);
					return IntVec3.Zero;
				}
				return MapGenerator.playerStartSpotInt;
			}
			set
			{
				MapGenerator.playerStartSpotInt = value;
			}
		}

		// Token: 0x06000BED RID: 3053 RVA: 0x00043A38 File Offset: 0x00041C38
		public static Map GenerateMap(IntVec3 mapSize, MapParent parent, MapGeneratorDef mapGenerator, IEnumerable<GenStepWithParams> extraGenStepDefs = null, Action<Map> extraInitBeforeContentGen = null)
		{
			ProgramState programState = Current.ProgramState;
			Current.ProgramState = ProgramState.MapInitializing;
			MapGenerator.playerStartSpotInt = IntVec3.Invalid;
			MapGenerator.rootsToUnfog.Clear();
			MapGenerator.data.Clear();
			MapGenerator.mapBeingGenerated = null;
			DeepProfiler.Start("InitNewGeneratedMap");
			Rand.PushState();
			int seed = Gen.HashCombineInt(Find.World.info.Seed, parent.Tile);
			Rand.Seed = seed;
			Map result;
			try
			{
				if (parent != null && parent.HasMap)
				{
					Log.Error("Tried to generate a new map and set " + parent + " as its parent, but this world object already has a map. One world object can't have more than 1 map.", false);
					parent = null;
				}
				DeepProfiler.Start("Set up map");
				Map map = new Map();
				map.uniqueID = Find.UniqueIDsManager.GetNextMapID();
				map.generationTick = GenTicks.TicksGame;
				MapGenerator.mapBeingGenerated = map;
				map.info.Size = mapSize;
				map.info.parent = parent;
				map.ConstructComponents();
				DeepProfiler.End();
				Current.Game.AddMap(map);
				if (extraInitBeforeContentGen != null)
				{
					extraInitBeforeContentGen(map);
				}
				if (mapGenerator == null)
				{
					Log.Error("Attempted to generate map without generator; falling back on encounter map", false);
					mapGenerator = MapGeneratorDefOf.Encounter;
				}
				IEnumerable<GenStepWithParams> enumerable = from x in mapGenerator.genSteps
				select new GenStepWithParams(x, default(GenStepParams));
				if (extraGenStepDefs != null)
				{
					enumerable = enumerable.Concat(extraGenStepDefs);
				}
				map.areaManager.AddStartingAreas();
				map.weatherDecider.StartInitialWeather();
				DeepProfiler.Start("Generate contents into map");
				MapGenerator.GenerateContentsIntoMap(enumerable, map, seed);
				DeepProfiler.End();
				Find.Scenario.PostMapGenerate(map);
				DeepProfiler.Start("Finalize map init");
				map.FinalizeInit();
				DeepProfiler.End();
				DeepProfiler.Start("MapComponent.MapGenerated()");
				MapComponentUtility.MapGenerated(map);
				DeepProfiler.End();
				if (parent != null)
				{
					parent.PostMapGenerate();
				}
				result = map;
			}
			finally
			{
				DeepProfiler.End();
				MapGenerator.mapBeingGenerated = null;
				Current.ProgramState = programState;
				Rand.PopState();
			}
			return result;
		}

		// Token: 0x06000BEE RID: 3054 RVA: 0x00043C24 File Offset: 0x00041E24
		public static void GenerateContentsIntoMap(IEnumerable<GenStepWithParams> genStepDefs, Map map, int seed)
		{
			MapGenerator.data.Clear();
			Rand.PushState();
			try
			{
				Rand.Seed = seed;
				RockNoises.Init(map);
				MapGenerator.tmpGenSteps.Clear();
				MapGenerator.tmpGenSteps.AddRange(from x in genStepDefs
				orderby x.def.order, x.def.index
				select x);
				for (int i = 0; i < MapGenerator.tmpGenSteps.Count; i++)
				{
					DeepProfiler.Start("GenStep - " + MapGenerator.tmpGenSteps[i].def);
					try
					{
						Rand.Seed = Gen.HashCombineInt(seed, MapGenerator.GetSeedPart(MapGenerator.tmpGenSteps, i));
						MapGenerator.tmpGenSteps[i].def.genStep.Generate(map, MapGenerator.tmpGenSteps[i].parms);
					}
					catch (Exception arg)
					{
						Log.Error("Error in GenStep: " + arg, false);
					}
					finally
					{
						DeepProfiler.End();
					}
				}
			}
			finally
			{
				Rand.PopState();
				RockNoises.Reset();
				MapGenerator.data.Clear();
			}
		}

		// Token: 0x06000BEF RID: 3055 RVA: 0x00043DA4 File Offset: 0x00041FA4
		public static T GetVar<T>(string name)
		{
			object obj;
			if (MapGenerator.data.TryGetValue(name, out obj))
			{
				return (T)((object)obj);
			}
			return default(T);
		}

		// Token: 0x06000BF0 RID: 3056 RVA: 0x00043DD0 File Offset: 0x00041FD0
		public static bool TryGetVar<T>(string name, out T var)
		{
			object obj;
			if (MapGenerator.data.TryGetValue(name, out obj))
			{
				var = (T)((object)obj);
				return true;
			}
			var = default(T);
			return false;
		}

		// Token: 0x06000BF1 RID: 3057 RVA: 0x00043E02 File Offset: 0x00042002
		public static void SetVar<T>(string name, T var)
		{
			MapGenerator.data[name] = var;
		}

		// Token: 0x06000BF2 RID: 3058 RVA: 0x00043E18 File Offset: 0x00042018
		public static MapGenFloatGrid FloatGridNamed(string name)
		{
			MapGenFloatGrid var = MapGenerator.GetVar<MapGenFloatGrid>(name);
			if (var != null)
			{
				return var;
			}
			MapGenFloatGrid mapGenFloatGrid = new MapGenFloatGrid(MapGenerator.mapBeingGenerated);
			MapGenerator.SetVar<MapGenFloatGrid>(name, mapGenFloatGrid);
			return mapGenFloatGrid;
		}

		// Token: 0x06000BF3 RID: 3059 RVA: 0x00043E44 File Offset: 0x00042044
		private static int GetSeedPart(List<GenStepWithParams> genSteps, int index)
		{
			int seedPart = genSteps[index].def.genStep.SeedPart;
			int num = 0;
			for (int i = 0; i < index; i++)
			{
				if (MapGenerator.tmpGenSteps[i].def.genStep.SeedPart == seedPart)
				{
					num++;
				}
			}
			return seedPart + num;
		}

		// Token: 0x04000983 RID: 2435
		public static Map mapBeingGenerated;

		// Token: 0x04000984 RID: 2436
		private static Dictionary<string, object> data = new Dictionary<string, object>();

		// Token: 0x04000985 RID: 2437
		private static IntVec3 playerStartSpotInt = IntVec3.Invalid;

		// Token: 0x04000986 RID: 2438
		public static List<IntVec3> rootsToUnfog = new List<IntVec3>();

		// Token: 0x04000987 RID: 2439
		private static List<GenStepWithParams> tmpGenSteps = new List<GenStepWithParams>();

		// Token: 0x04000988 RID: 2440
		public const string ElevationName = "Elevation";

		// Token: 0x04000989 RID: 2441
		public const string FertilityName = "Fertility";

		// Token: 0x0400098A RID: 2442
		public const string CavesName = "Caves";

		// Token: 0x0400098B RID: 2443
		public const string RectOfInterestName = "RectOfInterest";

		// Token: 0x0400098C RID: 2444
		public const string UsedRectsName = "UsedRects";

		// Token: 0x0400098D RID: 2445
		public const string RectOfInterestTurretsGenStepsCount = "RectOfInterestTurretsGenStepsCount";
	}
}
