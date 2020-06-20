using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020011A0 RID: 4512
	public class QuestNode_SignalFactionNonPlayer : QuestNode
	{
		// Token: 0x06006871 RID: 26737 RVA: 0x002478AE File Offset: 0x00245AAE
		protected override bool TestRunInt(Slate slate)
		{
			return this.node == null || this.node.TestRun(slate);
		}

		// Token: 0x06006872 RID: 26738 RVA: 0x002478C8 File Offset: 0x00245AC8
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_Filter_FactionNonPlayer questPart_Filter_FactionNonPlayer = new QuestPart_Filter_FactionNonPlayer();
			questPart_Filter_FactionNonPlayer.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			if (this.node != null)
			{
				questPart_Filter_FactionNonPlayer.outSignal = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
				QuestGenUtility.RunInnerNode(this.node, questPart_Filter_FactionNonPlayer.outSignal);
			}
			QuestGen.quest.AddPart(questPart_Filter_FactionNonPlayer);
		}

		// Token: 0x040040C1 RID: 16577
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x040040C2 RID: 16578
		public QuestNode node;

		// Token: 0x040040C3 RID: 16579
		private const string OuterNodeCompletedSignal = "OuterNodeCompleted";
	}
}
