using System;

namespace Verse
{
	// Token: 0x02000281 RID: 641
	public class HediffGiver_Bleeding : HediffGiver
	{
		// Token: 0x0600112D RID: 4397 RVA: 0x00060DC8 File Offset: 0x0005EFC8
		public override void OnIntervalPassed(Pawn pawn, Hediff cause)
		{
			HediffSet hediffSet = pawn.health.hediffSet;
			if (hediffSet.BleedRateTotal >= 0.1f)
			{
				HealthUtility.AdjustSeverity(pawn, this.hediff, hediffSet.BleedRateTotal * 0.001f);
				return;
			}
			HealthUtility.AdjustSeverity(pawn, this.hediff, -0.00033333333f);
		}
	}
}
