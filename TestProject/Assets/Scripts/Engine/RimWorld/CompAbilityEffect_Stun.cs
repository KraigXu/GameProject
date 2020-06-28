using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AD6 RID: 2774
	public class CompAbilityEffect_Stun : CompAbilityEffect_WithDuration
	{
		// Token: 0x060041A5 RID: 16805 RVA: 0x0015F030 File Offset: 0x0015D230
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			if (target.HasThing)
			{
				base.Apply(target, dest);
				Pawn pawn = target.Thing as Pawn;
				if (pawn != null)
				{
					pawn.stances.stunner.StunFor(base.GetDurationSeconds(pawn).SecondsToTicks(), this.parent.pawn, false);
				}
			}
		}
	}
}
