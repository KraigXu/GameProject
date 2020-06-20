using System;

namespace RimWorld.QuestGen
{
	// Token: 0x0200118B RID: 4491
	public class QuestNode_SetChallengeRating : QuestNode
	{
		// Token: 0x06006824 RID: 26660 RVA: 0x002465A6 File Offset: 0x002447A6
		protected override void RunInt()
		{
			QuestGen.quest.challengeRating = this.challengeRating.GetValue(QuestGen.slate);
		}

		// Token: 0x06006825 RID: 26661 RVA: 0x0001028D File Offset: 0x0000E48D
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x04004071 RID: 16497
		public SlateRef<int> challengeRating;
	}
}
