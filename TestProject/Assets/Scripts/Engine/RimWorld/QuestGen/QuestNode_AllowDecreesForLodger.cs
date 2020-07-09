using System;
using Verse;

namespace RimWorld.QuestGen
{
	
	public class QuestNode_AllowDecreesForLodger : QuestNode
	{
		
		protected override void RunInt()
		{
			QuestGen.quest.AddPart(new QuestPart_AllowDecreesForLodger
			{
				lodger = this.lodger.GetValue(QuestGen.slate)
			});
		}

		
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		
		public SlateRef<Pawn> lodger;
	}
}
