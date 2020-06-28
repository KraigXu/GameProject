﻿using System;

namespace Verse.AI
{
	// Token: 0x020005AD RID: 1453
	public class ThinkNode_QueuedJob : ThinkNode
	{
		// Token: 0x060028C4 RID: 10436 RVA: 0x000EF9A7 File Offset: 0x000EDBA7
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_QueuedJob thinkNode_QueuedJob = (ThinkNode_QueuedJob)base.DeepCopy(resolve);
			thinkNode_QueuedJob.inBedOnly = this.inBedOnly;
			return thinkNode_QueuedJob;
		}

		// Token: 0x060028C5 RID: 10437 RVA: 0x000EF9C4 File Offset: 0x000EDBC4
		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			JobQueue jobQueue = pawn.jobs.jobQueue;
			if (pawn.Downed || jobQueue.AnyCanBeginNow(pawn, this.inBedOnly))
			{
				while (jobQueue.Count > 0 && !jobQueue.Peek().job.CanBeginNow(pawn, this.inBedOnly))
				{
					QueuedJob queuedJob = jobQueue.Dequeue();
					pawn.ClearReservationsForJob(queuedJob.job);
					if (pawn.jobs.debugLog)
					{
						pawn.jobs.DebugLogEvent("   Throwing away queued job that I cannot begin now: " + queuedJob.job);
					}
				}
			}
			if (jobQueue.Count > 0 && jobQueue.Peek().job.CanBeginNow(pawn, this.inBedOnly))
			{
				QueuedJob queuedJob2 = jobQueue.Dequeue();
				if (pawn.jobs.debugLog)
				{
					pawn.jobs.DebugLogEvent("   Returning queued job: " + queuedJob2.job);
				}
				return new ThinkResult(queuedJob2.job, this, queuedJob2.tag, true);
			}
			return ThinkResult.NoJob;
		}

		// Token: 0x0400186F RID: 6255
		public bool inBedOnly;
	}
}
