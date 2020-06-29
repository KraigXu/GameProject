using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGenNew
{
	
	public class QuestNode_AnyPawnAlive : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		
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

		
		[NoTranslate]
		public SlateRef<string> inSignal;

		
		public SlateRef<List<Pawn>> pawns;

		
		public QuestNode node;

		
		private const string OuterNodeCompletedSignal = "OuterNodeCompleted";
	}
}
