using System;

namespace Verse.AI
{
	// Token: 0x0200058C RID: 1420
	public abstract class ThinkNode_Conditional : ThinkNode_Priority
	{
		// Token: 0x0600285A RID: 10330 RVA: 0x000EE97B File Offset: 0x000ECB7B
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_Conditional thinkNode_Conditional = (ThinkNode_Conditional)base.DeepCopy(resolve);
			thinkNode_Conditional.invert = this.invert;
			return thinkNode_Conditional;
		}

		// Token: 0x0600285B RID: 10331 RVA: 0x000EE995 File Offset: 0x000ECB95
		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			if (this.Satisfied(pawn) == !this.invert)
			{
				return base.TryIssueJobPackage(pawn, jobParams);
			}
			return ThinkResult.NoJob;
		}

		// Token: 0x0600285C RID: 10332
		protected abstract bool Satisfied(Pawn pawn);

		// Token: 0x0400183E RID: 6206
		public bool invert;
	}
}
