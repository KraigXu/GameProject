using System;

namespace Verse
{
	// Token: 0x02000277 RID: 631
	public class HediffCompProperties_SeverityPerDay : HediffCompProperties
	{
		// Token: 0x060010EE RID: 4334 RVA: 0x0005FC95 File Offset: 0x0005DE95
		public HediffCompProperties_SeverityPerDay()
		{
			this.compClass = typeof(HediffComp_SeverityPerDay);
		}

		// Token: 0x04000C47 RID: 3143
		public float severityPerDay;

		// Token: 0x04000C48 RID: 3144
		public bool showDaysToRecover;

		// Token: 0x04000C49 RID: 3145
		public bool showHoursToRecover;
	}
}
