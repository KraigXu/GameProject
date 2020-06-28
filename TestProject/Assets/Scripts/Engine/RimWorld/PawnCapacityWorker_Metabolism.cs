using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B06 RID: 2822
	public class PawnCapacityWorker_Metabolism : PawnCapacityWorker
	{
		// Token: 0x06004288 RID: 17032 RVA: 0x00163670 File Offset: 0x00161870
		public override float CalculateCapacityLevel(HediffSet diffSet, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
		{
			return PawnCapacityUtility.CalculateTagEfficiency(diffSet, BodyPartTagDefOf.MetabolismSource, float.MaxValue, default(FloatRange), impactors, -1f);
		}

		// Token: 0x06004289 RID: 17033 RVA: 0x0016369C File Offset: 0x0016189C
		public override bool CanHaveCapacity(BodyDef body)
		{
			return body.HasPartWithTag(BodyPartTagDefOf.MetabolismSource);
		}
	}
}
