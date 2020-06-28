using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001203 RID: 4611
	public static class GenWorldClosest
	{
		// Token: 0x06006A9E RID: 27294 RVA: 0x00252E60 File Offset: 0x00251060
		public static bool TryFindClosestTile(int rootTile, Predicate<int> predicate, out int foundTile, int maxTilesToScan = 2147483647, bool canSearchThroughImpassable = true)
		{
			int foundTileLocal = -1;
			Find.WorldFloodFiller.FloodFill(rootTile, (int x) => canSearchThroughImpassable || !Find.World.Impassable(x), delegate(int t)
			{
				bool flag = predicate(t);
				if (flag)
				{
					foundTileLocal = t;
				}
				return flag;
			}, maxTilesToScan, null);
			foundTile = foundTileLocal;
			return foundTileLocal >= 0;
		}

		// Token: 0x06006A9F RID: 27295 RVA: 0x00252EC2 File Offset: 0x002510C2
		public static bool TryFindClosestPassableTile(int rootTile, out int foundTile)
		{
			return GenWorldClosest.TryFindClosestTile(rootTile, (int x) => !Find.World.Impassable(x), out foundTile, int.MaxValue, true);
		}
	}
}
