using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x020005B3 RID: 1459
	public class JobGiver_WanderMapEdge : JobGiver_Wander
	{
		// Token: 0x060028D3 RID: 10451 RVA: 0x000EFDAF File Offset: 0x000EDFAF
		public JobGiver_WanderMapEdge()
		{
			this.wanderRadius = 7f;
			this.ticksBetweenWandersRange = new IntRange(50, 125);
		}

		// Token: 0x060028D4 RID: 10452 RVA: 0x000EFDD4 File Offset: 0x000EDFD4
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			IntVec3 result;
			if (RCellFinder.TryFindBestExitSpot(pawn, out result, TraverseMode.ByPawn))
			{
				return result;
			}
			return pawn.Position;
		}
	}
}
