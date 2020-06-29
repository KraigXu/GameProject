using System;
using Verse;

namespace RimWorld.QuestGenNew
{
	
	public class QuestNode_HasRoyalTitleInCurrentFaction : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			if (this.HasRoyalTitleInCurrentFaction(slate))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		
		protected override void RunInt()
		{
			if (this.HasRoyalTitleInCurrentFaction(QuestGen.slate))
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

		
		private bool HasRoyalTitleInCurrentFaction(Slate slate)
		{
			Pawn value = this.pawn.GetValue(slate);
			return value != null && value.Faction != null && value.royalty != null && value.royalty.HasAnyTitleIn(value.Faction);
		}

		
		public SlateRef<Pawn> pawn;

		
		public QuestNode node;

		
		public QuestNode elseNode;
	}
}
