using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGenNew
{
	
	public class QuestNode_MoodBelow : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			return this.node == null || this.node.TestRun(slate);
		}

		
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.pawns.GetValue(slate) == null)
			{
				return;
			}
			QuestPart_MoodBelow questPart_MoodBelow = new QuestPart_MoodBelow();
			questPart_MoodBelow.pawns.AddRange(this.pawns.GetValue(slate));
			questPart_MoodBelow.threshold = this.threshold.GetValue(slate);
			questPart_MoodBelow.minTicksBelowThreshold = 40000;
			questPart_MoodBelow.inSignalEnable = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalEnable.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			if (this.node != null)
			{
				QuestGenUtility.RunInnerNode(this.node, questPart_MoodBelow);
			}
			if (!this.outSignal.GetValue(slate).NullOrEmpty())
			{
				questPart_MoodBelow.outSignalsCompleted.Add(this.outSignal.GetValue(slate));
			}
			QuestGen.quest.AddPart(questPart_MoodBelow);
		}

		
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		
		[NoTranslate]
		public SlateRef<string> outSignal;

		
		public SlateRef<IEnumerable<Pawn>> pawns;

		
		public SlateRef<float> threshold;

		
		public QuestNode node;

		
		private const int MinTicksBelowMinMood = 40000;
	}
}
