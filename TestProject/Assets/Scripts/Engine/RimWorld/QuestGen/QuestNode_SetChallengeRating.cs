using System;

namespace RimWorld.QuestGenNew
{
	
	public class QuestNode_SetChallengeRating : QuestNode
	{
		
		protected override void RunInt()
		{
			QuestGen.quest.challengeRating = this.challengeRating.GetValue(QuestGen.slate);
		}

		
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		
		public SlateRef<int> challengeRating;
	}
}
