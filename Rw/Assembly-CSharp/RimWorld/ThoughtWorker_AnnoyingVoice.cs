using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200081E RID: 2078
	public class ThoughtWorker_AnnoyingVoice : ThoughtWorker
	{
		// Token: 0x06003449 RID: 13385 RVA: 0x0011F588 File Offset: 0x0011D788
		protected override ThoughtState CurrentSocialStateInternal(Pawn pawn, Pawn other)
		{
			if (!other.RaceProps.Humanlike || !RelationsUtility.PawnsKnowEachOther(pawn, other))
			{
				return false;
			}
			if (!other.story.traits.HasTrait(TraitDefOf.AnnoyingVoice))
			{
				return false;
			}
			if (!pawn.health.capacities.CapableOf(PawnCapacityDefOf.Hearing))
			{
				return false;
			}
			return true;
		}
	}
}
