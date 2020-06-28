using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x02000BDB RID: 3035
	public class BiomeWorker_ColdBog : BiomeWorker
	{
		// Token: 0x06004815 RID: 18453 RVA: 0x00186998 File Offset: 0x00184B98
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
			if (tile.swampiness < 0.5f)
			{
				return 0f;
			}
			return -tile.temperature + 13f + tile.swampiness * 8f;
		}
	}
}
