using System;

namespace RimWorld.QuestGen
{
	
	public class QuestNode_RequireRoyalFavorFromFaction : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			return this.faction.GetValue(slate).allowRoyalFavorRewards;
		}

		
		protected override void RunInt()
		{
		}

		
		public SlateRef<Faction> faction;
	}
}
