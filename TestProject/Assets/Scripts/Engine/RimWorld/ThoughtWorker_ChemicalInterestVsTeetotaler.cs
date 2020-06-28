using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000819 RID: 2073
	public class ThoughtWorker_ChemicalInterestVsTeetotaler : ThoughtWorker
	{
		// Token: 0x0600343F RID: 13375 RVA: 0x0011F300 File Offset: 0x0011D500
		protected override ThoughtState CurrentSocialStateInternal(Pawn p, Pawn other)
		{
			if (!p.RaceProps.Humanlike)
			{
				return false;
			}
			if (p.story.traits.DegreeOfTrait(TraitDefOf.DrugDesire) <= 0)
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
			if (other.story.traits.DegreeOfTrait(TraitDefOf.DrugDesire) >= 0)
			{
				return false;
			}
			return true;
		}
	}
}
