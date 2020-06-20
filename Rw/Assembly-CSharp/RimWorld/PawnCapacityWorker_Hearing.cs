using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B04 RID: 2820
	public class PawnCapacityWorker_Hearing : PawnCapacityWorker
	{
		// Token: 0x06004282 RID: 17026 RVA: 0x001635E8 File Offset: 0x001617E8
		public override float CalculateCapacityLevel(HediffSet diffSet, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
		{
			return PawnCapacityUtility.CalculateTagEfficiency(diffSet, BodyPartTagDefOf.HearingSource, float.MaxValue, default(FloatRange), impactors, 0.75f);
		}

		// Token: 0x06004283 RID: 17027 RVA: 0x00163614 File Offset: 0x00161814
		public override bool CanHaveCapacity(BodyDef body)
		{
			return body.HasPartWithTag(BodyPartTagDefOf.HearingSource);
		}
	}
}
