using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000823 RID: 2083
	public class ThoughtWorker_BodyPuristDisgust : ThoughtWorker
	{
		// Token: 0x06003453 RID: 13395 RVA: 0x0011F7E0 File Offset: 0x0011D9E0
		protected override ThoughtState CurrentSocialStateInternal(Pawn p, Pawn other)
		{
			if (!p.RaceProps.Humanlike)
			{
				return false;
			}
			if (!p.story.traits.HasTrait(TraitDefOf.BodyPurist))
			{
				return false;
			}
			if (!RelationsUtility.PawnsKnowEachOther(p, other))
			{
				return false;
			}
			if (other.def != p.def)
			{
				return false;
			}
			int num = other.health.hediffSet.CountAddedAndImplantedParts();
			if (num > 0)
			{
				return ThoughtState.ActiveAtStage(num - 1);
			}
			return false;
		}
	}
}
