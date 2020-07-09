using System;
using Verse;

namespace RimWorld.QuestGen
{
	
	public class QuestNode_GetRandomInRangeForChallengeRating : QuestNode
	{
		
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			float randomInRange = this.GetRangeFromRating().RandomInRange;
			slate.Set<float>(this.storeAs.GetValue(slate), this.roundRandom.GetValue(slate) ? ((float)GenMath.RoundRandom(randomInRange)) : randomInRange, false);
		}

		
		public FloatRange GetRangeFromRating()
		{
			int challengeRating = QuestGen.quest.challengeRating;
			Slate slate = QuestGen.slate;
			if (challengeRating == 3)
			{
				return this.threeStarRange.GetValue(slate);
			}
			if (challengeRating == 2)
			{
				return this.twoStarRange.GetValue(slate);
			}
			return this.oneStarRange.GetValue(slate);
		}

		
		protected override bool TestRunInt(Slate slate)
		{
			slate.Set<int>(this.storeAs.GetValue(slate), 0, false);
			return true;
		}

		
		[NoTranslate]
		public SlateRef<string> storeAs;

		
		public SlateRef<FloatRange> oneStarRange;

		
		public SlateRef<FloatRange> twoStarRange;

		
		public SlateRef<FloatRange> threeStarRange;

		
		public SlateRef<bool> roundRandom;
	}
}
