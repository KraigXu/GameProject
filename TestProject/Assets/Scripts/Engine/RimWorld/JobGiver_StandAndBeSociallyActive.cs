using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020006C8 RID: 1736
	public class JobGiver_StandAndBeSociallyActive : ThinkNode_JobGiver
	{
		// Token: 0x06002E95 RID: 11925 RVA: 0x00105DA7 File Offset: 0x00103FA7
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_StandAndBeSociallyActive jobGiver_StandAndBeSociallyActive = (JobGiver_StandAndBeSociallyActive)base.DeepCopy(resolve);
			jobGiver_StandAndBeSociallyActive.ticksRange = this.ticksRange;
			return jobGiver_StandAndBeSociallyActive;
		}

		// Token: 0x06002E96 RID: 11926 RVA: 0x00105DC1 File Offset: 0x00103FC1
		protected override Job TryGiveJob(Pawn pawn)
		{
			Job job = JobMaker.MakeJob(JobDefOf.StandAndBeSociallyActive);
			job.expiryInterval = this.ticksRange.RandomInRange;
			return job;
		}

		// Token: 0x04001A73 RID: 6771
		public IntRange ticksRange = new IntRange(300, 600);
	}
}
