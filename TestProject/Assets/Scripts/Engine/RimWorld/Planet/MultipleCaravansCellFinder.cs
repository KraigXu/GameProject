using System;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld.Planet
{
	// Token: 0x02001245 RID: 4677
	public static class MultipleCaravansCellFinder
	{
		// Token: 0x06006CFF RID: 27903 RVA: 0x00262614 File Offset: 0x00260814
		public static void FindStartingCellsFor2Groups(Map map, out IntVec3 first, out IntVec3 second)
		{
			for (int i = 0; i < 10; i++)
			{
				if (MultipleCaravansCellFinder.TryFindOppositeSpots(map, 0.05f, out first, out second))
				{
					return;
				}
			}
			for (int j = 0; j < 10; j++)
			{
				if (MultipleCaravansCellFinder.TryFindOppositeSpots(map, 0.15f, out first, out second))
				{
					return;
				}
			}
			if (!CellFinder.TryFindRandomEdgeCellWith((IntVec3 x) => x.Standable(map) && !x.Fogged(map), map, CellFinder.EdgeRoadChance_Neutral, out first))
			{
				Log.Error("Could not find any valid starting cell for a caravan.", false);
				first = CellFinder.RandomCell(map);
				second = CellFinder.RandomCell(map);
				return;
			}
			IntVec3 localFirst = first;
			float tryMinDistBetweenSpots = (float)Mathf.Max(map.Size.x, map.Size.z) * 0.6f;
			TraverseParms traverseParams = TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false);
			if (!CellFinder.TryFindRandomEdgeCellWith((IntVec3 x) => x.Standable(map) && !x.Fogged(map) && !x.InHorDistOf(localFirst, tryMinDistBetweenSpots) && map.reachability.CanReach(x, localFirst, PathEndMode.OnCell, traverseParams), map, CellFinder.EdgeRoadChance_Neutral, out second) && !CellFinder.TryFindRandomEdgeCellWith((IntVec3 x) => x.Standable(map) && !x.Fogged(map) && map.reachability.CanReach(x, localFirst, PathEndMode.OnCell, traverseParams), map, 0.5f, out second))
			{
				Log.Error("Could not find any valid starting cell for a caravan.", false);
				second = CellFinder.RandomCell(map);
				return;
			}
			first = CellFinder.RandomClosewalkCellNear(first, map, 7, null);
			second = CellFinder.RandomClosewalkCellNear(second, map, 7, null);
		}

		// Token: 0x06006D00 RID: 27904 RVA: 0x00262798 File Offset: 0x00260998
		private static bool TryFindOppositeSpots(Map map, float maxDistPctToOppositeSpots, out IntVec3 first, out IntVec3 second)
		{
			IntVec3 intVec = MultipleCaravansCellFinder.RandomSpotNearEdge(map);
			IntVec3 intVec2 = MultipleCaravansCellFinder.OppositeSpot(intVec, map);
			int num = Mathf.Min(map.Size.x, map.Size.z);
			CellRect cellRect = CellRect.CenteredOn(intVec, Mathf.Max(Mathf.RoundToInt((float)num * maxDistPctToOppositeSpots), 1)).ClipInsideMap(map);
			CellRect cellRect2 = CellRect.CenteredOn(intVec2, Mathf.Max(Mathf.RoundToInt((float)num * maxDistPctToOppositeSpots), 1)).ClipInsideMap(map);
			for (int i = 0; i < 20; i++)
			{
				IntVec3 intVec3 = (i == 0) ? intVec : cellRect.RandomCell;
				IntVec3 intVec4 = (i == 0) ? intVec2 : cellRect2.RandomCell;
				if (intVec3.Standable(map) && !intVec3.Fogged(map) && intVec4.Standable(map) && !intVec4.Fogged(map) && map.reachability.CanReach(intVec3, intVec4, PathEndMode.OnCell, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false)))
				{
					first = intVec3;
					second = intVec4;
					return true;
				}
			}
			first = IntVec3.Invalid;
			second = IntVec3.Invalid;
			return false;
		}

		// Token: 0x06006D01 RID: 27905 RVA: 0x002628B8 File Offset: 0x00260AB8
		private static IntVec3 RandomSpotNearEdge(Map map)
		{
			CellRect cellRect = CellRect.WholeMap(map);
			cellRect.minX += Mathf.RoundToInt((float)map.Size.x * 0.2f);
			cellRect.minZ += Mathf.RoundToInt((float)map.Size.z * 0.2f);
			cellRect.maxX -= Mathf.RoundToInt((float)map.Size.x * 0.2f);
			cellRect.maxZ -= Mathf.RoundToInt((float)map.Size.z * 0.2f);
			return cellRect.EdgeCells.RandomElement<IntVec3>();
		}

		// Token: 0x06006D02 RID: 27906 RVA: 0x00262960 File Offset: 0x00260B60
		private static IntVec3 OppositeSpot(IntVec3 spot, Map map)
		{
			return new IntVec3(map.Size.x - spot.x, spot.y, map.Size.z - spot.z);
		}

		// Token: 0x040043C0 RID: 17344
		private const int TriesToFindPerfectOppositeSpots = 10;

		// Token: 0x040043C1 RID: 17345
		private const int TriesToFindGoodEnoughOppositeSpots = 10;

		// Token: 0x040043C2 RID: 17346
		private const int TriesToFindMatchingPair = 20;

		// Token: 0x040043C3 RID: 17347
		private const float PerfectIfDistPctToOppositeSpotsAtMost = 0.05f;

		// Token: 0x040043C4 RID: 17348
		private const float GoodEnoughIfDistPctToOppositeSpotsAtMost = 0.15f;

		// Token: 0x040043C5 RID: 17349
		private const float SpotDistPctToEdge = 0.2f;

		// Token: 0x040043C6 RID: 17350
		private const float TryMinDistPctBetweenFallbackEdgeCells = 0.6f;
	}
}
