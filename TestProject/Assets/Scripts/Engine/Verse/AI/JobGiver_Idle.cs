using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x020005A4 RID: 1444
	public class JobGiver_Idle : ThinkNode_JobGiver
	{
		// Token: 0x0600289C RID: 10396 RVA: 0x000EF4E7 File Offset: 0x000ED6E7
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_Idle jobGiver_Idle = (JobGiver_Idle)base.DeepCopy(resolve);
			jobGiver_Idle.ticks = this.ticks;
			return jobGiver_Idle;
		}

		// Token: 0x0600289D RID: 10397 RVA: 0x000EF501 File Offset: 0x000ED701
		protected override Job TryGiveJob(Pawn pawn)
		{
			Job job = JobMaker.MakeJob(JobDefOf.Wait);
			job.expiryInterval = this.ticks;
			return job;
		}

		// Token: 0x04001861 RID: 6241
		public int ticks = 50;
	}
}
