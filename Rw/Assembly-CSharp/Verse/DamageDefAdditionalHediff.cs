using System;
using RimWorld;

namespace Verse
{
	// Token: 0x0200007F RID: 127
	public class DamageDefAdditionalHediff
	{
		// Token: 0x0400020E RID: 526
		public HediffDef hediff;

		// Token: 0x0400020F RID: 527
		public float severityPerDamageDealt = 0.1f;

		// Token: 0x04000210 RID: 528
		public float severityFixed;

		// Token: 0x04000211 RID: 529
		public StatDef victimSeverityScaling;

		// Token: 0x04000212 RID: 530
		public bool victimSeverityScalingByInvBodySize;
	}
}
