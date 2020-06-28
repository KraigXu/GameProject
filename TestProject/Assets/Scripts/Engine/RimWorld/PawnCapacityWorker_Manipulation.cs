using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B05 RID: 2821
	public class PawnCapacityWorker_Manipulation : PawnCapacityWorker
	{
		// Token: 0x06004285 RID: 17029 RVA: 0x00163624 File Offset: 0x00161824
		public override float CalculateCapacityLevel(HediffSet diffSet, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
		{
			float num = 0f;
			return PawnCapacityUtility.CalculateLimbEfficiency(diffSet, BodyPartTagDefOf.ManipulationLimbCore, BodyPartTagDefOf.ManipulationLimbSegment, BodyPartTagDefOf.ManipulationLimbDigit, 0.8f, out num, impactors) * base.CalculateCapacityAndRecord(diffSet, PawnCapacityDefOf.Consciousness, impactors);
		}

		// Token: 0x06004286 RID: 17030 RVA: 0x00163662 File Offset: 0x00161862
		public override bool CanHaveCapacity(BodyDef body)
		{
			return body.HasPartWithTag(BodyPartTagDefOf.ManipulationLimbCore);
		}
	}
}
