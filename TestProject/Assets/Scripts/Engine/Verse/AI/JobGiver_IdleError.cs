using System;
using RimWorld;

namespace Verse.AI
{
	
	public class JobGiver_IdleError : ThinkNode_JobGiver
	{
		
		protected override Job TryGiveJob(Pawn pawn)
		{
			Log.ErrorOnce(pawn + " issued IdleError wait job. The behavior tree should never get here.", 532983, false);
			Job job = JobMaker.MakeJob(JobDefOf.Wait);
			job.expiryInterval = 100;
			return job;
		}

		
		private const int WaitTime = 100;
	}
}
