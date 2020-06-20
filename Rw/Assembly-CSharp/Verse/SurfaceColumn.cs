using System;

namespace Verse
{
	// Token: 0x02000414 RID: 1044
	public struct SurfaceColumn
	{
		// Token: 0x06001F32 RID: 7986 RVA: 0x000C0897 File Offset: 0x000BEA97
		public SurfaceColumn(float x, SimpleCurve y)
		{
			this.x = x;
			this.y = y;
		}

		// Token: 0x04001310 RID: 4880
		public float x;

		// Token: 0x04001311 RID: 4881
		public SimpleCurve y;
	}
}
