using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B00 RID: 2816
	public class PawnCapacityWorker_BloodPumping : PawnCapacityWorker
	{
		// Token: 0x06004276 RID: 17014 RVA: 0x001633D0 File Offset: 0x001615D0
		public override float CalculateCapacityLevel(HediffSet diffSet, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
		{
			return PawnCapacityUtility.CalculateTagEfficiency(diffSet, BodyPartTagDefOf.BloodPumpingSource, float.MaxValue, default(FloatRange), impactors, -1f);
		}

		// Token: 0x06004277 RID: 17015 RVA: 0x001633FC File Offset: 0x001615FC
		public override bool CanHaveCapacity(BodyDef body)
		{
			return body.HasPartWithTag(BodyPartTagDefOf.BloodPumpingSource);
		}
	}
}
