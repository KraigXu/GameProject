using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200042A RID: 1066
	public static class DijkstraUtility
	{
		// Token: 0x06001FDA RID: 8154 RVA: 0x000C2F88 File Offset: 0x000C1188
		public static IEnumerable<IntVec3> AdjacentCellsNeighborsGetter(IntVec3 cell, Map map)
		{
			DijkstraUtility.adjacentCells.Clear();
			IntVec3[] array = GenAdj.AdjacentCells;
			for (int i = 0; i < array.Length; i++)
			{
				IntVec3 intVec = cell + array[i];
				if (intVec.InBounds(map))
				{
					DijkstraUtility.adjacentCells.Add(intVec);
				}
			}
			return DijkstraUtility.adjacentCells;
		}

		// Token: 0x040013A8 RID: 5032
		private static List<IntVec3> adjacentCells = new List<IntVec3>();
	}
}
