using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	
	public class JobGiver_StandAndBeSociallyActive : ThinkNode_JobGiver
	{
		
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_StandAndBeSociallyActive jobGiver_StandAndBeSociallyActive = (JobGiver_StandAndBeSociallyActive)base.DeepCopy(resolve);
			jobGiver_StandAndBeSociallyActive.ticksRange = this.ticksRange;
			return jobGiver_StandAndBeSociallyActive;
		}

		
		protected override Job TryGiveJob(Pawn pawn)
		{
			Job job = JobMaker.MakeJob(JobDefOf.StandAndBeSociallyActive);
			job.expiryInterval = this.ticksRange.RandomInRange;
			return job;
		}

		
		public IntRange ticksRange = new IntRange(300, 600);
	}
}
