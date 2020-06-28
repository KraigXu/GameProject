using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x0200059E RID: 1438
	public abstract class JobGiver_ExitMap : ThinkNode_JobGiver
	{
		// Token: 0x0600288F RID: 10383 RVA: 0x000EF220 File Offset: 0x000ED420
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_ExitMap jobGiver_ExitMap = (JobGiver_ExitMap)base.DeepCopy(resolve);
			jobGiver_ExitMap.defaultLocomotion = this.defaultLocomotion;
			jobGiver_ExitMap.jobMaxDuration = this.jobMaxDuration;
			jobGiver_ExitMap.canBash = this.canBash;
			jobGiver_ExitMap.forceCanDig = this.forceCanDig;
			jobGiver_ExitMap.forceCanDigIfAnyHostileActiveThreat = this.forceCanDigIfAnyHostileActiveThreat;
			jobGiver_ExitMap.forceCanDigIfCantReachMapEdge = this.forceCanDigIfCantReachMapEdge;
			jobGiver_ExitMap.failIfCantJoinOrCreateCaravan = this.failIfCantJoinOrCreateCaravan;
			return jobGiver_ExitMap;
		}

		// Token: 0x06002890 RID: 10384 RVA: 0x000EF290 File Offset: 0x000ED490
		protected override Job TryGiveJob(Pawn pawn)
		{
			bool flag = this.forceCanDig || (pawn.mindState.duty != null && pawn.mindState.duty.canDig && !pawn.CanReachMapEdge()) || (this.forceCanDigIfCantReachMapEdge && !pawn.CanReachMapEdge()) || (this.forceCanDigIfAnyHostileActiveThreat && pawn.Faction != null && GenHostility.AnyHostileActiveThreatTo(pawn.Map, pawn.Faction, true));
			IntVec3 c;
			if (!this.TryFindGoodExitDest(pawn, flag, out c))
			{
				return null;
			}
			if (flag)
			{
				using (PawnPath pawnPath = pawn.Map.pathFinder.FindPath(pawn.Position, c, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.PassAllDestroyableThings, false), PathEndMode.OnCell))
				{
					IntVec3 cellBeforeBlocker;
					Thing thing = pawnPath.FirstBlockingBuilding(out cellBeforeBlocker, pawn);
					if (thing != null)
					{
						Job job = DigUtility.PassBlockerJob(pawn, thing, cellBeforeBlocker, true, true);
						if (job != null)
						{
							return job;
						}
					}
				}
			}
			Job job2 = JobMaker.MakeJob(JobDefOf.Goto, c);
			job2.exitMapOnArrival = true;
			job2.failIfCantJoinOrCreateCaravan = this.failIfCantJoinOrCreateCaravan;
			job2.locomotionUrgency = PawnUtility.ResolveLocomotion(pawn, this.defaultLocomotion, LocomotionUrgency.Jog);
			job2.expiryInterval = this.jobMaxDuration;
			job2.canBash = this.canBash;
			return job2;
		}

		// Token: 0x06002891 RID: 10385
		protected abstract bool TryFindGoodExitDest(Pawn pawn, bool canDig, out IntVec3 dest);

		// Token: 0x0400185A RID: 6234
		protected LocomotionUrgency defaultLocomotion;

		// Token: 0x0400185B RID: 6235
		protected int jobMaxDuration = 999999;

		// Token: 0x0400185C RID: 6236
		protected bool canBash;

		// Token: 0x0400185D RID: 6237
		protected bool forceCanDig;

		// Token: 0x0400185E RID: 6238
		protected bool forceCanDigIfAnyHostileActiveThreat;

		// Token: 0x0400185F RID: 6239
		protected bool forceCanDigIfCantReachMapEdge;

		// Token: 0x04001860 RID: 6240
		protected bool failIfCantJoinOrCreateCaravan;
	}
}
