using System;

namespace Verse
{
	// Token: 0x02000289 RID: 649
	public class HediffGiver_Refresh : HediffGiver
	{
		// Token: 0x0600113E RID: 4414 RVA: 0x00061448 File Offset: 0x0005F648
		public override void OnIntervalPassed(Pawn pawn, Hediff cause)
		{
			Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(this.hediff, false);
			if (firstHediffOfDef != null)
			{
				firstHediffOfDef.ageTicks = 0;
				return;
			}
			if (base.TryApply(pawn, null))
			{
				base.SendLetter(pawn, cause);
			}
		}
	}
}
