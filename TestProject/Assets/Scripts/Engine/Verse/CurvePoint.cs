using System;
using UnityEngine;

namespace Verse
{
	
	public struct CurvePoint
	{
		
		// (get) Token: 0x06000095 RID: 149 RVA: 0x000045BA File Offset: 0x000027BA
		public Vector2 Loc
		{
			get
			{
				return this.loc;
			}
		}

		
		// (get) Token: 0x06000096 RID: 150 RVA: 0x000045C2 File Offset: 0x000027C2
		public float x
		{
			get
			{
				return this.loc.x;
			}
		}

		
		// (get) Token: 0x06000097 RID: 151 RVA: 0x000045CF File Offset: 0x000027CF
		public float y
		{
			get
			{
				return this.loc.y;
			}
		}

		
		public CurvePoint(float x, float y)
		{
			this.loc = new Vector2(x, y);
		}

		
		public CurvePoint(Vector2 loc)
		{
			this.loc = loc;
		}

		
		public static CurvePoint FromString(string str)
		{
			return new CurvePoint(ParseHelper.FromString<Vector2>(str));
		}

		
		public override string ToString()
		{
			return this.loc.ToStringTwoDigits();
		}

		
		public static implicit operator Vector2(CurvePoint pt)
		{
			return pt.loc;
		}

		
		private Vector2 loc;
	}
}
