using System;

namespace RimWorld.QuestGenNew
{
	
	public class QuestNode_Greater : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			if (this.value1.GetValue(slate) > this.value2.GetValue(slate))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.value1.GetValue(slate) > this.value2.GetValue(slate))
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

		
		public SlateRef<double> value1;

		
		public SlateRef<double> value2;

		
		public QuestNode node;

		
		public QuestNode elseNode;
	}
}
