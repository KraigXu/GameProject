using System;
using Verse;

namespace RimWorld.QuestGenNew
{
	
	public class QuestNode_IsSet : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			if (slate.Exists(this.name.GetValue(slate), false))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (QuestGen.slate.Exists(this.name.GetValue(slate), false))
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

		
		public QuestNode node;

		
		public QuestNode elseNode;
	}
}
