using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200081C RID: 2076
	public class ThoughtWorker_Pretty : ThoughtWorker
	{
		// Token: 0x06003445 RID: 13381 RVA: 0x0011F484 File Offset: 0x0011D684
		protected override ThoughtState CurrentSocialStateInternal(Pawn pawn, Pawn other)
		{
			if (!other.RaceProps.Humanlike || !RelationsUtility.PawnsKnowEachOther(pawn, other))
			{
				return false;
			}
			if (RelationsUtility.IsDisfigured(other))
			{
				return false;
			}
			if (!pawn.health.capacities.CapableOf(PawnCapacityDefOf.Sight))
			{
				return false;
			}
			float statValue = other.GetStatValue(StatDefOf.PawnBeauty, true);
			if (statValue >= 2f)
			{
				return ThoughtState.ActiveAtStage(1);
			}
			if (statValue >= 1f)
			{
				return ThoughtState.ActiveAtStage(0);
			}
			return false;
		}
	}
}
