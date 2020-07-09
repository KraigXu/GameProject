using System;
using Verse;

namespace RimWorld.QuestGen
{
	
	public class QuestNode_SignalFactionNonPlayer : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			return this.node == null || this.node.TestRun(slate);
		}

		
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

		
		[NoTranslate]
		public SlateRef<string> inSignal;

		
		public QuestNode node;

		
		private const string OuterNodeCompletedSignal = "OuterNodeCompleted";
	}
}
