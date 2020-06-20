using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020006E5 RID: 1765
	public class JobGiver_PatientGoToBed : ThinkNode
	{
		// Token: 0x06002EF1 RID: 12017 RVA: 0x0010819C File Offset: 0x0010639C
		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			if (!HealthAIUtility.ShouldSeekMedicalRest(pawn))
			{
				return ThinkResult.NoJob;
			}
			if (this.respectTimetable && RestUtility.TimetablePreventsLayDown(pawn) && !HealthAIUtility.ShouldHaveSurgeryDoneNow(pawn) && !HealthAIUtility.ShouldBeTendedNowByPlayer(pawn))
			{
				return ThinkResult.NoJob;
			}
			if (RestUtility.DisturbancePreventsLyingDown(pawn))
			{
				return ThinkResult.NoJob;
			}
			Thing thing = RestUtility.FindPatientBedFor(pawn);
			if (thing == null)
			{
				return ThinkResult.NoJob;
			}
			return new ThinkResult(JobMaker.MakeJob(JobDefOf.LayDown, thing), this, null, false);
		}

		// Token: 0x04001AA2 RID: 6818
		public bool respectTimetable = true;
	}
}
