using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000563 RID: 1379
	public class MentalStateWorker_WanderOwnRoom : MentalStateWorker
	{
		// Token: 0x06002728 RID: 10024 RVA: 0x000E51B0 File Offset: 0x000E33B0
		public override bool StateCanOccur(Pawn pawn)
		{
			if (!base.StateCanOccur(pawn))
			{
				return false;
			}
			Building_Bed ownedBed = pawn.ownership.OwnedBed;
			return ownedBed != null && ownedBed.GetRoom(RegionType.Set_Passable) != null && !ownedBed.GetRoom(RegionType.Set_Passable).PsychologicallyOutdoors;
		}
	}
}
