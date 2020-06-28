using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020006CE RID: 1742
	public class JobGiver_Kidnap : ThinkNode_JobGiver
	{
		// Token: 0x06002EA7 RID: 11943 RVA: 0x001061CC File Offset: 0x001043CC
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

		// Token: 0x04001A7B RID: 6779
		public const float VictimSearchRadiusInitial = 8f;

		// Token: 0x04001A7C RID: 6780
		private const float VictimSearchRadiusOngoing = 18f;
	}
}
