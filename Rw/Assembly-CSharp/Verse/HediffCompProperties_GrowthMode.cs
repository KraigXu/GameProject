using System;

namespace Verse
{
	// Token: 0x0200025A RID: 602
	public class HediffCompProperties_GrowthMode : HediffCompProperties
	{
		// Token: 0x06001076 RID: 4214 RVA: 0x0005E154 File Offset: 0x0005C354
		public HediffCompProperties_GrowthMode()
		{
			this.compClass = typeof(HediffComp_GrowthMode);
		}

		// Token: 0x04000C05 RID: 3077
		public float severityPerDayGrowing;

		// Token: 0x04000C06 RID: 3078
		public float severityPerDayRemission;

		// Token: 0x04000C07 RID: 3079
		public FloatRange severityPerDayGrowingRandomFactor = new FloatRange(1f, 1f);

		// Token: 0x04000C08 RID: 3080
		public FloatRange severityPerDayRemissionRandomFactor = new FloatRange(1f, 1f);
	}
}
