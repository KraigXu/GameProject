using System;

namespace Verse
{
	// Token: 0x020001B0 RID: 432
	public struct FloodFillRange
	{
		// Token: 0x06000C07 RID: 3079 RVA: 0x00044250 File Offset: 0x00042450
		public FloodFillRange(int minX, int maxX, int y)
		{
			this.minX = minX;
			this.maxX = maxX;
			this.z = y;
		}

		// Token: 0x04000992 RID: 2450
		public int minX;

		// Token: 0x04000993 RID: 2451
		public int maxX;

		// Token: 0x04000994 RID: 2452
		public int z;
	}
}
