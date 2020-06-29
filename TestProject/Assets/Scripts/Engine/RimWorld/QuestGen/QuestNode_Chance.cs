using System;
using Verse;

namespace RimWorld.QuestGenNew
{
	
	public class QuestNode_Chance : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			if (this.node == null || this.elseNode == null)
			{
				return true;
			}
			if (this.node.TestRun(slate.DeepCopy()))
			{
				this.node.TestRun(slate);
				return true;
			}
			if (this.elseNode.TestRun(slate.DeepCopy()))
			{
				this.elseNode.TestRun(slate);
				return true;
			}
			return false;
		}

		
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (Rand.Chance(this.chance.GetValue(slate)))
			{
				if (this.node == null)
				{
					return;
				}
				if (this.node.TestRun(QuestGen.slate.DeepCopy()))
				{
					this.node.Run();
					return;
				}
				if (this.elseNode != null && this.elseNode.TestRun(QuestGen.slate.DeepCopy()))
				{
					this.elseNode.Run();
					return;
				}
			}
			else
			{
				if (this.elseNode == null)
				{
					return;
				}
				if (this.elseNode.TestRun(QuestGen.slate.DeepCopy()))
				{
					this.elseNode.Run();
					return;
				}
				if (this.node != null && this.node.TestRun(QuestGen.slate.DeepCopy()))
				{
					this.node.Run();
				}
			}
		}

		
		public SlateRef<float> chance;

		
		public QuestNode node;

		
		public QuestNode elseNode;
	}
}
