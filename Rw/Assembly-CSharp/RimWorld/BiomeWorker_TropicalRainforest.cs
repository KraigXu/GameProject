using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x02000BE3 RID: 3043
	public class BiomeWorker_TropicalRainforest : BiomeWorker
	{
		// Token: 0x06004827 RID: 18471 RVA: 0x00186CE0 File Offset: 0x00184EE0
		public override float GetScore(Tile tile, int tileID)
		{
			if (tile.WaterCovered)
			{
				return -100f;
			}
			if (tile.temperature < 15f)
			{
				return 0f;
			}
			if (tile.rainfall < 2000f)
			{
				return 0f;
			}
			return 28f + (tile.temperature - 20f) * 1.5f + (tile.rainfall - 600f) / 165f;
		}
	}
}
