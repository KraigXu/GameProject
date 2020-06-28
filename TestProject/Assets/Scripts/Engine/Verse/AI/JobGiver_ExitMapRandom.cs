using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x0200059F RID: 1439
	public class JobGiver_ExitMapRandom : JobGiver_ExitMap
	{
		// Token: 0x06002893 RID: 10387 RVA: 0x000EF3E4 File Offset: 0x000ED5E4
		protected override bool TryFindGoodExitDest(Pawn pawn, bool canDig, out IntVec3 spot)
		{
			TraverseMode mode = canDig ? TraverseMode.PassAllDestroyableThings : TraverseMode.ByPawn;
			return RCellFinder.TryFindRandomExitSpot(pawn, out spot, mode);
		}
	}
}
