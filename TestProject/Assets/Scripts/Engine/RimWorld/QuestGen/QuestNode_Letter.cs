using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGenNew
{
	
	public class QuestNode_Letter : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_Letter questPart_Letter = new QuestPart_Letter();
			questPart_Letter.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? slate.Get<string>("inSignal", null, false));
			LetterDef letterDef = this.letterDef.GetValue(slate) ?? LetterDefOf.NeutralEvent;
			if (typeof(ChoiceLetter).IsAssignableFrom(letterDef.letterClass))
			{
				ChoiceLetter choiceLetter = LetterMaker.MakeLetter("error", "error", letterDef, QuestGenUtility.ToLookTargets(this.lookTargets, slate), this.relatedFaction.GetValue(slate), QuestGen.quest, null);
				questPart_Letter.letter = choiceLetter;
				QuestGen.AddTextRequest("root", delegate(string x)
				{
					choiceLetter.label = x;
				}, QuestGenUtility.MergeRules(this.labelRules.GetValue(slate), this.label.GetValue(slate), "root"));
				QuestGen.AddTextRequest("root", delegate(string x)
				{
					choiceLetter.text = x;
				}, QuestGenUtility.MergeRules(this.textRules.GetValue(slate), this.text.GetValue(slate), "root"));
			}
			else
			{
				questPart_Letter.letter = LetterMaker.MakeLetter(letterDef);
				questPart_Letter.letter.lookTargets = QuestGenUtility.ToLookTargets(this.lookTargets, slate);
				questPart_Letter.letter.relatedFaction = this.relatedFaction.GetValue(slate);
			}
			questPart_Letter.chosenPawnSignal = QuestGenUtility.HardcodedSignalWithQuestID(this.chosenPawnSignal.GetValue(slate));
			questPart_Letter.useColonistsOnMap = this.useColonistsOnMap.GetValue(slate);
			questPart_Letter.useColonistsFromCaravanArg = this.useColonistsFromCaravanArg.GetValue(slate);
			questPart_Letter.signalListenMode = (this.signalListenMode.GetValue(slate) ?? QuestPart.SignalListenMode.OngoingOnly);
			questPart_Letter.filterDeadPawnsFromLookTargets = this.filterDeadPawnsFromLookTargets.GetValue(slate);
			QuestGen.quest.AddPart(questPart_Letter);
		}

		
		[NoTranslate]
		public SlateRef<string> inSignal;

		
		public SlateRef<Faction> relatedFaction;

		
		public SlateRef<LetterDef> letterDef;

		
		public SlateRef<string> label;

		
		public SlateRef<string> text;

		
		public SlateRef<RulePack> labelRules;

		
		public SlateRef<RulePack> textRules;

		
		public SlateRef<IEnumerable<object>> lookTargets;

		
		public SlateRef<QuestPart.SignalListenMode?> signalListenMode;

		
		[NoTranslate]
		public SlateRef<string> chosenPawnSignal;

		
		public SlateRef<MapParent> useColonistsOnMap;

		
		public SlateRef<bool> useColonistsFromCaravanArg;

		
		public SlateRef<bool> filterDeadPawnsFromLookTargets;

		
		private const string RootSymbol = "root";
	}
}
