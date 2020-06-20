using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001172 RID: 4466
	public class QuestNode_GiveRoyalFavor : QuestNode
	{
		// Token: 0x060067D6 RID: 26582 RVA: 0x0001028D File Offset: 0x0000E48D
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x060067D7 RID: 26583 RVA: 0x00244FFC File Offset: 0x002431FC
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_GiveRoyalFavor questPart_GiveRoyalFavor = new QuestPart_GiveRoyalFavor();
			questPart_GiveRoyalFavor.giveTo = this.giveTo.GetValue(slate);
			questPart_GiveRoyalFavor.giveToAccepter = this.giveToAccepter.GetValue(slate);
			questPart_GiveRoyalFavor.faction = (this.faction.GetValue(slate) ?? this.factionOf.GetValue(slate).Faction);
			questPart_GiveRoyalFavor.amount = this.amount.GetValue(slate);
			questPart_GiveRoyalFavor.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			QuestGen.quest.AddPart(questPart_GiveRoyalFavor);
			if (this.isSingleReward.GetValue(slate))
			{
				QuestPart_Choice questPart_Choice = new QuestPart_Choice();
				questPart_Choice.inSignalChoiceUsed = questPart_GiveRoyalFavor.inSignal;
				QuestPart_Choice.Choice choice = new QuestPart_Choice.Choice();
				choice.questParts.Add(questPart_GiveRoyalFavor);
				choice.rewards.Add(new Reward_RoyalFavor
				{
					faction = questPart_GiveRoyalFavor.faction,
					amount = questPart_GiveRoyalFavor.amount
				});
				questPart_Choice.choices.Add(choice);
				QuestGen.quest.AddPart(questPart_Choice);
			}
		}

		// Token: 0x04004003 RID: 16387
		public SlateRef<Pawn> giveTo;

		// Token: 0x04004004 RID: 16388
		public SlateRef<bool> giveToAccepter;

		// Token: 0x04004005 RID: 16389
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x04004006 RID: 16390
		public SlateRef<Faction> faction;

		// Token: 0x04004007 RID: 16391
		public SlateRef<Thing> factionOf;

		// Token: 0x04004008 RID: 16392
		public SlateRef<int> amount;

		// Token: 0x04004009 RID: 16393
		public SlateRef<bool> isSingleReward;
	}
}
