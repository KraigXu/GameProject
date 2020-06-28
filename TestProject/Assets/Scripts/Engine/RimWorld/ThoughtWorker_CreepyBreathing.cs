using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200081F RID: 2079
	public class ThoughtWorker_CreepyBreathing : ThoughtWorker
	{
		// Token: 0x0600344B RID: 13387 RVA: 0x0011F5F4 File Offset: 0x0011D7F4
		protected override ThoughtState CurrentSocialStateInternal(Pawn pawn, Pawn other)
		{
			if (!other.RaceProps.Humanlike || !RelationsUtility.PawnsKnowEachOther(pawn, other))
			{
				return false;
			}
			if (!other.story.traits.HasTrait(TraitDefOf.CreepyBreathing))
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
