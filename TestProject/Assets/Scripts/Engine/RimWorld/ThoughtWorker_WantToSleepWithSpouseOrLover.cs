using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200080D RID: 2061
	public class ThoughtWorker_WantToSleepWithSpouseOrLover : ThoughtWorker
	{
		// Token: 0x06003426 RID: 13350 RVA: 0x0011EDA0 File Offset: 0x0011CFA0
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			DirectPawnRelation directPawnRelation = LovePartnerRelationUtility.ExistingMostLikedLovePartnerRel(p, false);
			if (directPawnRelation == null)
			{
				return false;
			}
			if (!directPawnRelation.otherPawn.IsColonist || directPawnRelation.otherPawn.IsWorldPawn() || !directPawnRelation.otherPawn.relations.everSeenByPlayer)
			{
				return false;
			}
			if (p.ownership.OwnedBed != null && p.ownership.OwnedBed == directPawnRelation.otherPawn.ownership.OwnedBed)
			{
				return false;
			}
			if (p.relations.OpinionOf(directPawnRelation.otherPawn) <= 0)
			{
				return false;
			}
			return true;
		}
	}
}
