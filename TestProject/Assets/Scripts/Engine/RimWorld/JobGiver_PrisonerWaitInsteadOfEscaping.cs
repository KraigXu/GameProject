using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020006E8 RID: 1768
	public class JobGiver_PrisonerWaitInsteadOfEscaping : JobGiver_Wander
	{
		// Token: 0x06002EFA RID: 12026 RVA: 0x00108668 File Offset: 0x00106868
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.guest == null || !pawn.guest.ShouldWaitInsteadOfEscaping)
			{
				return null;
			}
			Room room = pawn.GetRoom(RegionType.Set_Passable);
			if (room != null && room.isPrisonCell)
			{
				return null;
			}
			IntVec3 spotToWaitInsteadOfEscaping = pawn.guest.spotToWaitInsteadOfEscaping;
			if (!spotToWaitInsteadOfEscaping.IsValid || !pawn.CanReach(spotToWaitInsteadOfEscaping, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn))
			{
				if (!RCellFinder.TryFindRandomSpotJustOutsideColony(pawn, out spotToWaitInsteadOfEscaping))
				{
					return null;
				}
				pawn.guest.spotToWaitInsteadOfEscaping = spotToWaitInsteadOfEscaping;
			}
			return base.TryGiveJob(pawn);
		}

		// Token: 0x06002EFB RID: 12027 RVA: 0x001086E7 File Offset: 0x001068E7
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			return pawn.guest.spotToWaitInsteadOfEscaping;
		}
	}
}
