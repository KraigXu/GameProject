using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D96 RID: 3478
	public class CompTargetEffect_MoodBoost : CompTargetEffect
	{
		// Token: 0x0600549D RID: 21661 RVA: 0x001C3738 File Offset: 0x001C1938
		public override void DoEffectOn(Pawn user, Thing target)
		{
			Pawn pawn = (Pawn)target;
			if (pawn.Dead || pawn.needs == null || pawn.needs.mood == null)
			{
				return;
			}
			pawn.needs.mood.thoughts.memories.TryGainMemory((Thought_Memory)ThoughtMaker.MakeThought(ThoughtDefOf.ArtifactMoodBoost), null);
		}
	}
}
