using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200117E RID: 4478
	public class QuestNode_NoWorldObject : QuestNode
	{
		// Token: 0x060067FB RID: 26619 RVA: 0x00245A8D File Offset: 0x00243C8D
		protected override bool TestRunInt(Slate slate)
		{
			return this.node == null || this.node.TestRun(slate);
		}

		// Token: 0x060067FC RID: 26620 RVA: 0x00245AA8 File Offset: 0x00243CA8
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_NoWorldObject questPart_NoWorldObject = new QuestPart_NoWorldObject();
			questPart_NoWorldObject.worldObject = this.worldObject.GetValue(slate);
			questPart_NoWorldObject.inSignalEnable = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalEnable.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			if (this.node != null)
			{
				QuestGenUtility.RunInnerNode(this.node, questPart_NoWorldObject);
			}
			if (!this.outSignalComplete.GetValue(slate).NullOrEmpty())
			{
				questPart_NoWorldObject.outSignalsCompleted.Add(QuestGenUtility.HardcodedSignalWithQuestID(this.outSignalComplete.GetValue(slate)));
			}
			QuestGen.quest.AddPart(questPart_NoWorldObject);
		}

		// Token: 0x0400403E RID: 16446
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		// Token: 0x0400403F RID: 16447
		[NoTranslate]
		public SlateRef<string> outSignalComplete;

		// Token: 0x04004040 RID: 16448
		public SlateRef<WorldObject> worldObject;

		// Token: 0x04004041 RID: 16449
		public QuestNode node;
	}
}
