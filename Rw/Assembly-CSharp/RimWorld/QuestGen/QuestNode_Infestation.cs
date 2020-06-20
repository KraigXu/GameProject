using System;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x020011AC RID: 4524
	public class QuestNode_Infestation : QuestNode
	{
		// Token: 0x06006898 RID: 26776 RVA: 0x002484C8 File Offset: 0x002466C8
		protected override bool TestRunInt(Slate slate)
		{
			if (!Find.Storyteller.difficulty.allowViolentQuests)
			{
				return false;
			}
			if (!slate.Exists("map", false))
			{
				return false;
			}
			Map map = slate.Get<Map>("map", null, false);
			IntVec3 intVec;
			return InfestationCellFinder.TryFindCell(out intVec, map);
		}

		// Token: 0x06006899 RID: 26777 RVA: 0x00248514 File Offset: 0x00246714
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			Map map = QuestGen.slate.Get<Map>("map", null, false);
			if (map == null)
			{
				return;
			}
			QuestPart_Infestation questPart = new QuestPart_Infestation();
			questPart.mapParent = map.Parent;
			questPart.hivesCount = this.hivesCount.GetValue(slate);
			questPart.tag = QuestGenUtility.HardcodedTargetQuestTagWithQuestID(this.tag.GetValue(slate));
			if (!this.customLetterLabel.GetValue(slate).NullOrEmpty() || this.customLetterLabelRules.GetValue(slate) != null)
			{
				QuestGen.AddTextRequest("root", delegate(string x)
				{
					questPart.customLetterLabel = x;
				}, QuestGenUtility.MergeRules(this.customLetterLabelRules.GetValue(slate), this.customLetterLabel.GetValue(slate), "root"));
			}
			if (!this.customLetterText.GetValue(slate).NullOrEmpty() || this.customLetterTextRules.GetValue(slate) != null)
			{
				QuestGen.AddTextRequest("root", delegate(string x)
				{
					questPart.customLetterText = x;
				}, QuestGenUtility.MergeRules(this.customLetterTextRules.GetValue(slate), this.customLetterText.GetValue(slate), "root"));
			}
			questPart.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			QuestGen.quest.AddPart(questPart);
		}

		// Token: 0x040040F1 RID: 16625
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x040040F2 RID: 16626
		public SlateRef<int> hivesCount;

		// Token: 0x040040F3 RID: 16627
		public SlateRef<string> tag;

		// Token: 0x040040F4 RID: 16628
		public SlateRef<string> customLetterLabel;

		// Token: 0x040040F5 RID: 16629
		public SlateRef<string> customLetterText;

		// Token: 0x040040F6 RID: 16630
		public SlateRef<RulePack> customLetterLabelRules;

		// Token: 0x040040F7 RID: 16631
		public SlateRef<RulePack> customLetterTextRules;

		// Token: 0x040040F8 RID: 16632
		private const string RootSymbol = "root";
	}
}
