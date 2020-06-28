using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000812 RID: 2066
	public class ThoughtWorker_Disfigured : ThoughtWorker
	{
		// Token: 0x06003431 RID: 13361 RVA: 0x0011F010 File Offset: 0x0011D210
		protected override ThoughtState CurrentSocialStateInternal(Pawn pawn, Pawn other)
		{
			if (!other.RaceProps.Humanlike || other.Dead)
			{
				return false;
			}
			if (!RelationsUtility.PawnsKnowEachOther(pawn, other))
			{
				return false;
			}
			if (!RelationsUtility.IsDisfigured(other))
			{
				return false;
			}
			if (!pawn.health.capacities.CapableOf(PawnCapacityDefOf.Sight))
			{
				return false;
			}
			return true;
		}
	}
}
