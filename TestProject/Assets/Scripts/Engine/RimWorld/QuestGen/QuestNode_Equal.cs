using System;
using Verse;

namespace RimWorld.QuestGen
{
	
	public class QuestNode_Equal : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			if (QuestNodeEqualUtility.Equal(this.value1.GetValue(slate), this.value2.GetValue(slate), this.compareAs.GetValue(slate)))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (QuestNodeEqualUtility.Equal(this.value1.GetValue(slate), this.value2.GetValue(slate), this.compareAs.GetValue(slate)))
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

		
		[NoTranslate]
		public SlateRef<object> value1;

		
		[NoTranslate]
		public SlateRef<object> value2;

		
		public SlateRef<Type> compareAs;

		
		public QuestNode node;

		
		public QuestNode elseNode;
	}
}
