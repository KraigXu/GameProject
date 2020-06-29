using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	
	public class QuestNode_ShuttleLeaveDelay : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			return this.node == null || this.node.TestRun(slate);
		}

		
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

		
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		
		[NoTranslate]
		public SlateRef<string> outSignalComplete;

		
		[NoTranslate]
		public SlateRef<IEnumerable<string>> inSignalsDisable;

		
		public SlateRef<int> delayTicks;

		
		public SlateRef<Thing> shuttle;

		
		public QuestNode node;
	}
}
