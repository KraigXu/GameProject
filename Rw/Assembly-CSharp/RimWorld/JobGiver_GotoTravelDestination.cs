using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020006CD RID: 1741
	public class JobGiver_GotoTravelDestination : ThinkNode_JobGiver
	{
		// Token: 0x06002EA4 RID: 11940 RVA: 0x00106071 File Offset: 0x00104271
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_GotoTravelDestination jobGiver_GotoTravelDestination = (JobGiver_GotoTravelDestination)base.DeepCopy(resolve);
			jobGiver_GotoTravelDestination.locomotionUrgency = this.locomotionUrgency;
			jobGiver_GotoTravelDestination.maxDanger = this.maxDanger;
			jobGiver_GotoTravelDestination.jobMaxDuration = this.jobMaxDuration;
			jobGiver_GotoTravelDestination.exactCell = this.exactCell;
			return jobGiver_GotoTravelDestination;
		}

		// Token: 0x06002EA5 RID: 11941 RVA: 0x001060B0 File Offset: 0x001042B0
		protected override Job TryGiveJob(Pawn pawn)
		{
			pawn.mindState.nextMoveOrderIsWait = !pawn.mindState.nextMoveOrderIsWait;
			if (pawn.mindState.nextMoveOrderIsWait && !this.exactCell)
			{
				Job job = JobMaker.MakeJob(JobDefOf.Wait_Wander);
				job.expiryInterval = this.WaitTicks.RandomInRange;
				return job;
			}
			IntVec3 cell = pawn.mindState.duty.focus.Cell;
			if (!pawn.CanReach(cell, PathEndMode.OnCell, PawnUtility.ResolveMaxDanger(pawn, this.maxDanger), false, TraverseMode.ByPawn))
			{
				return null;
			}
			if (this.exactCell && pawn.Position == cell)
			{
				return null;
			}
			IntVec3 c = cell;
			if (!this.exactCell)
			{
				c = CellFinder.RandomClosewalkCellNear(cell, pawn.Map, 6, null);
			}
			Job job2 = JobMaker.MakeJob(JobDefOf.Goto, c);
			job2.locomotionUrgency = PawnUtility.ResolveLocomotion(pawn, this.locomotionUrgency);
			job2.expiryInterval = this.jobMaxDuration;
			return job2;
		}

		// Token: 0x04001A76 RID: 6774
		private LocomotionUrgency locomotionUrgency = LocomotionUrgency.Walk;

		// Token: 0x04001A77 RID: 6775
		private Danger maxDanger = Danger.Some;

		// Token: 0x04001A78 RID: 6776
		private int jobMaxDuration = 999999;

		// Token: 0x04001A79 RID: 6777
		private bool exactCell;

		// Token: 0x04001A7A RID: 6778
		private IntRange WaitTicks = new IntRange(30, 80);
	}
}
