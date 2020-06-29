using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	
	public class JobGiver_TakeWoundedGuest : ThinkNode_JobGiver
	{
		
		protected override Job TryGiveJob(Pawn pawn)
		{
			IntVec3 c;
			if (!RCellFinder.TryFindBestExitSpot(pawn, out c, TraverseMode.ByPawn))
			{
				return null;
			}
			Pawn pawn2 = KidnapAIUtility.ReachableWoundedGuest(pawn);
			if (pawn2 == null)
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.Kidnap);
			job.targetA = pawn2;
			job.targetB = c;
			job.count = 1;
			return job;
		}
	}
}
