using System;

namespace Verse
{
	// Token: 0x02000087 RID: 135
	public class CompProperties_AffectsSky : CompProperties
	{
		// Token: 0x060004C2 RID: 1218 RVA: 0x00017DB2 File Offset: 0x00015FB2
		public CompProperties_AffectsSky()
		{
			this.compClass = typeof(CompAffectsSky);
		}

		// Token: 0x0400021B RID: 539
		public float glow = 1f;

		// Token: 0x0400021C RID: 540
		public SkyColorSet skyColors;

		// Token: 0x0400021D RID: 541
		public float lightsourceShineSize = 1f;

		// Token: 0x0400021E RID: 542
		public float lightsourceShineIntensity = 1f;

		// Token: 0x0400021F RID: 543
		public bool lerpDarken;
	}
}
