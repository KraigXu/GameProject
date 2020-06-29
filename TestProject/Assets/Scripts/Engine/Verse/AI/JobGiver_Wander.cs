using System;
using RimWorld;

namespace Verse.AI
{
	
	public abstract class JobGiver_Wander : ThinkNode_JobGiver
	{
		
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

		
		protected virtual IntVec3 GetExactWanderDest(Pawn pawn)
		{
			IntVec3 wanderRoot = this.GetWanderRoot(pawn);
			return RCellFinder.RandomWanderDestFor(pawn, wanderRoot, this.wanderRadius, this.wanderDestValidator, PawnUtility.ResolveMaxDanger(pawn, this.maxDanger));
		}

		
		protected abstract IntVec3 GetWanderRoot(Pawn pawn);

		
		protected float wanderRadius;

		
		protected Func<Pawn, IntVec3, IntVec3, bool> wanderDestValidator;

		
		protected IntRange ticksBetweenWandersRange = new IntRange(20, 100);

		
		protected LocomotionUrgency locomotionUrgency = LocomotionUrgency.Walk;

		
		protected Danger maxDanger = Danger.None;

		
		protected int expiryInterval = -1;
	}
}
