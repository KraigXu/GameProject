using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x0200116B RID: 4459
	public class QuestNode_DropPods : QuestNode
	{
		// Token: 0x060067B9 RID: 26553 RVA: 0x00243DE5 File Offset: 0x00241FE5
		protected override bool TestRunInt(Slate slate)
		{
			return slate.Exists("map", false);
		}

		// Token: 0x060067BA RID: 26554 RVA: 0x00243E70 File Offset: 0x00242070
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

		// Token: 0x04003FD2 RID: 16338
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x04003FD3 RID: 16339
		public SlateRef<IEnumerable<Thing>> contents;

		// Token: 0x04003FD4 RID: 16340
		public SlateRef<bool> useTradeDropSpot;

		// Token: 0x04003FD5 RID: 16341
		public SlateRef<bool> joinPlayer;

		// Token: 0x04003FD6 RID: 16342
		public SlateRef<bool> makePrisoners;

		// Token: 0x04003FD7 RID: 16343
		public SlateRef<bool?> sendStandardLetter;

		// Token: 0x04003FD8 RID: 16344
		public SlateRef<string> customLetterLabel;

		// Token: 0x04003FD9 RID: 16345
		public SlateRef<string> customLetterText;

		// Token: 0x04003FDA RID: 16346
		public SlateRef<RulePack> customLetterLabelRules;

		// Token: 0x04003FDB RID: 16347
		public SlateRef<RulePack> customLetterTextRules;

		// Token: 0x04003FDC RID: 16348
		public SlateRef<IEnumerable<Thing>> thingsToExcludeFromHyperlinks;

		// Token: 0x04003FDD RID: 16349
		private const string RootSymbol = "root";
	}
}
