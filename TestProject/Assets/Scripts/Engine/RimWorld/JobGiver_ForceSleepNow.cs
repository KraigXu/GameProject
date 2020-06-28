using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020006CC RID: 1740
	public class JobGiver_ForceSleepNow : ThinkNode_JobGiver
	{
		// Token: 0x06002EA2 RID: 11938 RVA: 0x00106053 File Offset: 0x00104253
		protected override Job TryGiveJob(Pawn pawn)
		{
			Job job = JobMaker.MakeJob(JobDefOf.LayDown, pawn.Position);
			job.forceSleep = true;
			return job;
		}
	}
}
