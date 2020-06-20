using System;

namespace Verse
{
	// Token: 0x020003FA RID: 1018
	public static class CellIndicesUtility
	{
		// Token: 0x06001E42 RID: 7746 RVA: 0x000BC854 File Offset: 0x000BAA54
		public static int CellToIndex(IntVec3 c, int mapSizeX)
		{
			return c.z * mapSizeX + c.x;
		}

		// Token: 0x06001E43 RID: 7747 RVA: 0x000BC865 File Offset: 0x000BAA65
		public static int CellToIndex(int x, int z, int mapSizeX)
		{
			return z * mapSizeX + x;
		}

		// Token: 0x06001E44 RID: 7748 RVA: 0x000BC86C File Offset: 0x000BAA6C
		public static IntVec3 IndexToCell(int ind, int mapSizeX)
		{
			int newX = ind % mapSizeX;
			int newZ = ind / mapSizeX;
			return new IntVec3(newX, 0, newZ);
		}
	}
}
