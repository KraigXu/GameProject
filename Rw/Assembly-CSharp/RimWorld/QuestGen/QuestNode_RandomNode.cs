using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001109 RID: 4361
	public class QuestNode_RandomNode : QuestNode
	{
		// Token: 0x0600664A RID: 26186 RVA: 0x0023D528 File Offset: 0x0023B728
		protected override bool TestRunInt(Slate slate)
		{
			QuestNode questNode = this.GetNodesCanRun(slate).FirstOrDefault<QuestNode>();
			if (questNode == null)
			{
				return false;
			}
			questNode.TestRun(slate);
			return true;
		}

		// Token: 0x0600664B RID: 26187 RVA: 0x0023D550 File Offset: 0x0023B750
		protected override void RunInt()
		{
			QuestNode questNode;
			if (!this.GetNodesCanRun(QuestGen.slate).TryRandomElementByWeight((QuestNode e) => e.SelectionWeight(QuestGen.slate), out questNode))
			{
				return;
			}
			questNode.Run();
		}

		// Token: 0x0600664C RID: 26188 RVA: 0x0023D597 File Offset: 0x0023B797
		private IEnumerable<QuestNode> GetNodesCanRun(Slate slate)
		{
			int num;
			for (int i = 0; i < this.nodes.Count; i = num + 1)
			{
				if (this.nodes[i].SelectionWeight(slate) > 0f && this.nodes[i].TestRun(slate.DeepCopy()))
				{
					yield return this.nodes[i];
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x04003E65 RID: 15973
		public List<QuestNode> nodes = new List<QuestNode>();
	}
}
