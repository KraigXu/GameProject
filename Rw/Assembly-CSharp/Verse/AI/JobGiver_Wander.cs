using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x020005AE RID: 1454
	public abstract class JobGiver_Wander : ThinkNode_JobGiver
	{
		// Token: 0x060028C7 RID: 10439 RVA: 0x000EFAC0 File Offset: 0x000EDCC0
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_Wander jobGiver_Wander = (JobGiver_Wander)base.DeepCopy(resolve);
			jobGiver_Wander.wanderRadius = this.wanderRadius;
			jobGiver_Wander.wanderDestValidator = this.wanderDestValidator;
			jobGiver_Wander.ticksBetweenWandersRange = this.ticksBetweenWandersRange;
			jobGiver_Wander.locomotionUrgency = this.locomotionUrgency;
			jobGiver_Wander.maxDanger = this.maxDanger;
			jobGiver_Wander.expiryInterval = this.expiryInterval;
			return jobGiver_Wander;
		}

		// Token: 0x060028C8 RID: 10440 RVA: 0x000EFB24 File Offset: 0x000EDD24
		protected override Job TryGiveJob(Pawn pawn)
		{
			bool flag = pawn.CurJob != null && pawn.CurJob.def == JobDefOf.GotoWander;
			bool nextMoveOrderIsWait = pawn.mindState.nextMoveOrderIsWait;
			if (!flag)
			{
				pawn.mindState.nextMoveOrderIsWait = !pawn.mindState.nextMoveOrderIsWait;
			}
			if (nextMoveOrderIsWait && !flag)
			{
				Job job = JobMaker.MakeJob(JobDefOf.Wait_Wander);
				job.expiryInterval = this.ticksBetweenWandersRange.RandomInRange;
				return job;
			}
			IntVec3 exactWanderDest = this.GetExactWanderDest(pawn);
			if (!exactWanderDest.IsValid)
			{
				pawn.mindState.nextMoveOrderIsWait = false;
				return null;
			}
			Job job2 = JobMaker.MakeJob(JobDefOf.GotoWander, exactWanderDest);
			job2.locomotionUrgency = this.locomotionUrgency;
			job2.expiryInterval = this.expiryInterval;
			job2.checkOverrideOnExpire = true;
			return job2;
		}

		// Token: 0x060028C9 RID: 10441 RVA: 0x000EFBE8 File Offset: 0x000EDDE8
		protected virtual IntVec3 GetExactWanderDest(Pawn pawn)
		{
			IntVec3 wanderRoot = this.GetWanderRoot(pawn);
			return RCellFinder.RandomWanderDestFor(pawn, wanderRoot, this.wanderRadius, this.wanderDestValidator, PawnUtility.ResolveMaxDanger(pawn, this.maxDanger));
		}

		// Token: 0x060028CA RID: 10442
		protected abstract IntVec3 GetWanderRoot(Pawn pawn);

		// Token: 0x04001870 RID: 6256
		protected float wanderRadius;

		// Token: 0x04001871 RID: 6257
		protected Func<Pawn, IntVec3, IntVec3, bool> wanderDestValidator;

		// Token: 0x04001872 RID: 6258
		protected IntRange ticksBetweenWandersRange = new IntRange(20, 100);

		// Token: 0x04001873 RID: 6259
		protected LocomotionUrgency locomotionUrgency = LocomotionUrgency.Walk;

		// Token: 0x04001874 RID: 6260
		protected Danger maxDanger = Danger.None;

		// Token: 0x04001875 RID: 6261
		protected int expiryInterval = -1;
	}
}
