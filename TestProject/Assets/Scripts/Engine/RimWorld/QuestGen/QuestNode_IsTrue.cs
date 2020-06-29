using System;

namespace RimWorld.QuestGenNew
{
	
	public class QuestNode_IsTrue : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			if (this.value.GetValue(slate))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.value.GetValue(slate))
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

		
		public SlateRef<bool> value;

		
		public QuestNode node;

		
		public QuestNode elseNode;
	}
}
