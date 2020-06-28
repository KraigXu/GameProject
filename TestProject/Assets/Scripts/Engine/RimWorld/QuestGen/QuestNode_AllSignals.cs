using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001101 RID: 4353
	public class QuestNode_AllSignals : QuestNode
	{
		// Token: 0x06006631 RID: 26161 RVA: 0x0023C9F1 File Offset: 0x0023ABF1
		protected override bool TestRunInt(Slate slate)
		{
			return this.node == null || this.node.TestRun(slate);
		}

		// Token: 0x06006632 RID: 26162 RVA: 0x0023CA0C File Offset: 0x0023AC0C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			int num = ((this.outSignals.GetValue(slate) != null) ? this.outSignals.GetValue(slate).Count<string>() : 0) + ((this.node != null) ? 1 : 0);
			if (num == 0)
			{
				return;
			}
			if (num == 1)
			{
				QuestPart_PassAll questPart_PassAll = new QuestPart_PassAll();
				foreach (string signal in this.inSignals.GetValue(slate))
				{
					questPart_PassAll.inSignals.Add(QuestGenUtility.HardcodedSignalWithQuestID(signal));
				}
				if (this.node != null)
				{
					questPart_PassAll.outSignal = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
					QuestGenUtility.RunInnerNode(this.node, questPart_PassAll.outSignal);
				}
				else
				{
					questPart_PassAll.outSignal = QuestGenUtility.HardcodedSignalWithQuestID(this.outSignals.GetValue(slate).First<string>());
				}
				questPart_PassAll.signalListenMode = (this.signalListenMode.GetValue(slate) ?? QuestPart.SignalListenMode.OngoingOnly);
				QuestGen.quest.AddPart(questPart_PassAll);
				return;
			}
			QuestPart_PassAllOutMany questPart_PassAllOutMany = new QuestPart_PassAllOutMany();
			foreach (string signal2 in this.inSignals.GetValue(slate))
			{
				questPart_PassAllOutMany.inSignals.Add(QuestGenUtility.HardcodedSignalWithQuestID(signal2));
			}
			if (this.node != null)
			{
				string text = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
				questPart_PassAllOutMany.outSignals.Add(text);
				QuestGenUtility.RunInnerNode(this.node, text);
			}
			foreach (string signal3 in this.outSignals.GetValue(slate))
			{
				questPart_PassAllOutMany.outSignals.Add(QuestGenUtility.HardcodedSignalWithQuestID(signal3));
			}
			questPart_PassAllOutMany.signalListenMode = (this.signalListenMode.GetValue(slate) ?? QuestPart.SignalListenMode.OngoingOnly);
			QuestGen.quest.AddPart(questPart_PassAllOutMany);
		}

		// Token: 0x04003E3B RID: 15931
		[NoTranslate]
		public SlateRef<IEnumerable<string>> inSignals;

		// Token: 0x04003E3C RID: 15932
		[NoTranslate]
		public SlateRef<IEnumerable<string>> outSignals;

		// Token: 0x04003E3D RID: 15933
		public QuestNode node;

		// Token: 0x04003E3E RID: 15934
		public SlateRef<QuestPart.SignalListenMode?> signalListenMode;

		// Token: 0x04003E3F RID: 15935
		private const string OuterNodeCompletedSignal = "OuterNodeCompleted";
	}
}
