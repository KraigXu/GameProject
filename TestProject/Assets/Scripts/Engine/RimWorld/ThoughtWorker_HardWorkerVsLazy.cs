using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000813 RID: 2067
	public class ThoughtWorker_HardWorkerVsLazy : ThoughtWorker
	{
		// Token: 0x06003433 RID: 13363 RVA: 0x0011F07C File Offset: 0x0011D27C
		protected override ThoughtState CurrentSocialStateInternal(Pawn p, Pawn other)
		{
			if (!p.RaceProps.Humanlike)
			{
				return false;
			}
			if (p.story.traits.DegreeOfTrait(TraitDefOf.Industriousness) <= 0)
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
			if (other.story.traits.DegreeOfTrait(TraitDefOf.Industriousness) > 0)
			{
				return false;
			}
			return true;
		}
	}
}
