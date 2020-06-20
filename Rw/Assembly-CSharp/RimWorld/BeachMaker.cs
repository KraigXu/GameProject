using System;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x02000A5E RID: 2654
	public static class BeachMaker
	{
		// Token: 0x06003EB9 RID: 16057 RVA: 0x0014D460 File Offset: 0x0014B660
		public static void Init(Map map)
		{
			Rot4 a = Find.World.CoastDirectionAt(map.Tile);
			if (!a.IsValid)
			{
				BeachMaker.beachNoise = null;
				return;
			}
			ModuleBase moduleBase = new Perlin(0.029999999329447746, 2.0, 0.5, 3, Rand.Range(0, int.MaxValue), QualityMode.Medium);
			moduleBase = new ScaleBias(0.5, 0.5, moduleBase);
			NoiseDebugUI.StoreNoiseRender(moduleBase, "BeachMaker base", map.Size.ToIntVec2);
			ModuleBase moduleBase2 = new DistFromAxis(BeachMaker.CoastWidthRange.RandomInRange);
			if (a == Rot4.North)
			{
				moduleBase2 = new Rotate(0.0, 90.0, 0.0, moduleBase2);
				moduleBase2 = new Translate(0.0, 0.0, (double)(-(double)map.Size.z), moduleBase2);
			}
			else if (a == Rot4.East)
			{
				moduleBase2 = new Translate((double)(-(double)map.Size.x), 0.0, 0.0, moduleBase2);
			}
			else if (a == Rot4.South)
			{
				moduleBase2 = new Rotate(0.0, 90.0, 0.0, moduleBase2);
			}
			moduleBase2 = new ScaleBias(1.0, -1.0, moduleBase2);
			moduleBase2 = new Clamp(-1.0, 2.5, moduleBase2);
			NoiseDebugUI.StoreNoiseRender(moduleBase2, "BeachMaker axis bias");
			BeachMaker.beachNoise = new Add(moduleBase, moduleBase2);
			NoiseDebugUI.StoreNoiseRender(BeachMaker.beachNoise, "beachNoise");
		}

		// Token: 0x06003EBA RID: 16058 RVA: 0x0014D615 File Offset: 0x0014B815
		public static void Cleanup()
		{
			BeachMaker.beachNoise = null;
		}

		// Token: 0x06003EBB RID: 16059 RVA: 0x0014D620 File Offset: 0x0014B820
		public static TerrainDef BeachTerrainAt(IntVec3 loc, BiomeDef biome)
		{
			if (BeachMaker.beachNoise == null)
			{
				return null;
			}
			float value = BeachMaker.beachNoise.GetValue(loc);
			if (value < 0.1f)
			{
				return TerrainDefOf.WaterOceanDeep;
			}
			if (value < 0.45f)
			{
				return TerrainDefOf.WaterOceanShallow;
			}
			if (value >= 1f)
			{
				return null;
			}
			if (biome != BiomeDefOf.SeaIce)
			{
				return TerrainDefOf.Sand;
			}
			return TerrainDefOf.Ice;
		}

		// Token: 0x04002484 RID: 9348
		private static ModuleBase beachNoise;

		// Token: 0x04002485 RID: 9349
		private const float PerlinFrequency = 0.03f;

		// Token: 0x04002486 RID: 9350
		private const float MaxForDeepWater = 0.1f;

		// Token: 0x04002487 RID: 9351
		private const float MaxForShallowWater = 0.45f;

		// Token: 0x04002488 RID: 9352
		private const float MaxForSand = 1f;

		// Token: 0x04002489 RID: 9353
		private static readonly FloatRange CoastWidthRange = new FloatRange(20f, 60f);
	}
}
