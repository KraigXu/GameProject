using System;

namespace RimWorld.QuestGen
{
	// Token: 0x02001185 RID: 4485
	public class QuestNode_ReplaceLostLeaderReferences : QuestNode
	{
		// Token: 0x06006812 RID: 26642 RVA: 0x0001028D File Offset: 0x0000E48D
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x06006813 RID: 26643 RVA: 0x0024630C File Offset: 0x0024450C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_ReplaceLostLeaderReferences questPart_ReplaceLostLeaderReferences = new QuestPart_ReplaceLostLeaderReferences();
			questPart_ReplaceLostLeaderReferences.inSignal = QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate));
			questPart_ReplaceLostLeaderReferences.signalListenMode = (this.signalListenMode.GetValue(slate) ?? QuestPart.SignalListenMode.OngoingOnly);
			QuestGen.quest.AddPart(questPart_ReplaceLostLeaderReferences);
		}

		// Token: 0x04004065 RID: 16485
		public SlateRef<string> inSignal;

		// Token: 0x04004066 RID: 16486
		public SlateRef<QuestPart.SignalListenMode?> signalListenMode;
	}
}
