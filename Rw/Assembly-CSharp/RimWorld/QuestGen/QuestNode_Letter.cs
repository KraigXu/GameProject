using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x02001179 RID: 4473
	public class QuestNode_Letter : QuestNode
	{
		// Token: 0x060067EC RID: 26604 RVA: 0x0001028D File Offset: 0x0000E48D
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x060067ED RID: 26605 RVA: 0x002455B4 File Offset: 0x002437B4
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

		// Token: 0x0400401F RID: 16415
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x04004020 RID: 16416
		public SlateRef<Faction> relatedFaction;

		// Token: 0x04004021 RID: 16417
		public SlateRef<LetterDef> letterDef;

		// Token: 0x04004022 RID: 16418
		public SlateRef<string> label;

		// Token: 0x04004023 RID: 16419
		public SlateRef<string> text;

		// Token: 0x04004024 RID: 16420
		public SlateRef<RulePack> labelRules;

		// Token: 0x04004025 RID: 16421
		public SlateRef<RulePack> textRules;

		// Token: 0x04004026 RID: 16422
		public SlateRef<IEnumerable<object>> lookTargets;

		// Token: 0x04004027 RID: 16423
		public SlateRef<QuestPart.SignalListenMode?> signalListenMode;

		// Token: 0x04004028 RID: 16424
		[NoTranslate]
		public SlateRef<string> chosenPawnSignal;

		// Token: 0x04004029 RID: 16425
		public SlateRef<MapParent> useColonistsOnMap;

		// Token: 0x0400402A RID: 16426
		public SlateRef<bool> useColonistsFromCaravanArg;

		// Token: 0x0400402B RID: 16427
		public SlateRef<bool> filterDeadPawnsFromLookTargets;

		// Token: 0x0400402C RID: 16428
		private const string RootSymbol = "root";
	}
}
