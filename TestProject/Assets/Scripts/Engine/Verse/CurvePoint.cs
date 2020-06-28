using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200000B RID: 11
	public struct CurvePoint
	{
		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000095 RID: 149 RVA: 0x000045BA File Offset: 0x000027BA
		public Vector2 Loc
		{
			get
			{
				return this.loc;
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000096 RID: 150 RVA: 0x000045C2 File Offset: 0x000027C2
		public float x
		{
			get
			{
				return this.loc.x;
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000097 RID: 151 RVA: 0x000045CF File Offset: 0x000027CF
		public float y
		{
			get
			{
				return this.loc.y;
			}
		}

		// Token: 0x06000098 RID: 152 RVA: 0x000045DC File Offset: 0x000027DC
		public CurvePoint(float x, float y)
		{
			this.loc = new Vector2(x, y);
		}

		// Token: 0x06000099 RID: 153 RVA: 0x000045EB File Offset: 0x000027EB
		public CurvePoint(Vector2 loc)
		{
			this.loc = loc;
		}

		// Token: 0x0600009A RID: 154 RVA: 0x000045F4 File Offset: 0x000027F4
		public static CurvePoint FromString(string str)
		{
			return new CurvePoint(ParseHelper.FromString<Vector2>(str));
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00004601 File Offset: 0x00002801
		public override string ToString()
		{
			return this.loc.ToStringTwoDigits();
		}

		// Token: 0x0600009C RID: 156 RVA: 0x000045BA File Offset: 0x000027BA
		public static implicit operator Vector2(CurvePoint pt)
		{
			return pt.loc;
		}

		// Token: 0x04000027 RID: 39
		private Vector2 loc;
	}
}
