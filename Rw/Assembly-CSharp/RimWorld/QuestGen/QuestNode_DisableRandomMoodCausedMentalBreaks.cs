using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001169 RID: 4457
	public class QuestNode_DisableRandomMoodCausedMentalBreaks : QuestNode
	{
		// Token: 0x060067B3 RID: 26547 RVA: 0x0001028D File Offset: 0x0000E48D
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x060067B4 RID: 26548 RVA: 0x00243D60 File Offset: 0x00241F60
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

		// Token: 0x04003FCD RID: 16333
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		// Token: 0x04003FCE RID: 16334
		[NoTranslate]
		public SlateRef<string> inSignalDisable;

		// Token: 0x04003FCF RID: 16335
		public SlateRef<IEnumerable<Pawn>> pawns;
	}
}
