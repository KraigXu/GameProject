using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B02 RID: 2818
	public class PawnCapacityWorker_Consciousness : PawnCapacityWorker
	{
		// Token: 0x0600427C RID: 17020 RVA: 0x00163488 File Offset: 0x00161688
		public override float CalculateCapacityLevel(HediffSet diffSet, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
		{
			float num = PawnCapacityUtility.CalculateTagEfficiency(diffSet, BodyPartTagDefOf.ConsciousnessSource, float.MaxValue, default(FloatRange), impactors, -1f);
			float num2 = Mathf.Clamp(GenMath.LerpDouble(0.1f, 1f, 0f, 0.4f, diffSet.PainTotal), 0f, 0.4f);
			if ((double)num2 >= 0.01)
			{
				num -= num2;
				if (impactors != null)
				{
					impactors.Add(new PawnCapacityUtility.CapacityImpactorPain());
				}
			}
			num = Mathf.Lerp(num, num * Mathf.Min(base.CalculateCapacityAndRecord(diffSet, PawnCapacityDefOf.BloodPumping, impactors), 1f), 0.2f);
			num = Mathf.Lerp(num, num * Mathf.Min(base.CalculateCapacityAndRecord(diffSet, PawnCapacityDefOf.Breathing, impactors), 1f), 0.2f);
			return Mathf.Lerp(num, num * Mathf.Min(base.CalculateCapacityAndRecord(diffSet, PawnCapacityDefOf.BloodFiltration, impactors), 1f), 0.1f);
		}

		// Token: 0x0600427D RID: 17021 RVA: 0x00163573 File Offset: 0x00161773
		public override bool CanHaveCapacity(BodyDef body)
		{
			return body.HasPartWithTag(BodyPartTagDefOf.ConsciousnessSource);
		}
	}
}
