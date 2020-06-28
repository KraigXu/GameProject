using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D80 RID: 3456
	public class CompProperties_Scanner : CompProperties
	{
		// Token: 0x06005442 RID: 21570 RVA: 0x001C2290 File Offset: 0x001C0490
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			if (this.scanFindMtbDays <= 0f)
			{
				yield return "scanFindMtbDays not set";
			}
			yield break;
		}

		// Token: 0x04002E6B RID: 11883
		public float scanFindMtbDays;

		// Token: 0x04002E6C RID: 11884
		public float scanFindGuaranteedDays = -1f;

		// Token: 0x04002E6D RID: 11885
		public StatDef scanSpeedStat;
	}
}
