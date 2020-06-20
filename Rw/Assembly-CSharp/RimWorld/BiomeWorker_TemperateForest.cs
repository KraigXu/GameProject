using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x02000BE1 RID: 3041
	public class BiomeWorker_TemperateForest : BiomeWorker
	{
		// Token: 0x06004823 RID: 18467 RVA: 0x00186BF0 File Offset: 0x00184DF0
		public override float GetScore(Tile tile, int tileID)
		{
			if (tile.WaterCovered)
			{
				return -100f;
			}
			if (tile.temperature < -10f)
			{
				return 0f;
			}
			if (tile.rainfall < 600f)
			{
				return 0f;
			}
			return 15f + (tile.temperature - 7f) + (tile.rainfall - 600f) / 180f;
		}
	}
}
