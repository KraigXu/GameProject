using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x0200117F RID: 4479
	public class QuestNode_PawnsArrive : QuestNode
	{
		// Token: 0x060067FE RID: 26622 RVA: 0x00245B4D File Offset: 0x00243D4D
		protected override bool TestRunInt(Slate slate)
		{
			return slate.Exists("map", false);
		}

		// Token: 0x060067FF RID: 26623 RVA: 0x00245B60 File Offset: 0x00243D60
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			PawnsArrivalModeDef pawnsArrivalModeDef = this.arrivalMode.GetValue(slate) ?? PawnsArrivalModeDefOf.EdgeWalkIn;
			QuestPart_PawnsArrive pawnsArrive = new QuestPart_PawnsArrive();
			pawnsArrive.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			pawnsArrive.pawns.AddRange(this.pawns.GetValue(slate));
			pawnsArrive.arrivalMode = pawnsArrivalModeDef;
			pawnsArrive.joinPlayer = this.joinPlayer.GetValue(slate);
			pawnsArrive.mapParent = QuestGen.slate.Get<Map>("map", null, false).Parent;
			if (pawnsArrivalModeDef.walkIn)
			{
				pawnsArrive.spawnNear = (this.walkInSpot.GetValue(slate) ?? (QuestGen.slate.Get<IntVec3?>("walkInSpot", null, false) ?? IntVec3.Invalid));
			}
			if (!this.customLetterLabel.GetValue(slate).NullOrEmpty() || this.customLetterLabelRules.GetValue(slate) != null)
			{
				QuestGen.AddTextRequest("root", delegate(string x)
				{
					pawnsArrive.customLetterLabel = x;
				}, QuestGenUtility.MergeRules(this.customLetterLabelRules.GetValue(slate), this.customLetterLabel.GetValue(slate), "root"));
			}
			if (!this.customLetterText.GetValue(slate).NullOrEmpty() || this.customLetterTextRules.GetValue(slate) != null)
			{
				QuestGen.AddTextRequest("root", delegate(string x)
				{
					pawnsArrive.customLetterText = x;
				}, QuestGenUtility.MergeRules(this.customLetterTextRules.GetValue(slate), this.customLetterText.GetValue(slate), "root"));
			}
			QuestGen.quest.AddPart(pawnsArrive);
			if (this.isSingleReward.GetValue(slate))
			{
				QuestPart_Choice questPart_Choice = new QuestPart_Choice();
				questPart_Choice.inSignalChoiceUsed = pawnsArrive.inSignal;
				QuestPart_Choice.Choice choice = new QuestPart_Choice.Choice();
				choice.questParts.Add(pawnsArrive);
				foreach (Pawn pawn in pawnsArrive.pawns)
				{
					choice.rewards.Add(new Reward_Pawn
					{
						pawn = pawn,
						detailsHidden = this.rewardDetailsHidden.GetValue(slate)
					});
				}
				questPart_Choice.choices.Add(choice);
				QuestGen.quest.AddPart(questPart_Choice);
			}
		}

		// Token: 0x04004042 RID: 16450
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x04004043 RID: 16451
		public SlateRef<IEnumerable<Pawn>> pawns;

		// Token: 0x04004044 RID: 16452
		public SlateRef<PawnsArrivalModeDef> arrivalMode;

		// Token: 0x04004045 RID: 16453
		public SlateRef<bool> joinPlayer;

		// Token: 0x04004046 RID: 16454
		public SlateRef<IntVec3?> walkInSpot;

		// Token: 0x04004047 RID: 16455
		public SlateRef<string> customLetterLabel;

		// Token: 0x04004048 RID: 16456
		public SlateRef<string> customLetterText;

		// Token: 0x04004049 RID: 16457
		public SlateRef<RulePack> customLetterLabelRules;

		// Token: 0x0400404A RID: 16458
		public SlateRef<RulePack> customLetterTextRules;

		// Token: 0x0400404B RID: 16459
		public SlateRef<bool> isSingleReward;

		// Token: 0x0400404C RID: 16460
		public SlateRef<bool> rewardDetailsHidden;

		// Token: 0x0400404D RID: 16461
		private const string RootSymbol = "root";
	}
}
