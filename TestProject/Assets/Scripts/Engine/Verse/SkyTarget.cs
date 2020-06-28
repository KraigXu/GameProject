using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001E1 RID: 481
	public struct SkyTarget
	{
		// Token: 0x06000D95 RID: 3477 RVA: 0x0004DD28 File Offset: 0x0004BF28
		public SkyTarget(float glow, SkyColorSet colorSet, float lightsourceShineSize, float lightsourceShineIntensity)
		{
			this.glow = glow;
			this.lightsourceShineSize = lightsourceShineSize;
			this.lightsourceShineIntensity = lightsourceShineIntensity;
			this.colors = colorSet;
		}

		// Token: 0x06000D96 RID: 3478 RVA: 0x0004DD48 File Offset: 0x0004BF48
		public static SkyTarget Lerp(SkyTarget A, SkyTarget B, float t)
		{
			return new SkyTarget
			{
				colors = SkyColorSet.Lerp(A.colors, B.colors, t),
				glow = Mathf.Lerp(A.glow, B.glow, t),
				lightsourceShineSize = Mathf.Lerp(A.lightsourceShineSize, B.lightsourceShineSize, t),
				lightsourceShineIntensity = Mathf.Lerp(A.lightsourceShineIntensity, B.lightsourceShineIntensity, t)
			};
		}

		// Token: 0x06000D97 RID: 3479 RVA: 0x0004DDC4 File Offset: 0x0004BFC4
		public static SkyTarget LerpDarken(SkyTarget A, SkyTarget B, float t)
		{
			return new SkyTarget
			{
				colors = SkyColorSet.Lerp(A.colors, B.colors, t),
				glow = Mathf.Lerp(A.glow, Mathf.Min(A.glow, B.glow), t),
				lightsourceShineSize = Mathf.Lerp(A.lightsourceShineSize, Mathf.Min(A.lightsourceShineSize, B.lightsourceShineSize), t),
				lightsourceShineIntensity = Mathf.Lerp(A.lightsourceShineIntensity, Mathf.Min(A.lightsourceShineIntensity, B.lightsourceShineIntensity), t)
			};
		}

		// Token: 0x06000D98 RID: 3480 RVA: 0x0004DE60 File Offset: 0x0004C060
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"(glow=",
				this.glow.ToString("F2"),
				", colors=",
				this.colors.ToString(),
				", lightsourceShineSize=",
				this.lightsourceShineSize.ToString(),
				", lightsourceShineIntensity=",
				this.lightsourceShineIntensity.ToString(),
				")"
			});
		}

		// Token: 0x04000A6A RID: 2666
		public float glow;

		// Token: 0x04000A6B RID: 2667
		public SkyColorSet colors;

		// Token: 0x04000A6C RID: 2668
		public float lightsourceShineSize;

		// Token: 0x04000A6D RID: 2669
		public float lightsourceShineIntensity;
	}
}
