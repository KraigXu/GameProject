using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000167 RID: 359
	public sealed class BlueprintGrid
	{
		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x060009FD RID: 2557 RVA: 0x00036950 File Offset: 0x00034B50
		public List<Blueprint>[] InnerArray
		{
			get
			{
				return this.innerArray;
			}
		}

		// Token: 0x060009FE RID: 2558 RVA: 0x00036958 File Offset: 0x00034B58
		public BlueprintGrid(Map map)
		{
			this.map = map;
			this.innerArray = new List<Blueprint>[map.cellIndices.NumGridCells];
		}

		// Token: 0x060009FF RID: 2559 RVA: 0x00036980 File Offset: 0x00034B80
		public void Register(Blueprint ed)
		{
			CellIndices cellIndices = this.map.cellIndices;
			CellRect cellRect = ed.OccupiedRect();
			for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
			{
				for (int j = cellRect.minX; j <= cellRect.maxX; j++)
				{
					int num = cellIndices.CellToIndex(j, i);
					if (this.innerArray[num] == null)
					{
						this.innerArray[num] = new List<Blueprint>();
					}
					this.innerArray[num].Add(ed);
				}
			}
		}

		// Token: 0x06000A00 RID: 2560 RVA: 0x00036A00 File Offset: 0x00034C00
		public void DeRegister(Blueprint ed)
		{
			CellIndices cellIndices = this.map.cellIndices;
			CellRect cellRect = ed.OccupiedRect();
			for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
			{
				for (int j = cellRect.minX; j <= cellRect.maxX; j++)
				{
					int num = cellIndices.CellToIndex(j, i);
					this.innerArray[num].Remove(ed);
					if (this.innerArray[num].Count == 0)
					{
						this.innerArray[num] = null;
					}
				}
			}
		}

		// Token: 0x04000828 RID: 2088
		private Map map;

		// Token: 0x04000829 RID: 2089
		private List<Blueprint>[] innerArray;
	}
}
