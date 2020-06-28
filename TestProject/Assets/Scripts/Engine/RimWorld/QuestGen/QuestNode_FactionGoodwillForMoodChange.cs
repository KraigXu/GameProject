using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200116E RID: 4462
	public class QuestNode_FactionGoodwillForMoodChange : QuestNode
	{
		// Token: 0x060067C2 RID: 26562 RVA: 0x0001028D File Offset: 0x0000E48D
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x060067C3 RID: 26563 RVA: 0x00244230 File Offset: 0x00242430
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

		// Token: 0x04003FEA RID: 16362
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x04003FEB RID: 16363
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		// Token: 0x04003FEC RID: 16364
		[NoTranslate]
		public SlateRef<string> outSignalSuccess;

		// Token: 0x04003FED RID: 16365
		[NoTranslate]
		public SlateRef<string> outSignalFailed;

		// Token: 0x04003FEE RID: 16366
		public SlateRef<IEnumerable<Pawn>> pawns;
	}
}
