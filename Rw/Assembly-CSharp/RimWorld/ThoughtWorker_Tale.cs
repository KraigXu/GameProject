using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000824 RID: 2084
	public class ThoughtWorker_Tale : ThoughtWorker
	{
		// Token: 0x06003455 RID: 13397 RVA: 0x0011F868 File Offset: 0x0011DA68
		protected override ThoughtState CurrentSocialStateInternal(Pawn p, Pawn other)
		{
			if (!other.RaceProps.Humanlike)
			{
				return false;
			}
			if (!RelationsUtility.PawnsKnowEachOther(p, other))
			{
				return false;
			}
			if (Find.TaleManager.GetLatestTale(this.def.taleDef, other) == null)
			{
				return false;
			}
			return true;
		}
	}
}
