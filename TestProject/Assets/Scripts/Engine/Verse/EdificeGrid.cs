using System;

namespace Verse
{
	
	public sealed class EdificeGrid
	{
		
		// (get) Token: 0x06000A40 RID: 2624 RVA: 0x0003752E File Offset: 0x0003572E
		public Building[] InnerArray
		{
			get
			{
				return this.innerArray;
			}
		}

		
		public Building this[int index]
		{
			get
			{
				return this.innerArray[index];
			}
		}

		
		public Building this[IntVec3 c]
		{
			get
			{
				return this.innerArray[this.map.cellIndices.CellToIndex(c)];
			}
		}

		
		public EdificeGrid(Map map)
		{
			this.map = map;
			this.innerArray = new Building[map.cellIndices.NumGridCells];
		}

		
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

		
		private Map map;

		
		private Building[] innerArray;
	}
}
