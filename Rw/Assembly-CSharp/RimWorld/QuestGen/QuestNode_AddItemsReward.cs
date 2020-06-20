using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200115C RID: 4444
	public class QuestNode_AddItemsReward : QuestNode
	{
		// Token: 0x0600678C RID: 26508 RVA: 0x0001028D File Offset: 0x0000E48D
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600678D RID: 26509 RVA: 0x002436E4 File Offset: 0x002418E4
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

		// Token: 0x04003FAA RID: 16298
		public SlateRef<IEnumerable<Thing>> items;

		// Token: 0x04003FAB RID: 16299
		[NoTranslate]
		public SlateRef<string> inSignalChoiceUsed;
	}
}
