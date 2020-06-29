using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGenNew
{
	
	public class QuestNode_AddItemsReward : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			IEnumerable<Thing> value = this.items.GetValue(slate);
			if (value.EnumerableNullOrEmpty<Thing>())
			{
				return;
			}
			QuestPart_Choice questPart_Choice = new QuestPart_Choice();
			questPart_Choice.inSignalChoiceUsed = QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalChoiceUsed.GetValue(slate));
			QuestPart_Choice.Choice choice = new QuestPart_Choice.Choice();
			Reward_Items reward_Items = new Reward_Items();
			reward_Items.items.AddRange(value);
			choice.rewards.Add(reward_Items);
			questPart_Choice.choices.Add(choice);
			QuestGen.quest.AddPart(questPart_Choice);
		}

		
		public SlateRef<IEnumerable<Thing>> items;

		
		[NoTranslate]
		public SlateRef<string> inSignalChoiceUsed;
	}
}
