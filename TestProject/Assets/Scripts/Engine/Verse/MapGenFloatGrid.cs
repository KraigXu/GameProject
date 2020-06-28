using System;

namespace Verse
{
	// Token: 0x020001AB RID: 427
	public class MapGenFloatGrid
	{
		// Token: 0x17000253 RID: 595
		public float this[IntVec3 c]
		{
			get
			{
				return this.grid[this.map.cellIndices.CellToIndex(c)];
			}
			set
			{
				this.grid[this.map.cellIndices.CellToIndex(c)] = value;
			}
		}

		// Token: 0x06000BF7 RID: 3063 RVA: 0x00043EF9 File Offset: 0x000420F9
		public MapGenFloatGrid(Map map)
		{
			this.map = map;
			this.grid = new float[map.cellIndices.NumGridCells];
		}

		// Token: 0x0400098E RID: 2446
		private Map map;

		// Token: 0x0400098F RID: 2447
		private float[] grid;
	}
}
