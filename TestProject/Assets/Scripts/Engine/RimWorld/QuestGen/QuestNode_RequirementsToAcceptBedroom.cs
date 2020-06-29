using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.QuestGenNew
{
	
	public class QuestNode_RequirementsToAcceptBedroom : QuestNode
	{
		
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			Quest quest = QuestGen.quest;
			QuestPart_RequirementsToAcceptBedroom questPart_RequirementsToAcceptBedroom = new QuestPart_RequirementsToAcceptBedroom();
			questPart_RequirementsToAcceptBedroom.targetPawns = (from p in this.pawns.GetValue(QuestGen.slate)
			where p.royalty != null && p.royalty.HighestTitleWithBedroomRequirements() != null
			orderby p.royalty.HighestTitleWithBedroomRequirements().def.seniority descending
			select p).ToList<Pawn>();
			questPart_RequirementsToAcceptBedroom.mapParent = slate.Get<Map>("map", null, false).Parent;
			quest.AddPart(questPart_RequirementsToAcceptBedroom);
		}

		
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		
		public SlateRef<IEnumerable<Pawn>> pawns;
	}
}
