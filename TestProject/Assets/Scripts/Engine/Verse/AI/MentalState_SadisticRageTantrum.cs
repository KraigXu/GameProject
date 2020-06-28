using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x0200055B RID: 1371
	public class MentalState_SadisticRageTantrum : MentalState_TantrumRandom
	{
		// Token: 0x06002703 RID: 9987 RVA: 0x000E4B30 File Offset: 0x000E2D30
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.hits, "hits", 0, false);
		}

		// Token: 0x06002704 RID: 9988 RVA: 0x000E4B4A File Offset: 0x000E2D4A
		protected override void GetPotentialTargets(List<Thing> outThings)
		{
			TantrumMentalStateUtility.GetSmashableThingsNear(this.pawn, this.pawn.Position, outThings, this.GetCustomValidator(), 0, 40);
		}

		// Token: 0x06002705 RID: 9989 RVA: 0x000E4B6C File Offset: 0x000E2D6C
		protected override Predicate<Thing> GetCustomValidator()
		{
			return (Thing x) => TantrumMentalStateUtility.CanAttackPrisoner(this.pawn, x);
		}

		// Token: 0x06002706 RID: 9990 RVA: 0x000E4B7A File Offset: 0x000E2D7A
		public override void Notify_AttackedTarget(LocalTargetInfo hitTarget)
		{
			base.Notify_AttackedTarget(hitTarget);
			if (this.target != null && hitTarget.Thing == this.target)
			{
				this.hits++;
				if (this.hits >= 7)
				{
					base.RecoverFromState();
				}
			}
		}

		// Token: 0x04001750 RID: 5968
		private int hits;

		// Token: 0x04001751 RID: 5969
		public const int MaxHits = 7;
	}
}
