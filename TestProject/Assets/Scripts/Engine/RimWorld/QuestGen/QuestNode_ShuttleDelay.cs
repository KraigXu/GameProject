using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200118F RID: 4495
	public class QuestNode_ShuttleDelay : QuestNode
	{
		// Token: 0x06006832 RID: 26674 RVA: 0x00246689 File Offset: 0x00244889
		protected override bool TestRunInt(Slate slate)
		{
			return this.node == null || this.node.TestRun(slate);
		}

		// Token: 0x06006833 RID: 26675 RVA: 0x002466A4 File Offset: 0x002448A4
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_ShuttleDelay questPart_ShuttleDelay = new QuestPart_ShuttleDelay();
			questPart_ShuttleDelay.inSignalEnable = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalEnable.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_ShuttleDelay.delayTicks = this.delayTicks.GetValue(slate);
			if (this.lodgers.GetValue(slate) != null)
			{
				questPart_ShuttleDelay.lodgers.AddRange(this.lodgers.GetValue(slate));
			}
			questPart_ShuttleDelay.expiryInfoPart = "ShuttleArrivesIn".Translate();
			questPart_ShuttleDelay.expiryInfoPartTip = "ShuttleArrivesOn".Translate();
			if (this.node != null)
			{
				QuestGenUtility.RunInnerNode(this.node, questPart_ShuttleDelay);
			}
			if (!this.outSignalComplete.GetValue(slate).NullOrEmpty())
			{
				questPart_ShuttleDelay.outSignalsCompleted.Add(QuestGenUtility.HardcodedSignalWithQuestID(this.outSignalComplete.GetValue(slate)));
			}
			QuestGen.quest.AddPart(questPart_ShuttleDelay);
		}

		// Token: 0x04004079 RID: 16505
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		// Token: 0x0400407A RID: 16506
		[NoTranslate]
		public SlateRef<string> outSignalComplete;

		// Token: 0x0400407B RID: 16507
		public SlateRef<int> delayTicks;

		// Token: 0x0400407C RID: 16508
		public SlateRef<IEnumerable<Pawn>> lodgers;

		// Token: 0x0400407D RID: 16509
		public QuestNode node;
	}
}
