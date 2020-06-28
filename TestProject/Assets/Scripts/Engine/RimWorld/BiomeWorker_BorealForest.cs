using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x02000BDA RID: 3034
	public class BiomeWorker_BorealForest : BiomeWorker
	{
		// Token: 0x06004813 RID: 18451 RVA: 0x0018695D File Offset: 0x00184B5D
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
			return 15f;
		}
	}
}
