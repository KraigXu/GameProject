using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000763 RID: 1891
	public class WorkGiver_TendSelfEmergency : WorkGiver_TendSelf
	{
		// Token: 0x0600316D RID: 12653 RVA: 0x0011384C File Offset: 0x00111A4C
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

		// Token: 0x04001B03 RID: 6915
		private static JobGiver_SelfTend jgp = new JobGiver_SelfTend();
	}
}
