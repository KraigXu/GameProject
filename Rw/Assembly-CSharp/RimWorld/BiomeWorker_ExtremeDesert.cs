using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x02000BDD RID: 3037
	public class BiomeWorker_ExtremeDesert : BiomeWorker
	{
		// Token: 0x06004819 RID: 18457 RVA: 0x00186A24 File Offset: 0x00184C24
		public override float GetScore(Tile tile, int tileID)
		{
			if (tile.WaterCovered)
			{
				return -100f;
			}
			if (tile.rainfall >= 340f)
			{
				return 0f;
			}
			return tile.temperature * 2.7f - 13f - tile.rainfall * 0.14f;
		}
	}
}
