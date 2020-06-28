using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x020005A3 RID: 1443
	public class JobGiver_ForcedGoto : ThinkNode_JobGiver
	{
		// Token: 0x0600289A RID: 10394 RVA: 0x000EF480 File Offset: 0x000ED680
		protected override Job TryGiveJob(Pawn pawn)
		{
			IntVec3 forcedGotoPosition = pawn.mindState.forcedGotoPosition;
			if (!forcedGotoPosition.IsValid)
			{
				return null;
			}
			if (!pawn.CanReach(forcedGotoPosition, PathEndMode.ClosestTouch, Danger.Deadly, false, TraverseMode.ByPawn))
			{
				pawn.mindState.forcedGotoPosition = IntVec3.Invalid;
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.Goto, forcedGotoPosition);
			job.locomotionUrgency = LocomotionUrgency.Walk;
			return job;
		}
	}
}
