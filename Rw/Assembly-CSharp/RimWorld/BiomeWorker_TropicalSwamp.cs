using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x02000BE4 RID: 3044
	public class BiomeWorker_TropicalSwamp : BiomeWorker
	{
		// Token: 0x06004829 RID: 18473 RVA: 0x00186D4C File Offset: 0x00184F4C
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
			if (tile.swampiness < 0.5f)
			{
				return 0f;
			}
			return 28f + (tile.temperature - 20f) * 1.5f + (tile.rainfall - 600f) / 165f + tile.swampiness * 3f;
		}
	}
}
