using System;
using Verse;

namespace RimWorld.QuestGen
{
	
	public class QuestNode_IsFactionLeader : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			if (this.IsFactionLeader(slate))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		
		protected override void RunInt()
		{
			if (this.IsFactionLeader(QuestGen.slate))
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

		
		private bool IsFactionLeader(Slate slate)
		{
			return this.pawn.GetValue(slate) != null && this.pawn.GetValue(slate).Faction != null && this.pawn.GetValue(slate).Faction.leader == this.pawn.GetValue(slate);
		}

		
		public SlateRef<Pawn> pawn;

		
		public QuestNode node;

		
		public QuestNode elseNode;
	}
}
