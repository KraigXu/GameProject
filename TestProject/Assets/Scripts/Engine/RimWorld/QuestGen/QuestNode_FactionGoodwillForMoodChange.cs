using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGenNew
{
	
	public class QuestNode_FactionGoodwillForMoodChange : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.pawns.GetValue(slate) != null)
			{
				QuestPart_FactionGoodwillForMoodChange questPart_FactionGoodwillForMoodChange = new QuestPart_FactionGoodwillForMoodChange();
				questPart_FactionGoodwillForMoodChange.pawns.AddRange(this.pawns.GetValue(slate));
				questPart_FactionGoodwillForMoodChange.inSignal = QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate));
				questPart_FactionGoodwillForMoodChange.outSignalSuccess = QuestGenUtility.HardcodedSignalWithQuestID(this.outSignalSuccess.GetValue(slate));
				questPart_FactionGoodwillForMoodChange.outSignalFailed = QuestGenUtility.HardcodedSignalWithQuestID(this.outSignalFailed.GetValue(slate));
				questPart_FactionGoodwillForMoodChange.inSignalEnable = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalEnable.GetValue(slate)) ?? slate.Get<string>("inSignal", null, false));
				QuestGen.quest.AddPart(questPart_FactionGoodwillForMoodChange);
			}
		}

		
		[NoTranslate]
		public SlateRef<string> inSignal;

		
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		
		[NoTranslate]
		public SlateRef<string> outSignalSuccess;

		
		[NoTranslate]
		public SlateRef<string> outSignalFailed;

		
		public SlateRef<IEnumerable<Pawn>> pawns;
	}
}
