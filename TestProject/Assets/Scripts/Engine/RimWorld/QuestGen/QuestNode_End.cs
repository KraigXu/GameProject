using System;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001106 RID: 4358
	public class QuestNode_End : QuestNode
	{
		// Token: 0x06006642 RID: 26178 RVA: 0x0001028D File Offset: 0x0000E48D
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x06006643 RID: 26179 RVA: 0x0023D234 File Offset: 0x0023B434
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			int value = this.goodwillChangeAmount.GetValue(slate);
			Thing value2 = this.goodwillChangeFactionOf.GetValue(slate);
			if (value != 0 && value2 != null && value2.Faction != null)
			{
				QuestPart_FactionGoodwillChange questPart_FactionGoodwillChange = new QuestPart_FactionGoodwillChange();
				questPart_FactionGoodwillChange.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
				questPart_FactionGoodwillChange.faction = value2.Faction;
				questPart_FactionGoodwillChange.change = value;
				slate.Set<string>("goodwillPenalty", Mathf.Abs(value).ToString(), false);
				QuestGen.quest.AddPart(questPart_FactionGoodwillChange);
			}
			QuestPart_QuestEnd questPart_QuestEnd = new QuestPart_QuestEnd();
			questPart_QuestEnd.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_QuestEnd.outcome = new QuestEndOutcome?(this.outcome.GetValue(slate));
			questPart_QuestEnd.signalListenMode = (this.signalListenMode.GetValue(slate) ?? QuestPart.SignalListenMode.OngoingOnly);
			questPart_QuestEnd.sendLetter = (this.sendStandardLetter.GetValue(slate) ?? false);
			QuestGen.quest.AddPart(questPart_QuestEnd);
		}

		// Token: 0x04003E58 RID: 15960
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x04003E59 RID: 15961
		public SlateRef<QuestEndOutcome> outcome;

		// Token: 0x04003E5A RID: 15962
		public SlateRef<QuestPart.SignalListenMode?> signalListenMode;

		// Token: 0x04003E5B RID: 15963
		public SlateRef<bool?> sendStandardLetter;

		// Token: 0x04003E5C RID: 15964
		public SlateRef<int> goodwillChangeAmount;

		// Token: 0x04003E5D RID: 15965
		public SlateRef<Thing> goodwillChangeFactionOf;
	}
}
