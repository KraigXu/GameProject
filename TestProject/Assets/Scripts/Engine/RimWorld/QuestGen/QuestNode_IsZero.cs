using System;

namespace RimWorld.QuestGenNew
{
	
	public class QuestNode_IsZero : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			if (this.value.GetValue(slate) == 0.0)
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.value.GetValue(slate) == 0.0)
			{
				if (this.node != null)
				{
					this.node.Run();
					return;
				}
			}
			else if (this.elseNode != null)
			{
				this.elseNode.Run();
			}
		}

		
		public SlateRef<double> value;

		
		public QuestNode node;

		
		public QuestNode elseNode;
	}
}
