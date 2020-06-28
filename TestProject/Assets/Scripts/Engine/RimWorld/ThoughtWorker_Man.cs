using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000820 RID: 2080
	public class ThoughtWorker_Man : ThoughtWorker
	{
		// Token: 0x0600344D RID: 13389 RVA: 0x0011F660 File Offset: 0x0011D860
		protected override ThoughtState CurrentSocialStateInternal(Pawn p, Pawn other)
		{
			if (!p.RaceProps.Humanlike)
			{
				return false;
			}
			if (!p.story.traits.HasTrait(TraitDefOf.DislikesMen))
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
			if (other.gender != Gender.Male)
			{
				return false;
			}
			return true;
		}
	}
}
