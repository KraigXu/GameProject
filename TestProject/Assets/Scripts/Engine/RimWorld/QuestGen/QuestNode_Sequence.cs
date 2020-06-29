using System;
using System.Collections.Generic;

namespace RimWorld.QuestGen
{
	
	public class QuestNode_Sequence : QuestNode
	{
		
		protected override void RunInt()
		{
			for (int i = 0; i < this.nodes.Count; i++)
			{
				this.nodes[i].Run();
			}
		}

		
		protected override bool TestRunInt(Slate slate)
		{
			for (int i = 0; i < this.nodes.Count; i++)
			{
				if (!this.nodes[i].TestRun(slate))
				{
					return false;
				}
			}
			return true;
		}

		
		public List<QuestNode> nodes = new List<QuestNode>();
	}
}
