using System;
using Verse;

namespace RimWorld.QuestGen
{
	
	public class QuestNode_ThingsProduced : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			return this.node == null || this.node.TestRun(slate);
		}

		
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_ThingsProduced questPart_ThingsProduced = new QuestPart_ThingsProduced();
			questPart_ThingsProduced.inSignalEnable = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalEnable.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_ThingsProduced.def = this.def.GetValue(slate);
			questPart_ThingsProduced.stuff = this.stuff.GetValue(slate);
			questPart_ThingsProduced.count = this.count.GetValue(slate);
			if (this.node != null)
			{
				QuestGenUtility.RunInnerNode(this.node, questPart_ThingsProduced);
			}
			if (!this.outSignalComplete.GetValue(slate).NullOrEmpty())
			{
				questPart_ThingsProduced.outSignalsCompleted.Add(QuestGenUtility.HardcodedSignalWithQuestID(this.outSignalComplete.GetValue(slate)));
			}
			QuestGen.quest.AddPart(questPart_ThingsProduced);
		}

		
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		
		[NoTranslate]
		public SlateRef<string> outSignalComplete;

		
		public SlateRef<ThingDef> def;

		
		public SlateRef<ThingDef> stuff;

		
		public SlateRef<int> count;

		
		public QuestNode node;
	}
}
