using System;

namespace Verse.AI
{
	// Token: 0x02000596 RID: 1430
	public class ThinkNode_FilterPriority : ThinkNode
	{
		// Token: 0x06002876 RID: 10358 RVA: 0x000EEC34 File Offset: 0x000ECE34
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_FilterPriority thinkNode_FilterPriority = (ThinkNode_FilterPriority)base.DeepCopy(resolve);
			thinkNode_FilterPriority.minPriority = this.minPriority;
			return thinkNode_FilterPriority;
		}

		// Token: 0x06002877 RID: 10359 RVA: 0x000EEC50 File Offset: 0x000ECE50
		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			int count = this.subNodes.Count;
			for (int i = 0; i < count; i++)
			{
				if (this.subNodes[i].GetPriority(pawn) > this.minPriority)
				{
					ThinkResult result = this.subNodes[i].TryIssueJobPackage(pawn, jobParams);
					if (result.IsValid)
					{
						return result;
					}
				}
			}
			return ThinkResult.NoJob;
		}

		// Token: 0x04001845 RID: 6213
		public float minPriority = 0.5f;
	}
}
