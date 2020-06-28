using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001181 RID: 4481
	public class QuestNode_PlantsHarvested : QuestNode
	{
		// Token: 0x06006804 RID: 26628 RVA: 0x00245FDC File Offset: 0x002441DC
		protected override bool TestRunInt(Slate slate)
		{
			return this.node == null || this.node.TestRun(slate);
		}

		// Token: 0x06006805 RID: 26629 RVA: 0x00245FF4 File Offset: 0x002441F4
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_PlantsHarvested questPart_PlantsHarvested = new QuestPart_PlantsHarvested();
			questPart_PlantsHarvested.inSignalEnable = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalEnable.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_PlantsHarvested.plant = this.plant.GetValue(slate);
			questPart_PlantsHarvested.count = this.count.GetValue(slate);
			if (this.node != null)
			{
				QuestGenUtility.RunInnerNode(this.node, questPart_PlantsHarvested);
			}
			if (!this.outSignalComplete.GetValue(slate).NullOrEmpty())
			{
				questPart_PlantsHarvested.outSignalsCompleted.Add(QuestGenUtility.HardcodedSignalWithQuestID(this.outSignalComplete.GetValue(slate)));
			}
			QuestGen.quest.AddPart(questPart_PlantsHarvested);
		}

		// Token: 0x04004055 RID: 16469
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		// Token: 0x04004056 RID: 16470
		[NoTranslate]
		public SlateRef<string> outSignalComplete;

		// Token: 0x04004057 RID: 16471
		public SlateRef<ThingDef> plant;

		// Token: 0x04004058 RID: 16472
		public SlateRef<int> count;

		// Token: 0x04004059 RID: 16473
		public QuestNode node;
	}
}
