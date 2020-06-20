using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200119F RID: 4511
	public class QuestNode_DecreeNotPossible : QuestNode
	{
		// Token: 0x0600686E RID: 26734 RVA: 0x0001028D File Offset: 0x0000E48D
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600686F RID: 26735 RVA: 0x00247834 File Offset: 0x00245A34
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_Filter_DecreeNotPossible questPart_Filter_DecreeNotPossible = new QuestPart_Filter_DecreeNotPossible();
			questPart_Filter_DecreeNotPossible.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			if (this.node != null)
			{
				questPart_Filter_DecreeNotPossible.outSignal = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
				QuestGenUtility.RunInnerNode(this.node, questPart_Filter_DecreeNotPossible.outSignal);
			}
			QuestGen.quest.AddPart(questPart_Filter_DecreeNotPossible);
		}

		// Token: 0x040040BE RID: 16574
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x040040BF RID: 16575
		public QuestNode node;

		// Token: 0x040040C0 RID: 16576
		private const string OuterNodeCompletedSignal = "OuterNodeCompleted";
	}
}
