using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000806 RID: 2054
	public class ThoughtWorker_OpinionOfMyLover : ThoughtWorker
	{
		// Token: 0x06003413 RID: 13331 RVA: 0x0011EA24 File Offset: 0x0011CC24
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
			return p.relations.OpinionOf(directPawnRelation.otherPawn) != 0;
		}
	}
}
