using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200014B RID: 331
	public static class Altitudes
	{
		// Token: 0x0600094C RID: 2380 RVA: 0x0003325C File Offset: 0x0003145C
		static Altitudes()
		{
			for (int i = 0; i < 32; i++)
			{
				Altitudes.Alts[i] = (float)i * 0.454545468f;
			}
		}

		// Token: 0x0600094D RID: 2381 RVA: 0x000332AA File Offset: 0x000314AA
		public static float AltitudeFor(this AltitudeLayer alt)
		{
			return Altitudes.Alts[(int)alt];
		}

		// Token: 0x040007BE RID: 1982
		private const int NumAltitudeLayers = 32;

		// Token: 0x040007BF RID: 1983
		private static readonly float[] Alts = new float[32];

		// Token: 0x040007C0 RID: 1984
		private const float LayerSpacing = 0.454545468f;

		// Token: 0x040007C1 RID: 1985
		public const float AltInc = 0.0454545468f;

		// Token: 0x040007C2 RID: 1986
		public static readonly Vector3 AltIncVect = new Vector3(0f, 0.0454545468f, 0f);
	}
}
