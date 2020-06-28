using System;

namespace Verse
{
	// Token: 0x020003FB RID: 1019
	public struct CellLine
	{
		// Token: 0x170005AB RID: 1451
		// (get) Token: 0x06001E45 RID: 7749 RVA: 0x000BC887 File Offset: 0x000BAA87
		public float ZIntercept
		{
			get
			{
				return this.zIntercept;
			}
		}

		// Token: 0x170005AC RID: 1452
		// (get) Token: 0x06001E46 RID: 7750 RVA: 0x000BC88F File Offset: 0x000BAA8F
		public float Slope
		{
			get
			{
				return this.slope;
			}
		}

		// Token: 0x06001E47 RID: 7751 RVA: 0x000BC897 File Offset: 0x000BAA97
		public CellLine(float zIntercept, float slope)
		{
			this.zIntercept = zIntercept;
			this.slope = slope;
		}

		// Token: 0x06001E48 RID: 7752 RVA: 0x000BC8A7 File Offset: 0x000BAAA7
		public CellLine(IntVec3 cell, float slope)
		{
			this.slope = slope;
			this.zIntercept = (float)cell.z - (float)cell.x * slope;
		}

		// Token: 0x06001E49 RID: 7753 RVA: 0x000BC8C8 File Offset: 0x000BAAC8
		public static CellLine Between(IntVec3 a, IntVec3 b)
		{
			float num;
			if (a.x == b.x)
			{
				num = 1E+08f;
			}
			else
			{
				num = (float)(b.z - a.z) / (float)(b.x - a.x);
			}
			return new CellLine((float)a.z - (float)a.x * num, num);
		}

		// Token: 0x06001E4A RID: 7754 RVA: 0x000BC920 File Offset: 0x000BAB20
		public bool CellIsAbove(IntVec3 c)
		{
			return (float)c.z > this.slope * (float)c.x + this.zIntercept;
		}

		// Token: 0x04001271 RID: 4721
		private float zIntercept;

		// Token: 0x04001272 RID: 4722
		private float slope;
	}
}
