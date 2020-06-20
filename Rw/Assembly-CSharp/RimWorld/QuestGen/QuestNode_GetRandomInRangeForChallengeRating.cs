using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200113E RID: 4414
	public class QuestNode_GetRandomInRangeForChallengeRating : QuestNode
	{
		// Token: 0x06006717 RID: 26391 RVA: 0x00241710 File Offset: 0x0023F910
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			float randomInRange = this.GetRangeFromRating().RandomInRange;
			slate.Set<float>(this.storeAs.GetValue(slate), this.roundRandom.GetValue(slate) ? ((float)GenMath.RoundRandom(randomInRange)) : randomInRange, false);
		}

		// Token: 0x06006718 RID: 26392 RVA: 0x00241760 File Offset: 0x0023F960
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

		// Token: 0x06006719 RID: 26393 RVA: 0x002417AC File Offset: 0x0023F9AC
		protected override bool TestRunInt(Slate slate)
		{
			slate.Set<int>(this.storeAs.GetValue(slate), 0, false);
			return true;
		}

		// Token: 0x04003F3B RID: 16187
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x04003F3C RID: 16188
		public SlateRef<FloatRange> oneStarRange;

		// Token: 0x04003F3D RID: 16189
		public SlateRef<FloatRange> twoStarRange;

		// Token: 0x04003F3E RID: 16190
		public SlateRef<FloatRange> threeStarRange;

		// Token: 0x04003F3F RID: 16191
		public SlateRef<bool> roundRandom;
	}
}
