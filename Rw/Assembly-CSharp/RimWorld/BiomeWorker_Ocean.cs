using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x02000BE0 RID: 3040
	public class BiomeWorker_Ocean : BiomeWorker
	{
		// Token: 0x06004821 RID: 18465 RVA: 0x00186BDB File Offset: 0x00184DDB
		public override float GetScore(Tile tile, int tileID)
		{
			if (!tile.WaterCovered)
			{
				return -100f;
			}
			return 0f;
		}
	}
}
