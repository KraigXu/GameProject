using System;

namespace RimWorld.QuestGen
{
	
	public class QuestNode_IsTrueOrUnset : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			if (this.value.GetValue(slate) ?? true)
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		
		protected override void RunInt()
		{
			if (this.value.GetValue(QuestGen.slate) ?? true)
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

		
		public SlateRef<bool?> value;

		
		public QuestNode node;

		
		public QuestNode elseNode;
	}
}
