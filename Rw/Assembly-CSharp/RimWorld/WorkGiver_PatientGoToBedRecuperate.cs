using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000752 RID: 1874
	public class WorkGiver_PatientGoToBedRecuperate : WorkGiver
	{
		// Token: 0x06003113 RID: 12563 RVA: 0x00112B54 File Offset: 0x00110D54
		public override Job NonScanJob(Pawn pawn)
		{
			ThinkResult thinkResult = WorkGiver_PatientGoToBedRecuperate.jgp.TryIssueJobPackage(pawn, default(JobIssueParams));
			if (thinkResult.IsValid)
			{
				return thinkResult.Job;
			}
			return null;
		}

		// Token: 0x04001B01 RID: 6913
		private static JobGiver_PatientGoToBed jgp = new JobGiver_PatientGoToBed
		{
			respectTimetable = false
		};
	}
}
