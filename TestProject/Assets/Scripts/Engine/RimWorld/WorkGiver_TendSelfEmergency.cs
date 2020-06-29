using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	
	public class WorkGiver_TendSelfEmergency : WorkGiver_TendSelf
	{
		
		public override Job NonScanJob(Pawn pawn)
		{
			if (!this.HasJobOnThing(pawn, pawn, false) || !HealthAIUtility.ShouldBeTendedNowByPlayerUrgent(pawn))
			{
				return null;
			}
			ThinkResult thinkResult = WorkGiver_TendSelfEmergency.jgp.TryIssueJobPackage(pawn, default(JobIssueParams));
			if (thinkResult.IsValid)
			{
				return thinkResult.Job;
			}
			return null;
		}

		
		private static JobGiver_SelfTend jgp = new JobGiver_SelfTend();
	}
}
