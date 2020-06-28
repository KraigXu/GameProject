using System;

namespace Verse
{
	// Token: 0x0200016D RID: 365
	public sealed class EdificeGrid
	{
		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x06000A40 RID: 2624 RVA: 0x0003752E File Offset: 0x0003572E
		public Building[] InnerArray
		{
			get
			{
				return this.innerArray;
			}
		}

		// Token: 0x170001F6 RID: 502
		public Building this[int index]
		{
			get
			{
				return this.innerArray[index];
			}
		}

		// Token: 0x170001F7 RID: 503
		public Building this[IntVec3 c]
		{
			get
			{
				return this.innerArray[this.map.cellIndices.CellToIndex(c)];
			}
		}

		// Token: 0x06000A43 RID: 2627 RVA: 0x0003755A File Offset: 0x0003575A
		public EdificeGrid(Map map)
		{
			this.map = map;
			this.innerArray = new Building[map.cellIndices.NumGridCells];
		}

		// Token: 0x06000A44 RID: 2628 RVA: 0x00037580 File Offset: 0x00035780
		public void Register(Building ed)
		{
			CellIndices cellIndices = this.map.cellIndices;
			CellRect cellRect = ed.OccupiedRect();
			for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
			{
				for (int j = cellRect.minX; j <= cellRect.maxX; j++)
				{
					IntVec3 c = new IntVec3(j, 0, i);
					this.innerArray[cellIndices.CellToIndex(c)] = ed;
				}
			}
		}

		// Token: 0x06000A45 RID: 2629 RVA: 0x000375E8 File Offset: 0x000357E8
		public void DeRegister(Building ed)
		{
			CellIndices cellIndices = this.map.cellIndices;
			CellRect cellRect = ed.OccupiedRect();
			for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
			{
				for (int j = cellRect.minX; j <= cellRect.maxX; j++)
				{
					this.innerArray[cellIndices.CellToIndex(j, i)] = null;
				}
			}
		}

		// Token: 0x0400083C RID: 2108
		private Map map;

		// Token: 0x0400083D RID: 2109
		private Building[] innerArray;
	}
}
