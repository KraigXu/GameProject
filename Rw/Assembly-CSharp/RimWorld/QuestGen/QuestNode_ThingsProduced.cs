using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001195 RID: 4501
	public class QuestNode_ThingsProduced : QuestNode
	{
		// Token: 0x06006845 RID: 26693 RVA: 0x00246DC4 File Offset: 0x00244FC4
		protected override bool TestRunInt(Slate slate)
		{
			return this.node == null || this.node.TestRun(slate);
		}

		// Token: 0x06006846 RID: 26694 RVA: 0x00246DDC File Offset: 0x00244FDC
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

		// Token: 0x04004097 RID: 16535
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		// Token: 0x04004098 RID: 16536
		[NoTranslate]
		public SlateRef<string> outSignalComplete;

		// Token: 0x04004099 RID: 16537
		public SlateRef<ThingDef> def;

		// Token: 0x0400409A RID: 16538
		public SlateRef<ThingDef> stuff;

		// Token: 0x0400409B RID: 16539
		public SlateRef<int> count;

		// Token: 0x0400409C RID: 16540
		public QuestNode node;
	}
}
