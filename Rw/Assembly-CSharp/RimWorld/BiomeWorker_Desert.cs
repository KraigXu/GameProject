using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x02000BDC RID: 3036
	public class BiomeWorker_Desert : BiomeWorker
	{
		// Token: 0x06004817 RID: 18455 RVA: 0x001869F3 File Offset: 0x00184BF3
		public override float GetScore(Tile tile, int tileID)
		{
			if (tile.WaterCovered)
			{
				return -100f;
			}
			if (tile.rainfall >= 600f)
			{
				return 0f;
			}
			return tile.temperature + 0.0001f;
		}
	}
}
