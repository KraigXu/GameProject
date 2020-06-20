using System;

namespace Verse.AI
{
	// Token: 0x0200059C RID: 1436
	public class ThinkNode_Subtree : ThinkNode
	{
		// Token: 0x06002887 RID: 10375 RVA: 0x000EF088 File Offset: 0x000ED288
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_Subtree thinkNode_Subtree = (ThinkNode_Subtree)base.DeepCopy(false);
			thinkNode_Subtree.treeDef = this.treeDef;
			if (resolve)
			{
				thinkNode_Subtree.ResolveSubnodesAndRecur();
				thinkNode_Subtree.subtreeNode = thinkNode_Subtree.subNodes[this.subNodes.IndexOf(this.subtreeNode)];
			}
			return thinkNode_Subtree;
		}

		// Token: 0x06002888 RID: 10376 RVA: 0x000EF0DA File Offset: 0x000ED2DA
		protected override void ResolveSubnodes()
		{
			this.subtreeNode = this.treeDef.thinkRoot.DeepCopy(true);
			this.subNodes.Add(this.subtreeNode);
		}

		// Token: 0x06002889 RID: 10377 RVA: 0x000EF104 File Offset: 0x000ED304
		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			return this.subtreeNode.TryIssueJobPackage(pawn, jobParams);
		}

		// Token: 0x04001856 RID: 6230
		private ThinkTreeDef treeDef;

		// Token: 0x04001857 RID: 6231
		[Unsaved(false)]
		public ThinkNode subtreeNode;
	}
}
