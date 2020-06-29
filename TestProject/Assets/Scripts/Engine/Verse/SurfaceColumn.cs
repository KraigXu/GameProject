using System;

namespace Verse
{
	
	public struct SurfaceColumn
	{
		
		public SurfaceColumn(float x, SimpleCurve y)
		{
			this.x = x;
			this.y = y;
		}

		
		public float x;

		
		public SimpleCurve y;
	}
}
