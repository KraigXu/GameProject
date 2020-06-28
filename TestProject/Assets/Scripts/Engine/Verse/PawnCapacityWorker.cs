using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020000CD RID: 205
	public class PawnCapacityWorker
	{
		// Token: 0x060005BA RID: 1466 RVA: 0x0001BFCE File Offset: 0x0001A1CE
		public virtual float CalculateCapacityLevel(HediffSet diffSet, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
		{
			return 1f;
		}

		// Token: 0x060005BB RID: 1467 RVA: 0x0001028D File Offset: 0x0000E48D
		public virtual bool CanHaveCapacity(BodyDef body)
		{
			return true;
		}

		// Token: 0x060005BC RID: 1468 RVA: 0x0001BFD8 File Offset: 0x0001A1D8
		protected float CalculateCapacityAndRecord(HediffSet diffSet, PawnCapacityDef capacity, List<PawnCapacityUtility.CapacityImpactor> impactors)
		{
			float level = diffSet.pawn.health.capacities.GetLevel(capacity);
			if (impactors != null && level != 1f)
			{
				impactors.Add(new PawnCapacityUtility.CapacityImpactorCapacity
				{
					capacity = capacity
				});
			}
			return level;
		}
	}
}
