using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000834 RID: 2100
	public class ThoughtWorker_SharedBed : ThoughtWorker
	{
		// Token: 0x06003477 RID: 13431 RVA: 0x0011FEFE File Offset: 0x0011E0FE
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!p.RaceProps.Humanlike)
			{
				return false;
			}
			return LovePartnerRelationUtility.GetMostDislikedNonPartnerBedOwner(p) != null;
		}
	}
}
