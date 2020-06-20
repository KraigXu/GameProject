using System;

namespace Verse
{
	// Token: 0x02000169 RID: 361
	public sealed class ByteGrid : IExposable
	{
		// Token: 0x170001EA RID: 490
		public byte this[IntVec3 c]
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

		// Token: 0x170001EB RID: 491
		public byte this[int index]
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

		// Token: 0x170001EC RID: 492
		public byte this[int x, int z]
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

		// Token: 0x170001ED RID: 493
		// (get) Token: 0x06000A18 RID: 2584 RVA: 0x00036DA1 File Offset: 0x00034FA1
		public int CellsCount
		{
			get
			{
				return this.grid.Length;
			}
		}

		// Token: 0x06000A19 RID: 2585 RVA: 0x0000F2A9 File Offset: 0x0000D4A9
		public ByteGrid()
		{
		}

		// Token: 0x06000A1A RID: 2586 RVA: 0x00036DAB File Offset: 0x00034FAB
		public ByteGrid(Map map)
		{
			this.ClearAndResizeTo(map);
		}

		// Token: 0x06000A1B RID: 2587 RVA: 0x00036DBA File Offset: 0x00034FBA
		public bool MapSizeMatches(Map map)
		{
			return this.mapSizeX == map.Size.x && this.mapSizeZ == map.Size.z;
		}

		// Token: 0x06000A1C RID: 2588 RVA: 0x00036DE4 File Offset: 0x00034FE4
		public void ClearAndResizeTo(Map map)
		{
			if (this.MapSizeMatches(map) && this.grid != null)
			{
				this.Clear(0);
				return;
			}
			this.mapSizeX = map.Size.x;
			this.mapSizeZ = map.Size.z;
			this.grid = new byte[this.mapSizeX * this.mapSizeZ];
		}

		// Token: 0x06000A1D RID: 2589 RVA: 0x00036E44 File Offset: 0x00035044
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.mapSizeX, "mapSizeX", 0, false);
			Scribe_Values.Look<int>(ref this.mapSizeZ, "mapSizeZ", 0, false);
			DataExposeUtility.ByteArray(ref this.grid, "grid");
		}

		// Token: 0x06000A1E RID: 2590 RVA: 0x00036E7C File Offset: 0x0003507C
		public void Clear(byte value = 0)
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

		// Token: 0x06000A1F RID: 2591 RVA: 0x00036EC0 File Offset: 0x000350C0
		public void DebugDraw()
		{
			for (int i = 0; i < this.grid.Length; i++)
			{
				byte b = this.grid[i];
				if (b > 0)
				{
					CellRenderer.RenderCell(CellIndicesUtility.IndexToCell(i, this.mapSizeX), (float)b / 255f * 0.5f);
				}
			}
		}

		// Token: 0x04000830 RID: 2096
		private byte[] grid;

		// Token: 0x04000831 RID: 2097
		private int mapSizeX;

		// Token: 0x04000832 RID: 2098
		private int mapSizeZ;
	}
}
