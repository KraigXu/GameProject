using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200115E RID: 4446
	public class QuestNode_AddPassageOffworldReward : QuestNode
	{
		// Token: 0x06006792 RID: 26514 RVA: 0x0001028D File Offset: 0x0000E48D
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x06006793 RID: 26515 RVA: 0x00243860 File Offset: 0x00241A60
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_Choice questPart_Choice = new QuestPart_Choice();
			questPart_Choice.inSignalChoiceUsed = QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalChoiceUsed.GetValue(slate));
			QuestPart_Choice.Choice choice = new QuestPart_Choice.Choice();
			choice.rewards.Add(new Reward_PassageOffworld());
			questPart_Choice.choices.Add(choice);
			QuestGen.quest.AddPart(questPart_Choice);
		}

		// Token: 0x04003FB1 RID: 16305
		[NoTranslate]
		public SlateRef<string> inSignalChoiceUsed;
	}
}
