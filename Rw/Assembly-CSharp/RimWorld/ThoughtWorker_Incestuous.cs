using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000810 RID: 2064
	public class ThoughtWorker_Incestuous : ThoughtWorker
	{
		// Token: 0x0600342D RID: 13357 RVA: 0x0011EF90 File Offset: 0x0011D190
		protected override ThoughtState CurrentSocialStateInternal(Pawn pawn, Pawn other)
		{
			if (!other.RaceProps.Humanlike)
			{
				return false;
			}
			if (!RelationsUtility.PawnsKnowEachOther(pawn, other))
			{
				return false;
			}
			if (LovePartnerRelationUtility.IncestOpinionOffsetFor(other, pawn) == 0f)
			{
				return false;
			}
			return true;
		}
	}
}
