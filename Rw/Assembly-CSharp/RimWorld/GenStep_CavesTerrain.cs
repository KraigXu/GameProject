using System;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x02000A4A RID: 2634
	public class GenStep_CavesTerrain : GenStep
	{
		// Token: 0x17000B0F RID: 2831
		// (get) Token: 0x06003E4E RID: 15950 RVA: 0x00148B7B File Offset: 0x00146D7B
		public override int SeedPart
		{
			get
			{
				return 1921024373;
			}
		}

		// Token: 0x06003E4F RID: 15951 RVA: 0x00148B84 File Offset: 0x00146D84
		public override void Generate(Map map, GenStepParams parms)
		{
			if (!Find.World.HasCaves(map.Tile))
			{
				return;
			}
			Perlin perlin = new Perlin(0.079999998211860657, 2.0, 0.5, 6, Rand.Int, QualityMode.Medium);
			Perlin perlin2 = new Perlin(0.15999999642372131, 2.0, 0.5, 6, Rand.Int, QualityMode.Medium);
			MapGenFloatGrid caves = MapGenerator.Caves;
			foreach (IntVec3 intVec in map.AllCells)
			{
				if (caves[intVec] > 0f && !intVec.GetTerrain(map).IsRiver)
				{
					float num = (float)perlin.GetValue((double)intVec.x, 0.0, (double)intVec.z);
					float num2 = (float)perlin2.GetValue((double)intVec.x, 0.0, (double)intVec.z);
					if (num > 0.93f)
					{
						map.terrainGrid.SetTerrain(intVec, TerrainDefOf.WaterShallow);
					}
					else if (num2 > 0.55f)
					{
						map.terrainGrid.SetTerrain(intVec, TerrainDefOf.Gravel);
					}
				}
			}
		}

		// Token: 0x0400244F RID: 9295
		private const float WaterFrequency = 0.08f;

		// Token: 0x04002450 RID: 9296
		private const float GravelFrequency = 0.16f;

		// Token: 0x04002451 RID: 9297
		private const float WaterThreshold = 0.93f;

		// Token: 0x04002452 RID: 9298
		private const float GravelThreshold = 0.55f;
	}
}
