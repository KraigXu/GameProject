using System;

namespace Verse
{
	// Token: 0x020003F9 RID: 1017
	public class CellIndices
	{
		// Token: 0x170005AA RID: 1450
		// (get) Token: 0x06001E3D RID: 7741 RVA: 0x000BC7F0 File Offset: 0x000BA9F0
		public int NumGridCells
		{
			get
			{
				return this.mapSizeX * this.mapSizeZ;
			}
		}

		// Token: 0x06001E3E RID: 7742 RVA: 0x000BC7FF File Offset: 0x000BA9FF
		public CellIndices(Map map)
		{
			this.mapSizeX = map.Size.x;
			this.mapSizeZ = map.Size.z;
		}

		// Token: 0x06001E3F RID: 7743 RVA: 0x000BC829 File Offset: 0x000BAA29
		public int CellToIndex(IntVec3 c)
		{
			return CellIndicesUtility.CellToIndex(c, this.mapSizeX);
		}

		// Token: 0x06001E40 RID: 7744 RVA: 0x000BC837 File Offset: 0x000BAA37
		public int CellToIndex(int x, int z)
		{
			return CellIndicesUtility.CellToIndex(x, z, this.mapSizeX);
		}

		// Token: 0x06001E41 RID: 7745 RVA: 0x000BC846 File Offset: 0x000BAA46
		public IntVec3 IndexToCell(int ind)
		{
			return CellIndicesUtility.IndexToCell(ind, this.mapSizeX);
		}

		// Token: 0x0400126F RID: 4719
		private int mapSizeX;

		// Token: 0x04001270 RID: 4720
		private int mapSizeZ;
	}
}
