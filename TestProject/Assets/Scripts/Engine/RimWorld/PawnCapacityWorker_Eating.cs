using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B03 RID: 2819
	public class PawnCapacityWorker_Eating : PawnCapacityWorker
	{
		// Token: 0x0600427F RID: 17023 RVA: 0x00163580 File Offset: 0x00161780
		public override float CalculateCapacityLevel(HediffSet diffSet, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
		{
			return PawnCapacityUtility.CalculateTagEfficiency(diffSet, BodyPartTagDefOf.EatingSource, float.MaxValue, default(FloatRange), impactors, -1f) * PawnCapacityUtility.CalculateTagEfficiency(diffSet, BodyPartTagDefOf.EatingPathway, 1f, default(FloatRange), impactors, -1f) * base.CalculateCapacityAndRecord(diffSet, PawnCapacityDefOf.Consciousness, impactors);
		}

		// Token: 0x06004280 RID: 17024 RVA: 0x001635DA File Offset: 0x001617DA
		public override bool CanHaveCapacity(BodyDef body)
		{
			return body.HasPartWithTag(BodyPartTagDefOf.EatingSource);
		}
	}
}
