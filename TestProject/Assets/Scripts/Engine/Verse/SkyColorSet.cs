using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000A1 RID: 161
	public struct SkyColorSet
	{
		// Token: 0x0600052A RID: 1322 RVA: 0x0001A120 File Offset: 0x00018320
		public SkyColorSet(Color sky, Color shadow, Color overlay, float saturation)
		{
			this.sky = sky;
			this.shadow = shadow;
			this.overlay = overlay;
			this.saturation = saturation;
		}

		// Token: 0x0600052B RID: 1323 RVA: 0x0001A140 File Offset: 0x00018340
		public static SkyColorSet Lerp(SkyColorSet A, SkyColorSet B, float t)
		{
			return new SkyColorSet
			{
				sky = Color.Lerp(A.sky, B.sky, t),
				shadow = Color.Lerp(A.shadow, B.shadow, t),
				overlay = Color.Lerp(A.overlay, B.overlay, t),
				saturation = Mathf.Lerp(A.saturation, B.saturation, t)
			};
		}

		// Token: 0x0600052C RID: 1324 RVA: 0x0001A1BC File Offset: 0x000183BC
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"(sky=",
				this.sky,
				", shadow=",
				this.shadow,
				", overlay=",
				this.overlay,
				", sat=",
				this.saturation,
				")"
			});
		}

		// Token: 0x04000302 RID: 770
		public Color sky;

		// Token: 0x04000303 RID: 771
		public Color shadow;

		// Token: 0x04000304 RID: 772
		public Color overlay;

		// Token: 0x04000305 RID: 773
		public float saturation;
	}
}
