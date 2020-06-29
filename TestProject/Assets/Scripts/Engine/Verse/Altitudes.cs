using System;
using UnityEngine;

namespace Verse
{
	
	public static class Altitudes
	{
		
		static Altitudes()
		{
			for (int i = 0; i < 32; i++)
			{
				Altitudes.Alts[i] = (float)i * 0.454545468f;
			}
		}

		
		public static float AltitudeFor(this AltitudeLayer alt)
		{
			return Altitudes.Alts[(int)alt];
		}

		
		private const int NumAltitudeLayers = 32;

		
		private static readonly float[] Alts = new float[32];

		
		private const float LayerSpacing = 0.454545468f;

		
		public const float AltInc = 0.0454545468f;

		
		public static readonly Vector3 AltIncVect = new Vector3(0f, 0.0454545468f, 0f);
	}
}
