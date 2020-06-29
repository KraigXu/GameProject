using System;
using Verse;

namespace RimWorld.QuestGenNew
{
	
	public class QuestNode_IsInList : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			if (QuestGenUtility.IsInList(slate, this.name.GetValue(slate), this.value.GetValue(slate)))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (QuestGenUtility.IsInList(slate, this.name.GetValue(slate), this.value.GetValue(slate)))
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
		public SlateRef<string> name;

		
		public SlateRef<object> value;

		
		public QuestNode node;

		
		public QuestNode elseNode;
	}
}
