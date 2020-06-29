using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	
	public class QuestNode_DropPods : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			return slate.Exists("map", false);
		}

		
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.contents.GetValue(slate) == null)
			{
				return;
			}
			QuestPart_DropPods dropPods = new QuestPart_DropPods();
			dropPods.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			if (!this.customLetterLabel.GetValue(slate).NullOrEmpty() || this.customLetterLabelRules.GetValue(slate) != null)
			{
				QuestGen.AddTextRequest("root", delegate(string x)
				{
					dropPods.customLetterLabel = x;
				}, QuestGenUtility.MergeRules(this.customLetterLabelRules.GetValue(slate), this.customLetterLabel.GetValue(slate), "root"));
			}
			if (!this.customLetterText.GetValue(slate).NullOrEmpty() || this.customLetterTextRules.GetValue(slate) != null)
			{
				QuestGen.AddTextRequest("root", delegate(string x)
				{
					dropPods.customLetterText = x;
				}, QuestGenUtility.MergeRules(this.customLetterTextRules.GetValue(slate), this.customLetterText.GetValue(slate), "root"));
			}
			dropPods.sendStandardLetter = (this.sendStandardLetter.GetValue(slate) ?? dropPods.sendStandardLetter);
			dropPods.useTradeDropSpot = this.useTradeDropSpot.GetValue(slate);
			dropPods.joinPlayer = this.joinPlayer.GetValue(slate);
			dropPods.makePrisoners = this.makePrisoners.GetValue(slate);
			dropPods.mapParent = QuestGen.slate.Get<Map>("map", null, false).Parent;
			dropPods.Things = this.contents.GetValue(slate);
			if (this.thingsToExcludeFromHyperlinks.GetValue(slate) != null)
			{
				dropPods.thingsToExcludeFromHyperlinks.AddRange(from t in this.thingsToExcludeFromHyperlinks.GetValue(slate)
				select t.GetInnerIfMinified().def);
			}
			QuestGen.quest.AddPart(dropPods);
		}

		
		[NoTranslate]
		public SlateRef<string> inSignal;

		
		public SlateRef<IEnumerable<Thing>> contents;

		
		public SlateRef<bool> useTradeDropSpot;

		
		public SlateRef<bool> joinPlayer;

		
		public SlateRef<bool> makePrisoners;

		
		public SlateRef<bool?> sendStandardLetter;

		
		public SlateRef<string> customLetterLabel;

		
		public SlateRef<string> customLetterText;

		
		public SlateRef<RulePack> customLetterLabelRules;

		
		public SlateRef<RulePack> customLetterTextRules;

		
		public SlateRef<IEnumerable<Thing>> thingsToExcludeFromHyperlinks;

		
		private const string RootSymbol = "root";
	}
}
