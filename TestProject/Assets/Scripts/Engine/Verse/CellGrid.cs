using System;

namespace Verse
{
	// Token: 0x0200016A RID: 362
	public class CellGrid
	{
		// Token: 0x170001EE RID: 494
		public IntVec3 this[IntVec3 c]
		{
			get
			{
				int num = CellIndicesUtility.CellToIndex(c, this.mapSizeX);
				return CellIndicesUtility.IndexToCell(this.grid[num], this.mapSizeX);
			}
			set
			{
				int num = CellIndicesUtility.CellToIndex(c, this.mapSizeX);
				this.grid[num] = CellIndicesUtility.CellToIndex(value, this.mapSizeX);
			}
		}

		// Token: 0x170001EF RID: 495
		public IntVec3 this[int index]
		{
			get
			{
				return CellIndicesUtility.IndexToCell(this.grid[index], this.mapSizeX);
			}
			set
			{
				this.grid[index] = CellIndicesUtility.CellToIndex(value, this.mapSizeX);
			}
		}

		// Token: 0x170001F0 RID: 496
		public IntVec3 this[int x, int z]
		{
			get
			{
				int num = CellIndicesUtility.CellToIndex(x, z, this.mapSizeX);
				return CellIndicesUtility.IndexToCell(this.grid[num], this.mapSizeX);
			}
			set
			{
				int num = CellIndicesUtility.CellToIndex(x, z, this.mapSizeX);
				this.grid[num] = CellIndicesUtility.CellToIndex(x, z, this.mapSizeX);
			}
		}

		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x06000A26 RID: 2598 RVA: 0x00036FF8 File Offset: 0x000351F8
		public int CellsCount
		{
			get
			{
				return this.grid.Length;
			}
		}

		// Token: 0x06000A27 RID: 2599 RVA: 0x0000F2A9 File Offset: 0x0000D4A9
		public CellGrid()
		{
		}

		// Token: 0x06000A28 RID: 2600 RVA: 0x00037002 File Offset: 0x00035202
		public CellGrid(Map map)
		{
			this.ClearAndResizeTo(map);
		}

		// Token: 0x06000A29 RID: 2601 RVA: 0x00037011 File Offset: 0x00035211
		public bool MapSizeMatches(Map map)
		{
			return this.mapSizeX == map.Size.x && this.mapSizeZ == map.Size.z;
		}

		// Token: 0x06000A2A RID: 2602 RVA: 0x0003703C File Offset: 0x0003523C
		public void ClearAndResizeTo(Map map)
		{
			if (this.MapSizeMatches(map) && this.grid != null)
			{
				this.Clear();
				return;
			}
			this.mapSizeX = map.Size.x;
			this.mapSizeZ = map.Size.z;
			this.grid = new int[this.mapSizeX * this.mapSizeZ];
			this.Clear();
		}

		// Token: 0x06000A2B RID: 2603 RVA: 0x000370A4 File Offset: 0x000352A4
		public void Clear()
		{
			int num = CellIndicesUtility.CellToIndex(IntVec3.Invalid, this.mapSizeX);
			for (int i = 0; i < this.grid.Length; i++)
			{
				this.grid[i] = num;
			}
		}

		// Token: 0x04000833 RID: 2099
		private int[] grid;

		// Token: 0x04000834 RID: 2100
		private int mapSizeX;

		// Token: 0x04000835 RID: 2101
		private int mapSizeZ;
	}
}
