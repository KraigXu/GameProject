using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F05 RID: 3845
	public class TransferableComparer_HitPointsPercentage : TransferableComparer
	{
		// Token: 0x06005E43 RID: 24131 RVA: 0x0020A6B8 File Offset: 0x002088B8
		public override int Compare(Transferable lhs, Transferable rhs)
		{
			return this.GetValueFor(lhs).CompareTo(this.GetValueFor(rhs));
		}

		// Token: 0x06005E44 RID: 24132 RVA: 0x0020A6DC File Offset: 0x002088DC
		private float GetValueFor(Transferable t)
		{
			Thing anyThing = t.AnyThing;
			Pawn pawn = anyThing as Pawn;
			if (pawn != null)
			{
				return pawn.health.summaryHealth.SummaryHealthPercent;
			}
			if (!anyThing.def.useHitPoints || !anyThing.def.healthAffectsPrice)
			{
				return 1f;
			}
			return (float)anyThing.HitPoints / (float)anyThing.MaxHitPoints;
		}
	}
}
