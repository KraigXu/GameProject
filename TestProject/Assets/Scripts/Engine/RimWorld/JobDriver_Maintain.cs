using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000668 RID: 1640
	public class JobDriver_Maintain : JobDriver
	{
		// Token: 0x06002CC0 RID: 11456 RVA: 0x000DDBC6 File Offset: 0x000DBDC6
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x06002CC1 RID: 11457 RVA: 0x000FE6CF File Offset: 0x000FC8CF
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			Toil toil = Toils_General.Wait(180, TargetIndex.None);
			toil.WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
			toil.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			toil.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			yield return toil;
			Toil maintain = new Toil();
			maintain.initAction = delegate
			{
				maintain.actor.CurJob.targetA.Thing.TryGetComp<CompMaintainable>().Maintained();
			};
			maintain.defaultCompleteMode = ToilCompleteMode.Instant;
			yield return maintain;
			yield break;
		}

		// Token: 0x040019F4 RID: 6644
		private const int MaintainTicks = 180;
	}
}
