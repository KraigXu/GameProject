using System;

namespace Verse
{
	// Token: 0x020001DA RID: 474
	public struct CachedTempInfo
	{
		// Token: 0x06000D73 RID: 3443 RVA: 0x0004CD74 File Offset: 0x0004AF74
		public static CachedTempInfo NewCachedTempInfo()
		{
			CachedTempInfo result = default(CachedTempInfo);
			result.Reset();
			return result;
		}

		// Token: 0x06000D74 RID: 3444 RVA: 0x0004CD91 File Offset: 0x0004AF91
		public void Reset()
		{
			this.roomGroupID = -1;
			this.numCells = 0;
			this.temperature = 0f;
		}

		// Token: 0x06000D75 RID: 3445 RVA: 0x0004CDAC File Offset: 0x0004AFAC
		public CachedTempInfo(int roomGroupID, int numCells, float temperature)
		{
			this.roomGroupID = roomGroupID;
			this.numCells = numCells;
			this.temperature = temperature;
		}

		// Token: 0x04000A47 RID: 2631
		public int roomGroupID;

		// Token: 0x04000A48 RID: 2632
		public int numCells;

		// Token: 0x04000A49 RID: 2633
		public float temperature;
	}
}
