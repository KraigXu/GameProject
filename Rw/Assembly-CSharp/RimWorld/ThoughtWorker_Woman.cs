using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000821 RID: 2081
	public class ThoughtWorker_Woman : ThoughtWorker
	{
		// Token: 0x0600344F RID: 13391 RVA: 0x0011F6DC File Offset: 0x0011D8DC
		protected override ThoughtState CurrentSocialStateInternal(Pawn p, Pawn other)
		{
			if (!p.RaceProps.Humanlike)
			{
				return false;
			}
			if (!p.story.traits.HasTrait(TraitDefOf.DislikesWomen))
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
			if (other.gender != Gender.Female)
			{
				return false;
			}
			return true;
		}
	}
}
