using System;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGenNew
{
	
	public class QuestNode_End : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		
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

		
		[NoTranslate]
		public SlateRef<string> inSignal;

		
		public SlateRef<QuestEndOutcome> outcome;

		
		public SlateRef<QuestPart.SignalListenMode?> signalListenMode;

		
		public SlateRef<bool?> sendStandardLetter;

		
		public SlateRef<int> goodwillChangeAmount;

		
		public SlateRef<Thing> goodwillChangeFactionOf;
	}
}
