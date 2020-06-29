using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGenNew
{
	
	public class QuestNode_RemoveEquipmentFromPawns : QuestNode
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
			QuestPart_RemoveEquipmentFromPawns questPart_RemoveEquipmentFromPawns = new QuestPart_RemoveEquipmentFromPawns();
			questPart_RemoveEquipmentFromPawns.pawns.AddRange(this.pawns.GetValue(slate));
			questPart_RemoveEquipmentFromPawns.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			QuestGen.quest.AddPart(questPart_RemoveEquipmentFromPawns);
		}

		
		[NoTranslate]
		public SlateRef<string> inSignal;

		
		public SlateRef<IEnumerable<Pawn>> pawns;
	}
}
