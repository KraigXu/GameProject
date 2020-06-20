using System;

namespace Verse.AI
{
	// Token: 0x0200059B RID: 1435
	public class ThinkNode_Tagger : ThinkNode_Priority
	{
		// Token: 0x06002883 RID: 10371 RVA: 0x000EEFB9 File Offset: 0x000ED1B9
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_Tagger thinkNode_Tagger = (ThinkNode_Tagger)base.DeepCopy(resolve);
			thinkNode_Tagger.tagToGive = this.tagToGive;
			return thinkNode_Tagger;
		}

		// Token: 0x06002884 RID: 10372 RVA: 0x000EEFD4 File Offset: 0x000ED1D4
		public override float GetPriority(Pawn pawn)
		{
			if (this.priority >= 0f)
			{
				return this.priority;
			}
			if (this.subNodes.Any<ThinkNode>())
			{
				return this.subNodes[0].GetPriority(pawn);
			}
			Log.ErrorOnce("ThinkNode_PrioritySorter has child node which didn't give a priority: " + this, this.GetHashCode(), false);
			return 0f;
		}

		// Token: 0x06002885 RID: 10373 RVA: 0x000EF034 File Offset: 0x000ED234
		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			ThinkResult result = base.TryIssueJobPackage(pawn, jobParams);
			if (result.IsValid && result.Tag == null)
			{
				result = new ThinkResult(result.Job, result.SourceNode, new JobTag?(this.tagToGive), false);
			}
			return result;
		}

		// Token: 0x04001855 RID: 6229
		private JobTag tagToGive;
	}
}
