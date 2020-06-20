using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000245 RID: 581
	public class HediffCompProperties_ChangeNeed : HediffCompProperties
	{
		// Token: 0x06001033 RID: 4147 RVA: 0x0005D2C7 File Offset: 0x0005B4C7
		public HediffCompProperties_ChangeNeed()
		{
			this.compClass = typeof(HediffComp_ChangeNeed);
		}

		// Token: 0x04000BE2 RID: 3042
		public NeedDef needDef;

		// Token: 0x04000BE3 RID: 3043
		public float percentPerDay;
	}
}
