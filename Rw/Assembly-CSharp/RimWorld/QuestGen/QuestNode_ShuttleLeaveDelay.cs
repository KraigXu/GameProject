using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001190 RID: 4496
	public class QuestNode_ShuttleLeaveDelay : QuestNode
	{
		// Token: 0x06006835 RID: 26677 RVA: 0x00246798 File Offset: 0x00244998
		protected override bool TestRunInt(Slate slate)
		{
			return this.node == null || this.node.TestRun(slate);
		}

		// Token: 0x06006836 RID: 26678 RVA: 0x002467B0 File Offset: 0x002449B0
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_ShuttleLeaveDelay questPart_ShuttleLeaveDelay = new QuestPart_ShuttleLeaveDelay();
			questPart_ShuttleLeaveDelay.inSignalEnable = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalEnable.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_ShuttleLeaveDelay.delayTicks = this.delayTicks.GetValue(slate);
			questPart_ShuttleLeaveDelay.shuttle = this.shuttle.GetValue(slate);
			questPart_ShuttleLeaveDelay.expiryInfoPart = "ShuttleDepartsIn".Translate();
			questPart_ShuttleLeaveDelay.expiryInfoPartTip = "ShuttleDepartsOn".Translate();
			if (this.inSignalsDisable.GetValue(slate) != null)
			{
				foreach (string signal in this.inSignalsDisable.GetValue(slate))
				{
					questPart_ShuttleLeaveDelay.inSignalsDisable.Add(QuestGenUtility.HardcodedSignalWithQuestID(signal));
				}
			}
			if (this.node != null)
			{
				QuestGenUtility.RunInnerNode(this.node, questPart_ShuttleLeaveDelay);
			}
			if (!this.outSignalComplete.GetValue(slate).NullOrEmpty())
			{
				questPart_ShuttleLeaveDelay.outSignalsCompleted.Add(QuestGenUtility.HardcodedSignalWithQuestID(this.outSignalComplete.GetValue(slate)));
			}
			QuestGen.quest.AddPart(questPart_ShuttleLeaveDelay);
		}

		// Token: 0x0400407E RID: 16510
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		// Token: 0x0400407F RID: 16511
		[NoTranslate]
		public SlateRef<string> outSignalComplete;

		// Token: 0x04004080 RID: 16512
		[NoTranslate]
		public SlateRef<IEnumerable<string>> inSignalsDisable;

		// Token: 0x04004081 RID: 16513
		public SlateRef<int> delayTicks;

		// Token: 0x04004082 RID: 16514
		public SlateRef<Thing> shuttle;

		// Token: 0x04004083 RID: 16515
		public QuestNode node;
	}
}
