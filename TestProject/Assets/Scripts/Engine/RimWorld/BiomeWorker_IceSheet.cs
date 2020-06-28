using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x02000BDE RID: 3038
	public class BiomeWorker_IceSheet : BiomeWorker
	{
		// Token: 0x0600481B RID: 18459 RVA: 0x00186A71 File Offset: 0x00184C71
		public override float GetScore(Tile tile, int tileID)
		{
			if (tile.WaterCovered)
			{
				return -100f;
			}
			return BiomeWorker_IceSheet.PermaIceScore(tile);
		}

		// Token: 0x0600481C RID: 18460 RVA: 0x00186A87 File Offset: 0x00184C87
		public static float PermaIceScore(Tile tile)
		{
			return -20f + -tile.temperature * 2f;
		}
	}
}
