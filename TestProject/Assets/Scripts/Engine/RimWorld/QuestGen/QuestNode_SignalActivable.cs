using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.QuestGenNew
{
	
	public class QuestNode_SignalActivable : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			return this.node == null || this.node.TestRun(slate);
		}

		
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

		
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		
		[NoTranslate]
		public SlateRef<string> inSignalDisable;

		
		[NoTranslate]
		[TranslationHandle(Priority = 100)]
		public SlateRef<string> inSignal;

		
		[NoTranslate]
		public SlateRef<IEnumerable<string>> outSignals;

		
		public QuestNode node;

		
		public SlateRef<QuestPart.SignalListenMode?> signalListenMode;
	}
}
