using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x020008B7 RID: 2231
	public abstract class BiomeWorker
	{
		// Token: 0x060035D4 RID: 13780
		public abstract float GetScore(Tile tile, int tileID);
	}
}
