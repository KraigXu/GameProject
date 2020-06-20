using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200080F RID: 2063
	public class ThoughtWorker_Affair : ThoughtWorker
	{
		// Token: 0x0600342B RID: 13355 RVA: 0x0011EEF4 File Offset: 0x0011D0F4
		protected override ThoughtState CurrentSocialStateInternal(Pawn p, Pawn otherPawn)
		{
			if (!p.relations.DirectRelationExists(PawnRelationDefOf.Spouse, otherPawn))
			{
				return false;
			}
			List<DirectPawnRelation> directRelations = otherPawn.relations.DirectRelations;
			for (int i = 0; i < directRelations.Count; i++)
			{
				if (directRelations[i].otherPawn != p && !directRelations[i].otherPawn.Dead && (directRelations[i].def == PawnRelationDefOf.Lover || directRelations[i].def == PawnRelationDefOf.Fiance))
				{
					return true;
				}
			}
			return false;
		}
	}
}
