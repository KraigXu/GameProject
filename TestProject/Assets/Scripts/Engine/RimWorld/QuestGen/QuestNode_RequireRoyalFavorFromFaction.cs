using System;

namespace RimWorld.QuestGenNew
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
