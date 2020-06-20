using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200119E RID: 4510
	public class QuestNode_AnyPawnAlive : QuestNode
	{
		// Token: 0x0600686B RID: 26731 RVA: 0x0001028D File Offset: 0x0000E48D
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600686C RID: 26732 RVA: 0x002477A8 File Offset: 0x002459A8
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_Filter_AnyPawnAlive questPart_Filter_AnyPawnAlive = new QuestPart_Filter_AnyPawnAlive
			{
				inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false)),
				pawns = this.pawns.GetValue(slate)
			};
			if (this.node != null)
			{
				questPart_Filter_AnyPawnAlive.outSignal = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
				QuestGenUtility.RunInnerNode(this.node, questPart_Filter_AnyPawnAlive.outSignal);
			}
			QuestGen.quest.AddPart(questPart_Filter_AnyPawnAlive);
		}

		// Token: 0x040040BA RID: 16570
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x040040BB RID: 16571
		public SlateRef<List<Pawn>> pawns;

		// Token: 0x040040BC RID: 16572
		public QuestNode node;

		// Token: 0x040040BD RID: 16573
		private const string OuterNodeCompletedSignal = "OuterNodeCompleted";
	}
}
