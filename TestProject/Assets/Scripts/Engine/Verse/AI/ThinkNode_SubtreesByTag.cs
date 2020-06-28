using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse.AI
{
	// Token: 0x0200059D RID: 1437
	public class ThinkNode_SubtreesByTag : ThinkNode
	{
		// Token: 0x0600288B RID: 10379 RVA: 0x000EF113 File Offset: 0x000ED313
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_SubtreesByTag thinkNode_SubtreesByTag = (ThinkNode_SubtreesByTag)base.DeepCopy(resolve);
			thinkNode_SubtreesByTag.insertTag = this.insertTag;
			return thinkNode_SubtreesByTag;
		}

		// Token: 0x0600288C RID: 10380 RVA: 0x00002681 File Offset: 0x00000881
		protected override void ResolveSubnodes()
		{
		}

		// Token: 0x0600288D RID: 10381 RVA: 0x000EF130 File Offset: 0x000ED330
		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			if (this.matchedTrees == null)
			{
				this.matchedTrees = new List<ThinkTreeDef>();
				foreach (ThinkTreeDef thinkTreeDef in DefDatabase<ThinkTreeDef>.AllDefs)
				{
					if (thinkTreeDef.insertTag == this.insertTag)
					{
						this.matchedTrees.Add(thinkTreeDef);
					}
				}
				this.matchedTrees = (from tDef in this.matchedTrees
				orderby tDef.insertPriority descending
				select tDef).ToList<ThinkTreeDef>();
			}
			for (int i = 0; i < this.matchedTrees.Count; i++)
			{
				ThinkResult result = this.matchedTrees[i].thinkRoot.TryIssueJobPackage(pawn, jobParams);
				if (result.IsValid)
				{
					return result;
				}
			}
			return ThinkResult.NoJob;
		}

		// Token: 0x04001858 RID: 6232
		[NoTranslate]
		public string insertTag;

		// Token: 0x04001859 RID: 6233
		[Unsaved(false)]
		private List<ThinkTreeDef> matchedTrees;
	}
}
