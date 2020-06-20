using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200084E RID: 2126
	public class ThoughtWorker_NoThroneroom : ThoughtWorker
	{
		// Token: 0x060034B8 RID: 13496 RVA: 0x00120C24 File Offset: 0x0011EE24
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (p.royalty == null || p.MapHeld == null || !p.MapHeld.IsPlayerHome || p.royalty.HighestTitleWithThroneRoomRequirements() == null)
			{
				return false;
			}
			return p.ownership.AssignedThrone == null;
		}
	}
}
