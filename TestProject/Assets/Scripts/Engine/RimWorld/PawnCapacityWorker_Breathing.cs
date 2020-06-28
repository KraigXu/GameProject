using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B01 RID: 2817
	public class PawnCapacityWorker_Breathing : PawnCapacityWorker
	{
		// Token: 0x06004279 RID: 17017 RVA: 0x0016340C File Offset: 0x0016160C
		public override float CalculateCapacityLevel(HediffSet diffSet, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
		{
			return PawnCapacityUtility.CalculateTagEfficiency(diffSet, BodyPartTagDefOf.BreathingSource, float.MaxValue, default(FloatRange), impactors, -1f) * PawnCapacityUtility.CalculateTagEfficiency(diffSet, BodyPartTagDefOf.BreathingPathway, 1f, default(FloatRange), impactors, -1f) * PawnCapacityUtility.CalculateTagEfficiency(diffSet, BodyPartTagDefOf.BreathingSourceCage, 1f, default(FloatRange), impactors, -1f);
		}

		// Token: 0x0600427A RID: 17018 RVA: 0x00163478 File Offset: 0x00161678
		public override bool CanHaveCapacity(BodyDef body)
		{
			return body.HasPartWithTag(BodyPartTagDefOf.BreathingSource);
		}
	}
}
