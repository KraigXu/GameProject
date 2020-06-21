using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200122E RID: 4654
	public static class CaravanCarryUtility
	{
		// Token: 0x06006C5D RID: 27741 RVA: 0x0025C610 File Offset: 0x0025A810
		public static bool CarriedByCaravan(this Pawn p)
		{
			Caravan caravan = p.GetCaravan();
			return caravan != null && caravan.carryTracker.IsCarried(p);
		}

		// Token: 0x06006C5E RID: 27742 RVA: 0x0025C635 File Offset: 0x0025A835
		public static bool WouldBenefitFromBeingCarried(Pawn p)
		{
			return CaravanBedUtility.WouldBenefitFromRestingInBed(p);
		}
	}
}
