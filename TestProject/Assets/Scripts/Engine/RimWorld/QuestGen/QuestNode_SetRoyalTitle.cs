using System;
using Verse;

namespace RimWorld.QuestGenNew
{
	
	public class QuestNode_SetRoyalTitle : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			Pawn value = this.pawn.GetValue(slate);
			if (value.royalty != null)
			{
				value.royalty.SetTitle(this.faction.GetValue(slate), this.royalTitle.GetValue(slate), false, false, true);
			}
		}

		
		public SlateRef<Pawn> pawn;

		
		public SlateRef<RoyalTitleDef> royalTitle;

		
		public SlateRef<Faction> faction;
	}
}
