using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D69 RID: 3433
	public class CompProperties_TemperatureRuinable : CompProperties
	{
		// Token: 0x0600539B RID: 21403 RVA: 0x001BF467 File Offset: 0x001BD667
		public CompProperties_TemperatureRuinable()
		{
			this.compClass = typeof(CompTemperatureRuinable);
		}

		// Token: 0x04002E2E RID: 11822
		public float minSafeTemperature;

		// Token: 0x04002E2F RID: 11823
		public float maxSafeTemperature = 100f;

		// Token: 0x04002E30 RID: 11824
		public float progressPerDegreePerTick = 1E-05f;
	}
}
