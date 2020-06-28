using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001202 RID: 4610
	public static class GenPlanetMorphology
	{
		// Token: 0x06006A99 RID: 27289 RVA: 0x00252C68 File Offset: 0x00250E68
		public static void Erode(List<int> tiles, int count, Predicate<int> extraPredicate = null)
		{
			if (count <= 0)
			{
				return;
			}
			WorldGrid worldGrid = Find.WorldGrid;
			GenPlanetMorphology.tilesSet.Clear();
			GenPlanetMorphology.tilesSet.AddRange(tiles);
			GenPlanetMorphology.tmpEdgeTiles.Clear();
			for (int i = 0; i < tiles.Count; i++)
			{
				worldGrid.GetTileNeighbors(tiles[i], GenPlanetMorphology.tmpNeighbors);
				for (int j = 0; j < GenPlanetMorphology.tmpNeighbors.Count; j++)
				{
					if (!GenPlanetMorphology.tilesSet.Contains(GenPlanetMorphology.tmpNeighbors[j]))
					{
						GenPlanetMorphology.tmpEdgeTiles.Add(tiles[i]);
						break;
					}
				}
			}
			if (!GenPlanetMorphology.tmpEdgeTiles.Any<int>())
			{
				return;
			}
			GenPlanetMorphology.tmpOutput.Clear();
			Predicate<int> passCheck;
			if (extraPredicate != null)
			{
				passCheck = ((int x) => GenPlanetMorphology.tilesSet.Contains(x) && extraPredicate(x));
			}
			else
			{
				passCheck = ((int x) => GenPlanetMorphology.tilesSet.Contains(x));
			}
			Find.WorldFloodFiller.FloodFill(-1, passCheck, delegate(int tile, int traversalDist)
			{
				if (traversalDist >= count)
				{
					GenPlanetMorphology.tmpOutput.Add(tile);
				}
				return false;
			}, int.MaxValue, GenPlanetMorphology.tmpEdgeTiles);
			tiles.Clear();
			tiles.AddRange(GenPlanetMorphology.tmpOutput);
		}

		// Token: 0x06006A9A RID: 27290 RVA: 0x00252DA0 File Offset: 0x00250FA0
		public static void Dilate(List<int> tiles, int count, Predicate<int> extraPredicate = null)
		{
			if (count <= 0)
			{
				return;
			}
			WorldFloodFiller worldFloodFiller = Find.WorldFloodFiller;
			int rootTile = -1;
			Predicate<int> passCheck = extraPredicate;
			if (extraPredicate == null && (passCheck = GenPlanetMorphology.<>c.<>9__5_0) == null)
			{
				passCheck = (GenPlanetMorphology.<>c.<>9__5_0 = ((int x) => true));
			}
			worldFloodFiller.FloodFill(rootTile, passCheck, delegate(int tile, int traversalDist)
			{
				if (traversalDist > count)
				{
					return true;
				}
				if (traversalDist != 0)
				{
					tiles.Add(tile);
				}
				return false;
			}, int.MaxValue, tiles);
		}

		// Token: 0x06006A9B RID: 27291 RVA: 0x00252E11 File Offset: 0x00251011
		public static void Open(List<int> tiles, int count)
		{
			GenPlanetMorphology.Erode(tiles, count, null);
			GenPlanetMorphology.Dilate(tiles, count, null);
		}

		// Token: 0x06006A9C RID: 27292 RVA: 0x00252E23 File Offset: 0x00251023
		public static void Close(List<int> tiles, int count)
		{
			GenPlanetMorphology.Dilate(tiles, count, null);
			GenPlanetMorphology.Erode(tiles, count, null);
		}

		// Token: 0x04004285 RID: 17029
		private static HashSet<int> tmpOutput = new HashSet<int>();

		// Token: 0x04004286 RID: 17030
		private static HashSet<int> tilesSet = new HashSet<int>();

		// Token: 0x04004287 RID: 17031
		private static List<int> tmpNeighbors = new List<int>();

		// Token: 0x04004288 RID: 17032
		private static List<int> tmpEdgeTiles = new List<int>();
	}
}
