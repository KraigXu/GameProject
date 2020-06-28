using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200120F RID: 4623
	public static class WorldGenerator
	{
		// Token: 0x170011E6 RID: 4582
		// (get) Token: 0x06006AED RID: 27373 RVA: 0x00254E18 File Offset: 0x00253018
		public static IEnumerable<WorldGenStepDef> GenStepsInOrder
		{
			get
			{
				return from x in DefDatabase<WorldGenStepDef>.AllDefs
				orderby x.order, x.index
				select x;
			}
		}

		// Token: 0x06006AEE RID: 27374 RVA: 0x00254E74 File Offset: 0x00253074
		public static World GenerateWorld(float planetCoverage, string seedString, OverallRainfall overallRainfall, OverallTemperature overallTemperature, OverallPopulation population)
		{
			DeepProfiler.Start("GenerateWorld");
			Rand.PushState();
			int seedFromSeedString = WorldGenerator.GetSeedFromSeedString(seedString);
			Rand.Seed = seedFromSeedString;
			World creatingWorld;
			try
			{
				Current.CreatingWorld = new World();
				Current.CreatingWorld.info.seedString = seedString;
				Current.CreatingWorld.info.planetCoverage = planetCoverage;
				Current.CreatingWorld.info.overallRainfall = overallRainfall;
				Current.CreatingWorld.info.overallTemperature = overallTemperature;
				Current.CreatingWorld.info.overallPopulation = population;
				Current.CreatingWorld.info.name = NameGenerator.GenerateName(RulePackDefOf.NamerWorld, null, false, null, null);
				WorldGenerator.tmpGenSteps.Clear();
				WorldGenerator.tmpGenSteps.AddRange(WorldGenerator.GenStepsInOrder);
				for (int i = 0; i < WorldGenerator.tmpGenSteps.Count; i++)
				{
					DeepProfiler.Start("WorldGenStep - " + WorldGenerator.tmpGenSteps[i]);
					try
					{
						Rand.Seed = Gen.HashCombineInt(seedFromSeedString, WorldGenerator.GetSeedPart(WorldGenerator.tmpGenSteps, i));
						WorldGenerator.tmpGenSteps[i].worldGenStep.GenerateFresh(seedString);
					}
					catch (Exception arg)
					{
						Log.Error("Error in WorldGenStep: " + arg, false);
					}
					finally
					{
						DeepProfiler.End();
					}
				}
				Rand.Seed = seedFromSeedString;
				Current.CreatingWorld.grid.StandardizeTileData();
				Current.CreatingWorld.FinalizeInit();
				Find.Scenario.PostWorldGenerate();
				creatingWorld = Current.CreatingWorld;
			}
			finally
			{
				Rand.PopState();
				DeepProfiler.End();
				Current.CreatingWorld = null;
			}
			return creatingWorld;
		}

		// Token: 0x06006AEF RID: 27375 RVA: 0x00255034 File Offset: 0x00253234
		public static void GenerateWithoutWorldData(string seedString)
		{
			int seedFromSeedString = WorldGenerator.GetSeedFromSeedString(seedString);
			WorldGenerator.tmpGenSteps.Clear();
			WorldGenerator.tmpGenSteps.AddRange(WorldGenerator.GenStepsInOrder);
			Rand.PushState();
			for (int i = 0; i < WorldGenerator.tmpGenSteps.Count; i++)
			{
				try
				{
					Rand.Seed = Gen.HashCombineInt(seedFromSeedString, WorldGenerator.GetSeedPart(WorldGenerator.tmpGenSteps, i));
					WorldGenerator.tmpGenSteps[i].worldGenStep.GenerateWithoutWorldData(seedString);
				}
				catch (Exception arg)
				{
					Log.Error("Error in WorldGenStep: " + arg, false);
				}
			}
			Rand.PopState();
		}

		// Token: 0x06006AF0 RID: 27376 RVA: 0x002550D4 File Offset: 0x002532D4
		public static void GenerateFromScribe(string seedString)
		{
			int seedFromSeedString = WorldGenerator.GetSeedFromSeedString(seedString);
			WorldGenerator.tmpGenSteps.Clear();
			WorldGenerator.tmpGenSteps.AddRange(WorldGenerator.GenStepsInOrder);
			Rand.PushState();
			for (int i = 0; i < WorldGenerator.tmpGenSteps.Count; i++)
			{
				try
				{
					Rand.Seed = Gen.HashCombineInt(seedFromSeedString, WorldGenerator.GetSeedPart(WorldGenerator.tmpGenSteps, i));
					WorldGenerator.tmpGenSteps[i].worldGenStep.GenerateFromScribe(seedString);
				}
				catch (Exception arg)
				{
					Log.Error("Error in WorldGenStep: " + arg, false);
				}
			}
			Rand.PopState();
		}

		// Token: 0x06006AF1 RID: 27377 RVA: 0x00255174 File Offset: 0x00253374
		private static int GetSeedPart(List<WorldGenStepDef> genSteps, int index)
		{
			int seedPart = genSteps[index].worldGenStep.SeedPart;
			int num = 0;
			for (int i = 0; i < index; i++)
			{
				if (WorldGenerator.tmpGenSteps[i].worldGenStep.SeedPart == seedPart)
				{
					num++;
				}
			}
			return seedPart + num;
		}

		// Token: 0x06006AF2 RID: 27378 RVA: 0x002551C0 File Offset: 0x002533C0
		private static int GetSeedFromSeedString(string seedString)
		{
			return GenText.StableStringHash(seedString);
		}

		// Token: 0x040042D3 RID: 17107
		private static List<WorldGenStepDef> tmpGenSteps = new List<WorldGenStepDef>();

		// Token: 0x040042D4 RID: 17108
		public const float DefaultPlanetCoverage = 0.3f;

		// Token: 0x040042D5 RID: 17109
		public const OverallRainfall DefaultOverallRainfall = OverallRainfall.Normal;

		// Token: 0x040042D6 RID: 17110
		public const OverallPopulation DefaultOverallPopulation = OverallPopulation.Normal;

		// Token: 0x040042D7 RID: 17111
		public const OverallTemperature DefaultOverallTemperature = OverallTemperature.Normal;
	}
}
