using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGenNew
{
	
	public class QuestNode_IsFreeWorldPawn : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			if (this.IsFreeWorldPawn(slate))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		
		protected override void RunInt()
		{
			if (this.IsFreeWorldPawn(QuestGen.slate))
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

		
		private bool IsFreeWorldPawn(Slate slate)
		{
			return this.pawn.GetValue(slate) != null && Find.WorldPawns.GetSituation(this.pawn.GetValue(slate)) == WorldPawnSituation.Free;
		}

		
		public SlateRef<Pawn> pawn;

		
		public QuestNode node;

		
		public QuestNode elseNode;
	}
}
