using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x0200059A RID: 1434
	public class ThinkNode_Random : ThinkNode
	{
		// Token: 0x06002880 RID: 10368 RVA: 0x000EEF28 File Offset: 0x000ED128
		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			ThinkNode_Random.tempList.Clear();
			for (int i = 0; i < this.subNodes.Count; i++)
			{
				ThinkNode_Random.tempList.Add(this.subNodes[i]);
			}
			ThinkNode_Random.tempList.Shuffle<ThinkNode>();
			for (int j = 0; j < ThinkNode_Random.tempList.Count; j++)
			{
				ThinkResult result = ThinkNode_Random.tempList[j].TryIssueJobPackage(pawn, jobParams);
				if (result.IsValid)
				{
					return result;
				}
			}
			return ThinkResult.NoJob;
		}

		// Token: 0x04001854 RID: 6228
		private static List<ThinkNode> tempList = new List<ThinkNode>();
	}
}
