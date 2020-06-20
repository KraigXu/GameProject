using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B08 RID: 2824
	public class PawnCapacityWorker_Sight : PawnCapacityWorker
	{
		// Token: 0x0600428E RID: 17038 RVA: 0x00163790 File Offset: 0x00161990
		public override float CalculateCapacityLevel(HediffSet diffSet, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
		{
			return PawnCapacityUtility.CalculateTagEfficiency(diffSet, BodyPartTagDefOf.SightSource, float.MaxValue, default(FloatRange), impactors, 0.75f);
		}

		// Token: 0x0600428F RID: 17039 RVA: 0x001637BC File Offset: 0x001619BC
		public override bool CanHaveCapacity(BodyDef body)
		{
			return body.HasPartWithTag(BodyPartTagDefOf.SightSource);
		}
	}
}
