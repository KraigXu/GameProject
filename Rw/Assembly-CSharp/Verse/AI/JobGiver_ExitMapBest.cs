using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x020005A0 RID: 1440
	public class JobGiver_ExitMapBest : JobGiver_ExitMap
	{
		// Token: 0x06002895 RID: 10389 RVA: 0x000EF40C File Offset: 0x000ED60C
		protected override bool TryFindGoodExitDest(Pawn pawn, bool canDig, out IntVec3 spot)
		{
			TraverseMode mode = canDig ? TraverseMode.PassAllDestroyableThings : TraverseMode.ByPawn;
			return RCellFinder.TryFindBestExitSpot(pawn, out spot, mode);
		}
	}
}
