using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AFF RID: 2815
	public class PawnCapacityWorker_BloodFiltration : PawnCapacityWorker
	{
		// Token: 0x06004273 RID: 17011 RVA: 0x00163314 File Offset: 0x00161514
		public override float CalculateCapacityLevel(HediffSet diffSet, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
		{
			if (diffSet.pawn.RaceProps.body.HasPartWithTag(BodyPartTagDefOf.BloodFiltrationKidney))
			{
				return PawnCapacityUtility.CalculateTagEfficiency(diffSet, BodyPartTagDefOf.BloodFiltrationKidney, float.MaxValue, default(FloatRange), impactors, -1f) * PawnCapacityUtility.CalculateTagEfficiency(diffSet, BodyPartTagDefOf.BloodFiltrationLiver, float.MaxValue, default(FloatRange), impactors, -1f);
			}
			return PawnCapacityUtility.CalculateTagEfficiency(diffSet, BodyPartTagDefOf.BloodFiltrationSource, float.MaxValue, default(FloatRange), impactors, -1f);
		}

		// Token: 0x06004274 RID: 17012 RVA: 0x0016339C File Offset: 0x0016159C
		public override bool CanHaveCapacity(BodyDef body)
		{
			return (body.HasPartWithTag(BodyPartTagDefOf.BloodFiltrationKidney) && body.HasPartWithTag(BodyPartTagDefOf.BloodFiltrationLiver)) || body.HasPartWithTag(BodyPartTagDefOf.BloodFiltrationSource);
		}
	}
}
