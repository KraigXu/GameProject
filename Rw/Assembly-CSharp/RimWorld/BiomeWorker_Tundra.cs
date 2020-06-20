using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x02000BE5 RID: 3045
	public class BiomeWorker_Tundra : BiomeWorker
	{
		// Token: 0x0600482B RID: 18475 RVA: 0x00186DD8 File Offset: 0x00184FD8
		public override float GetScore(Tile tile, int tileID)
		{
			if (tile.WaterCovered)
			{
				return -100f;
			}
			return -tile.temperature;
		}
	}
}
