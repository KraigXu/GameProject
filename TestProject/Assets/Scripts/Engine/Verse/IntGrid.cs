using System;

namespace Verse
{
	// Token: 0x02000171 RID: 369
	public sealed class IntGrid
	{
		// Token: 0x170001FC RID: 508
		public int this[IntVec3 c]
		{
			get
			{
				return this.grid[CellIndicesUtility.CellToIndex(c, this.mapSizeX)];
			}
			set
			{
				int num = CellIndicesUtility.CellToIndex(c, this.mapSizeX);
				this.grid[num] = value;
			}
		}

		// Token: 0x170001FD RID: 509
		public int this[int index]
		{
			get
			{
				return this.grid[index];
			}
			set
			{
				this.grid[index] = value;
			}
		}

		// Token: 0x170001FE RID: 510
		public int this[int x, int z]
		{
			get
			{
				return this.grid[CellIndicesUtility.CellToIndex(x, z, this.mapSizeX)];
			}
			set
			{
				this.grid[CellIndicesUtility.CellToIndex(x, z, this.mapSizeX)] = value;
			}
		}

		// Token: 0x170001FF RID: 511
		// (get) Token: 0x06000A7C RID: 2684 RVA: 0x00037F8D File Offset: 0x0003618D
		public int CellsCount
		{
			get
			{
				return this.grid.Length;
			}
		}

		// Token: 0x06000A7D RID: 2685 RVA: 0x0000F2A9 File Offset: 0x0000D4A9
		public IntGrid()
		{
		}

		// Token: 0x06000A7E RID: 2686 RVA: 0x00037F97 File Offset: 0x00036197
		public IntGrid(Map map)
		{
			this.ClearAndResizeTo(map);
		}

		// Token: 0x06000A7F RID: 2687 RVA: 0x00037FA6 File Offset: 0x000361A6
		public bool MapSizeMatches(Map map)
		{
			return this.mapSizeX == map.Size.x && this.mapSizeZ == map.Size.z;
		}

		// Token: 0x06000A80 RID: 2688 RVA: 0x00037FD0 File Offset: 0x000361D0
		public void ClearAndResizeTo(Map map)
		{
			if (this.MapSizeMatches(map) && this.grid != null)
			{
				this.Clear(0);
				return;
			}
			this.mapSizeX = map.Size.x;
			this.mapSizeZ = map.Size.z;
			this.grid = new int[this.mapSizeX * this.mapSizeZ];
		}

		// Token: 0x06000A81 RID: 2689 RVA: 0x00038030 File Offset: 0x00036230
		public void Clear(int value = 0)
		{
			if (value == 0)
			{
				Array.Clear(this.grid, 0, this.grid.Length);
				return;
			}
			for (int i = 0; i < this.grid.Length; i++)
			{
				this.grid[i] = value;
			}
		}

		// Token: 0x06000A82 RID: 2690 RVA: 0x00038074 File Offset: 0x00036274
		public void DebugDraw()
		{
			for (int i = 0; i < this.grid.Length; i++)
			{
				int num = this.grid[i];
				if (num > 0)
				{
					CellRenderer.RenderCell(CellIndicesUtility.IndexToCell(i, this.mapSizeX), (float)(num % 100) / 100f * 0.5f);
				}
			}
		}

		// Token: 0x04000843 RID: 2115
		private int[] grid;

		// Token: 0x04000844 RID: 2116
		private int mapSizeX;

		// Token: 0x04000845 RID: 2117
		private int mapSizeZ;
	}
}
