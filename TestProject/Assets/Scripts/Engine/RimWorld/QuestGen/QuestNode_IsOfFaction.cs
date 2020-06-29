using System;
using Verse;

namespace RimWorld.QuestGenNew
{
	
	public class QuestNode_IsOfFaction : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			if (this.IsOfFaction(slate))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		
		protected override void RunInt()
		{
			if (this.IsOfFaction(QuestGen.slate))
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

		
		private bool IsOfFaction(Slate slate)
		{
			return this.thing.GetValue(slate) != null && this.thing.GetValue(slate).Faction == this.faction.GetValue(slate);
		}

		
		public SlateRef<Thing> thing;

		
		public SlateRef<Faction> faction;

		
		public QuestNode node;

		
		public QuestNode elseNode;
	}
}
