using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200117D RID: 4477
	public class QuestNode_MoodBelow : QuestNode
	{
		// Token: 0x060067F8 RID: 26616 RVA: 0x002459A4 File Offset: 0x00243BA4
		protected override bool TestRunInt(Slate slate)
		{
			return this.node == null || this.node.TestRun(slate);
		}

		// Token: 0x060067F9 RID: 26617 RVA: 0x002459BC File Offset: 0x00243BBC
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

		// Token: 0x04004038 RID: 16440
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		// Token: 0x04004039 RID: 16441
		[NoTranslate]
		public SlateRef<string> outSignal;

		// Token: 0x0400403A RID: 16442
		public SlateRef<IEnumerable<Pawn>> pawns;

		// Token: 0x0400403B RID: 16443
		public SlateRef<float> threshold;

		// Token: 0x0400403C RID: 16444
		public QuestNode node;

		// Token: 0x0400403D RID: 16445
		private const int MinTicksBelowMinMood = 40000;
	}
}
