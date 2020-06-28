using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200110F RID: 4367
	public class QuestNode_SignalActivable : QuestNode
	{
		// Token: 0x0600665D RID: 26205 RVA: 0x0023DABC File Offset: 0x0023BCBC
		protected override bool TestRunInt(Slate slate)
		{
			return this.node == null || this.node.TestRun(slate);
		}

		// Token: 0x0600665E RID: 26206 RVA: 0x0023DAD4 File Offset: 0x0023BCD4
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if ((this.outSignals.GetValue(slate) != null && this.outSignals.GetValue(slate).Count<string>() != 0) + this.node == null)
			{
				return;
			}
			QuestPart_PassActivable questPart_PassActivable = new QuestPart_PassActivable();
			QuestGen.quest.AddPart(questPart_PassActivable);
			questPart_PassActivable.inSignalEnable = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalEnable.GetValue(slate)) ?? slate.Get<string>("inSignal", null, false));
			questPart_PassActivable.inSignalDisable = QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalDisable.GetValue(slate));
			questPart_PassActivable.inSignal = QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate));
			if (this.node != null)
			{
				QuestGenUtility.RunInnerNode(this.node, questPart_PassActivable.OutSignalCompleted);
			}
			IEnumerable<string> value = this.outSignals.GetValue(slate);
			if (value != null)
			{
				foreach (string signal in value)
				{
					questPart_PassActivable.outSignalsCompleted.Add(QuestGenUtility.HardcodedSignalWithQuestID(signal));
				}
			}
			questPart_PassActivable.signalListenMode = (this.signalListenMode.GetValue(slate) ?? QuestPart.SignalListenMode.OngoingOnly);
		}

		// Token: 0x04003E74 RID: 15988
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		// Token: 0x04003E75 RID: 15989
		[NoTranslate]
		public SlateRef<string> inSignalDisable;

		// Token: 0x04003E76 RID: 15990
		[NoTranslate]
		[TranslationHandle(Priority = 100)]
		public SlateRef<string> inSignal;

		// Token: 0x04003E77 RID: 15991
		[NoTranslate]
		public SlateRef<IEnumerable<string>> outSignals;

		// Token: 0x04003E78 RID: 15992
		public QuestNode node;

		// Token: 0x04003E79 RID: 15993
		public SlateRef<QuestPart.SignalListenMode?> signalListenMode;
	}
}
