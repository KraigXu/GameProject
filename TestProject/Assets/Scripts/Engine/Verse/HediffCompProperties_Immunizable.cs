using System;

namespace Verse
{
	// Token: 0x02000275 RID: 629
	public class HediffCompProperties_Immunizable : HediffCompProperties
	{
		// Token: 0x060010E1 RID: 4321 RVA: 0x0005FA27 File Offset: 0x0005DC27
		public HediffCompProperties_Immunizable()
		{
			this.compClass = typeof(HediffComp_Immunizable);
		}

		// Token: 0x04000C40 RID: 3136
		public float immunityPerDayNotSick;

		// Token: 0x04000C41 RID: 3137
		public float immunityPerDaySick;

		// Token: 0x04000C42 RID: 3138
		public float severityPerDayNotImmune;

		// Token: 0x04000C43 RID: 3139
		public float severityPerDayImmune;

		// Token: 0x04000C44 RID: 3140
		public FloatRange severityPerDayNotImmuneRandomFactor = new FloatRange(1f, 1f);
	}
}
