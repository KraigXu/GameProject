using System;
using Verse;

namespace RimWorld.QuestGenNew
{
	
	public class QuestNode_GiveRoyalFavor : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		
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

		
		public SlateRef<Pawn> giveTo;

		
		public SlateRef<bool> giveToAccepter;

		
		[NoTranslate]
		public SlateRef<string> inSignal;

		
		public SlateRef<Faction> faction;

		
		public SlateRef<Thing> factionOf;

		
		public SlateRef<int> amount;

		
		public SlateRef<bool> isSingleReward;
	}
}
