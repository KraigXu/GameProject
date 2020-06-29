using System;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGenNew
{
	
	public class QuestNode_EndGame : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_EndGame endGame = new QuestPart_EndGame();
			endGame.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? slate.Get<string>("inSignal", null, false));
			endGame.signalListenMode = (this.signalListenMode.GetValue(slate) ?? QuestPart.SignalListenMode.OngoingOnly);
			QuestGen.AddTextRequest("root", delegate(string x)
			{
				endGame.introText = x;
			}, QuestGenUtility.MergeRules(this.introTextRules.GetValue(slate), this.introText.GetValue(slate), "root"));
			QuestGen.AddTextRequest("root", delegate(string x)
			{
				endGame.endingText = x;
			}, QuestGenUtility.MergeRules(this.endingTextRules.GetValue(slate), this.endingText.GetValue(slate), "root"));
			QuestGen.quest.AddPart(endGame);
		}

		
		[NoTranslate]
		public SlateRef<string> inSignal;

		
		public SlateRef<QuestPart.SignalListenMode?> signalListenMode;

		
		public SlateRef<string> introText;

		
		public SlateRef<string> endingText;

		
		public SlateRef<RulePack> introTextRules;

		
		public SlateRef<RulePack> endingTextRules;

		
		private const string RootSymbol = "root";
	}
}
