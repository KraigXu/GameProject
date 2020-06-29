using System;
using UnityEngine;

namespace Verse
{
	
	public struct CurvePoint
	{
		
		
		public Vector2 Loc
		{
			get
			{
				return this.loc;
			}
		}

		
		
		public float x
		{
			get
			{
				return this.loc.x;
			}
		}

		
		
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
