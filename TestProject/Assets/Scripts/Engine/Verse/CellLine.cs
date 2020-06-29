using System;

namespace Verse
{
	
	public struct CellLine
	{
		
		// (get) Token: 0x06001E45 RID: 7749 RVA: 0x000BC887 File Offset: 0x000BAA87
		public float ZIntercept
		{
			get
			{
				return this.zIntercept;
			}
		}

		
		// (get) Token: 0x06001E46 RID: 7750 RVA: 0x000BC88F File Offset: 0x000BAA8F
		public float Slope
		{
			get
			{
				return this.slope;
			}
		}

		
		public CellLine(float zIntercept, float slope)
		{
			this.zIntercept = zIntercept;
			this.slope = slope;
		}

		
		public CellLine(IntVec3 cell, float slope)
		{
			this.slope = slope;
			this.zIntercept = (float)cell.z - (float)cell.x * slope;
		}

		
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

		
		public bool CellIsAbove(IntVec3 c)
		{
			return (float)c.z > this.slope * (float)c.x + this.zIntercept;
		}

		
		private float zIntercept;

		
		private float slope;
	}
}
