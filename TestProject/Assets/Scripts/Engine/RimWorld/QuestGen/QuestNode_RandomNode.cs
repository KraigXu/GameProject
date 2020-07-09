using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.QuestGen
{
	
	public class QuestNode_RandomNode : QuestNode
	{
		
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

		
		protected override void RunInt()
		{
			QuestNode questNode;
			if (!this.GetNodesCanRun(QuestGen.slate).TryRandomElementByWeight((QuestNode e) => e.SelectionWeight(QuestGen.slate), out questNode))
			{
				return;
			}
			questNode.Run();
		}

		
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

		
		public List<QuestNode> nodes = new List<QuestNode>();
	}
}
