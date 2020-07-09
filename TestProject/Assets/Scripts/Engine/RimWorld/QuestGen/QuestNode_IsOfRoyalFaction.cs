using System;
using Verse;

namespace RimWorld.QuestGen
{
	
	public class QuestNode_IsOfRoyalFaction : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			if (this.IsOfRoyalFaction(slate))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		
		protected override void RunInt()
		{
			if (this.IsOfRoyalFaction(QuestGen.slate))
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

		
		private bool IsOfRoyalFaction(Slate slate)
		{
			return this.thing.GetValue(slate) != null && this.thing.GetValue(slate).Faction != null && this.thing.GetValue(slate).Faction.def.HasRoyalTitles;
		}

		
		public SlateRef<Thing> thing;

		
		public QuestNode node;

		
		public QuestNode elseNode;
	}
}
