using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200115F RID: 4447
	public class QuestNode_AddPawnReward : QuestNode
	{
		// Token: 0x06006795 RID: 26517 RVA: 0x0001028D File Offset: 0x0000E48D
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x06006796 RID: 26518 RVA: 0x002438C0 File Offset: 0x00241AC0
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			Pawn value = this.pawn.GetValue(slate);
			if (value == null)
			{
				return;
			}
			QuestPart_Choice questPart_Choice = new QuestPart_Choice();
			questPart_Choice.inSignalChoiceUsed = QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalChoiceUsed.GetValue(slate));
			QuestPart_Choice.Choice choice = new QuestPart_Choice.Choice();
			choice.rewards.Add(new Reward_Pawn
			{
				pawn = value,
				detailsHidden = this.rewardDetailsHidden.GetValue(slate)
			});
			questPart_Choice.choices.Add(choice);
			QuestGen.quest.AddPart(questPart_Choice);
		}

		// Token: 0x04003FB2 RID: 16306
		public SlateRef<Pawn> pawn;

		// Token: 0x04003FB3 RID: 16307
		[NoTranslate]
		public SlateRef<string> inSignalChoiceUsed;

		// Token: 0x04003FB4 RID: 16308
		public SlateRef<bool> rewardDetailsHidden;
	}
}
