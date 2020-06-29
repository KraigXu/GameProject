using System;
using Verse;

namespace RimWorld.QuestGen
{
	
	public class QuestNode_AddPawnReward : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		
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

		
		public SlateRef<Pawn> pawn;

		
		[NoTranslate]
		public SlateRef<string> inSignalChoiceUsed;

		
		public SlateRef<bool> rewardDetailsHidden;
	}
}
