using System;

namespace Verse
{
	// Token: 0x02000287 RID: 647
	public class HediffGiver_RandomDrugEffect : HediffGiver
	{
		// Token: 0x0600113A RID: 4410 RVA: 0x0006138B File Offset: 0x0005F58B
		public override void OnIntervalPassed(Pawn pawn, Hediff cause)
		{
			if (cause.Severity < this.minSeverity)
			{
				return;
			}
			if (Rand.MTBEventOccurs(this.baseMtbDays, 60000f, 60f) && base.TryApply(pawn, null))
			{
				base.SendLetter(pawn, cause);
			}
		}

		// Token: 0x04000C62 RID: 3170
		public float baseMtbDays;

		// Token: 0x04000C63 RID: 3171
		public float minSeverity;
	}
}
