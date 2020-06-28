using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000816 RID: 2070
	public class ThoughtWorker_TeetotalerVsChemicalInterest : ThoughtWorker
	{
		// Token: 0x06003439 RID: 13369 RVA: 0x0011F190 File Offset: 0x0011D390
		protected override ThoughtState CurrentSocialStateInternal(Pawn p, Pawn other)
		{
			if (!p.RaceProps.Humanlike)
			{
				return false;
			}
			if (!p.IsTeetotaler())
			{
				return false;
			}
			if (!other.RaceProps.Humanlike)
			{
				return false;
			}
			if (!RelationsUtility.PawnsKnowEachOther(p, other))
			{
				return false;
			}
			if (other.story.traits.DegreeOfTrait(TraitDefOf.DrugDesire) <= 0)
			{
				return false;
			}
			return true;
		}
	}
}
