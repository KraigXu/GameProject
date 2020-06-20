using System;

namespace Verse.AI
{
	// Token: 0x020005AC RID: 1452
	public abstract class ThinkNode_JobGiver : ThinkNode
	{
		// Token: 0x060028C1 RID: 10433
		protected abstract Job TryGiveJob(Pawn pawn);

		// Token: 0x060028C2 RID: 10434 RVA: 0x000EF978 File Offset: 0x000EDB78
		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			Job job = this.TryGiveJob(pawn);
			if (job == null)
			{
				return ThinkResult.NoJob;
			}
			return new ThinkResult(job, this, null, false);
		}
	}
}
