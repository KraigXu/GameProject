using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001105 RID: 4357
	public class QuestNode_Delay : QuestNode
	{
		// Token: 0x0600663E RID: 26174 RVA: 0x0023D05C File Offset: 0x0023B25C
		protected override bool TestRunInt(Slate slate)
		{
			return this.node == null || this.node.TestRun(slate);
		}

		// Token: 0x0600663F RID: 26175 RVA: 0x0023D074 File Offset: 0x0023B274
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_Delay questPart_Delay;
			if (this.delayTicksRange.GetValue(slate) != null)
			{
				questPart_Delay = new QuestPart_DelayRandom();
				((QuestPart_DelayRandom)questPart_Delay).delayTicksRange = this.delayTicksRange.GetValue(slate).Value;
			}
			else
			{
				questPart_Delay = this.MakeDelayQuestPart();
				questPart_Delay.delayTicks = this.delayTicks.GetValue(slate);
			}
			questPart_Delay.inSignalEnable = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalEnable.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_Delay.inSignalDisable = QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalDisable.GetValue(slate));
			questPart_Delay.reactivatable = this.reactivatable.GetValue(slate);
			if (!this.inspectStringTargets.GetValue(slate).EnumerableNullOrEmpty<ISelectable>())
			{
				questPart_Delay.inspectString = this.inspectString.GetValue(slate);
				questPart_Delay.inspectStringTargets = new List<ISelectable>();
				questPart_Delay.inspectStringTargets.AddRange(this.inspectStringTargets.GetValue(slate));
			}
			if (this.isQuestTimeout.GetValue(slate))
			{
				questPart_Delay.isBad = true;
				questPart_Delay.expiryInfoPart = "QuestExpiresIn".Translate();
				questPart_Delay.expiryInfoPartTip = "QuestExpiresOn".Translate();
			}
			else
			{
				questPart_Delay.expiryInfoPart = this.expiryInfoPart.GetValue(slate);
				questPart_Delay.expiryInfoPartTip = this.expiryInfoPartTip.GetValue(slate);
			}
			if (this.node != null)
			{
				QuestGenUtility.RunInnerNode(this.node, questPart_Delay);
			}
			if (!this.outSignalComplete.GetValue(slate).NullOrEmpty())
			{
				questPart_Delay.outSignalsCompleted.Add(QuestGenUtility.HardcodedSignalWithQuestID(this.outSignalComplete.GetValue(slate)));
			}
			QuestGen.quest.AddPart(questPart_Delay);
		}

		// Token: 0x06006640 RID: 26176 RVA: 0x0023D22C File Offset: 0x0023B42C
		protected virtual QuestPart_Delay MakeDelayQuestPart()
		{
			return new QuestPart_Delay();
		}

		// Token: 0x04003E4C RID: 15948
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		// Token: 0x04003E4D RID: 15949
		[NoTranslate]
		public SlateRef<string> inSignalDisable;

		// Token: 0x04003E4E RID: 15950
		[NoTranslate]
		public SlateRef<string> outSignalComplete;

		// Token: 0x04003E4F RID: 15951
		public SlateRef<string> expiryInfoPart;

		// Token: 0x04003E50 RID: 15952
		public SlateRef<string> expiryInfoPartTip;

		// Token: 0x04003E51 RID: 15953
		public SlateRef<string> inspectString;

		// Token: 0x04003E52 RID: 15954
		public SlateRef<IEnumerable<ISelectable>> inspectStringTargets;

		// Token: 0x04003E53 RID: 15955
		public SlateRef<int> delayTicks;

		// Token: 0x04003E54 RID: 15956
		public SlateRef<IntRange?> delayTicksRange;

		// Token: 0x04003E55 RID: 15957
		public SlateRef<bool> isQuestTimeout;

		// Token: 0x04003E56 RID: 15958
		public SlateRef<bool> reactivatable;

		// Token: 0x04003E57 RID: 15959
		public QuestNode node;
	}
}
