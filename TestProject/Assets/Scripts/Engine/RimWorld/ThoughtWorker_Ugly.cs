using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200081D RID: 2077
	public class ThoughtWorker_Ugly : ThoughtWorker
	{
		// Token: 0x06003447 RID: 13383 RVA: 0x0011F50C File Offset: 0x0011D70C
		protected override ThoughtState CurrentSocialStateInternal(Pawn pawn, Pawn other)
		{
			if (!other.RaceProps.Humanlike || !RelationsUtility.PawnsKnowEachOther(pawn, other))
			{
				return false;
			}
			if (!pawn.health.capacities.CapableOf(PawnCapacityDefOf.Sight))
			{
				return false;
			}
			float statValue = other.GetStatValue(StatDefOf.PawnBeauty, true);
			if (statValue <= -2f)
			{
				return ThoughtState.ActiveAtStage(1);
			}
			if (statValue <= -1f)
			{
				return ThoughtState.ActiveAtStage(0);
			}
			return false;
		}
	}
}
