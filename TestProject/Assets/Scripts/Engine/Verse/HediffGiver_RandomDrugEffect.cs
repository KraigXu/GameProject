using System;

namespace Verse
{
	
	public class HediffGiver_RandomDrugEffect : HediffGiver
	{
		
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

		
		public float baseMtbDays;

		
		public float minSeverity;
	}
}
