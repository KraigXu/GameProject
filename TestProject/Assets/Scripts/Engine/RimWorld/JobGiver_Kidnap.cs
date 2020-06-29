using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	
	public class JobGiver_Kidnap : ThinkNode_JobGiver
	{
		
		protected override Job TryGiveJob(Pawn pawn)
		{
			IntVec3 c;
			if (!RCellFinder.TryFindBestExitSpot(pawn, out c, TraverseMode.ByPawn))
			{
				return null;
			}
			Pawn t;
			if (KidnapAIUtility.TryFindGoodKidnapVictim(pawn, 18f, out t, null) && !GenAI.InDangerousCombat(pawn))
			{
				Job job = JobMaker.MakeJob(JobDefOf.Kidnap);
				job.targetA = t;
				job.targetB = c;
				job.count = 1;
				return job;
			}
			return null;
		}

		
		public const float VictimSearchRadiusInitial = 8f;

		
		private const float VictimSearchRadiusOngoing = 18f;
	}
}
