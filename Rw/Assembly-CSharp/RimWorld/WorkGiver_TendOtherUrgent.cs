using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000761 RID: 1889
	public class WorkGiver_TendOtherUrgent : WorkGiver_TendOther
	{
		// Token: 0x06003167 RID: 12647 RVA: 0x001137BC File Offset: 0x001119BC
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return base.HasJobOnThing(pawn, t, forced) && HealthAIUtility.ShouldBeTendedNowByPlayerUrgent((Pawn)t);
		}
	}
}
