using System;
using Verse;

namespace RimWorld.QuestGen
{
	
	public class QuestNode_PlantsHarvested : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			return this.node == null || this.node.TestRun(slate);
		}

		
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

		
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		
		[NoTranslate]
		public SlateRef<string> outSignalComplete;

		
		public SlateRef<ThingDef> plant;

		
		public SlateRef<int> count;

		
		public QuestNode node;
	}
}
