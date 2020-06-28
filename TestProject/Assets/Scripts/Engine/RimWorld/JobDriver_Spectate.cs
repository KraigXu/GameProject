using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200063F RID: 1599
	public class JobDriver_Spectate : JobDriver
	{
		// Token: 0x06002BC6 RID: 11206 RVA: 0x000DE503 File Offset: 0x000DC703
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x06002BC7 RID: 11207 RVA: 0x000FBB8B File Offset: 0x000F9D8B
		protected override IEnumerable<Toil> MakeNewToils()
		{
			if (this.job.GetTarget(TargetIndex.A).HasThing)
			{
				this.EndOnDespawnedOrNull(TargetIndex.A, JobCondition.Incompletable);
			}
			yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
			yield return new Toil
			{
				tickAction = delegate
				{
					this.pawn.rotationTracker.FaceCell(this.job.GetTarget(TargetIndex.B).Cell);
					this.pawn.GainComfortFromCellIfPossible(false);
					if (this.pawn.IsHashIntervalTick(100))
					{
						this.pawn.jobs.CheckForJobOverride();
					}
				},
				defaultCompleteMode = ToilCompleteMode.Never,
				handlingFacing = true
			};
			yield break;
		}

		// Token: 0x040019B1 RID: 6577
		private const TargetIndex MySpotOrChairInd = TargetIndex.A;

		// Token: 0x040019B2 RID: 6578
		private const TargetIndex WatchTargetInd = TargetIndex.B;
	}
}
