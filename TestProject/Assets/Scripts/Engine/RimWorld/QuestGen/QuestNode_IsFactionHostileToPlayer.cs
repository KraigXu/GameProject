using System;
using Verse;

namespace RimWorld.QuestGen
{
	
	public class QuestNode_IsFactionHostileToPlayer : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			if (this.IsHostile(slate))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		
		protected override void RunInt()
		{
			if (this.IsHostile(QuestGen.slate))
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

		
		private bool IsHostile(Slate slate)
		{
			Faction value = this.faction.GetValue(slate);
			if (value != null)
			{
				return value.HostileTo(Faction.OfPlayer);
			}
			Thing value2 = this.factionOf.GetValue(slate);
			return value2 != null && value2.Faction != null && value2.Faction.HostileTo(Faction.OfPlayer);
		}

		
		public SlateRef<Faction> faction;

		
		public SlateRef<Thing> factionOf;

		
		public QuestNode node;

		
		public QuestNode elseNode;
	}
}
