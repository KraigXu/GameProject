using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000521 RID: 1313
	public class JobDriver_Kill : JobDriver
	{
		// Token: 0x0600257C RID: 9596 RVA: 0x000DE503 File Offset: 0x000DC703
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x0600257D RID: 9597 RVA: 0x000DE526 File Offset: 0x000DC726
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.EndOnDespawnedOrNull(TargetIndex.A, JobCondition.Succeeded);
			yield return Toils_Combat.TrySetJobToUseAttackVerb(TargetIndex.A);
			Toil gotoCastPos = Toils_Combat.GotoCastPosition(TargetIndex.A, false, 0.95f);
			yield return gotoCastPos;
			Toil jumpIfCannotHit = Toils_Jump.JumpIfTargetNotHittable(TargetIndex.A, gotoCastPos);
			yield return jumpIfCannotHit;
			yield return Toils_Combat.CastVerb(TargetIndex.A, true);
			yield return Toils_Jump.Jump(jumpIfCannotHit);
			yield break;
		}

		// Token: 0x040016E0 RID: 5856
		private const TargetIndex VictimInd = TargetIndex.A;
	}
}
