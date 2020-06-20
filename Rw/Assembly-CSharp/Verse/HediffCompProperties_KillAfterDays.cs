using System;

namespace Verse
{
	// Token: 0x02000263 RID: 611
	public class HediffCompProperties_KillAfterDays : HediffCompProperties
	{
		// Token: 0x06001099 RID: 4249 RVA: 0x0005EB28 File Offset: 0x0005CD28
		public HediffCompProperties_KillAfterDays()
		{
			this.compClass = typeof(HediffComp_KillAfterDays);
		}

		// Token: 0x04000C1C RID: 3100
		public int days;
	}
}
