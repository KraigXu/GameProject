using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGenNew
{
	
	public class QuestNode_SetAllApparelLocked : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.pawns.GetValue(slate) == null)
			{
				return;
			}
			QuestPart_SetAllApparelLocked questPart_SetAllApparelLocked = new QuestPart_SetAllApparelLocked();
			questPart_SetAllApparelLocked.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? slate.Get<string>("inSignal", null, false));
			questPart_SetAllApparelLocked.pawns.AddRange(this.pawns.GetValue(slate));
			QuestGen.quest.AddPart(questPart_SetAllApparelLocked);
		}

		
		[NoTranslate]
		public SlateRef<string> inSignal;

		
		public SlateRef<IEnumerable<Pawn>> pawns;
	}
}
