using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGenNew
{
	
	public class QuestNode_DisableRandomMoodCausedMentalBreaks : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			IEnumerable<Pawn> value = this.pawns.GetValue(slate);
			if (value.EnumerableNullOrEmpty<Pawn>())
			{
				return;
			}
			QuestPart_DisableRandomMoodCausedMentalBreaks questPart_DisableRandomMoodCausedMentalBreaks = new QuestPart_DisableRandomMoodCausedMentalBreaks();
			questPart_DisableRandomMoodCausedMentalBreaks.inSignalEnable = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalEnable.GetValue(slate)) ?? slate.Get<string>("inSignal", null, false));
			questPart_DisableRandomMoodCausedMentalBreaks.inSignalDisable = QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalDisable.GetValue(slate));
			questPart_DisableRandomMoodCausedMentalBreaks.pawns.AddRange(value);
			QuestGen.quest.AddPart(questPart_DisableRandomMoodCausedMentalBreaks);
		}

		
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		
		[NoTranslate]
		public SlateRef<string> inSignalDisable;

		
		public SlateRef<IEnumerable<Pawn>> pawns;
	}
}
