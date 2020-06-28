using System;

namespace Verse.AI
{
	// Token: 0x020005B6 RID: 1462
	public class JobGiver_WanderNearFallbackLocation : JobGiver_Wander
	{
		// Token: 0x060028D9 RID: 10457 RVA: 0x000EFE7D File Offset: 0x000EE07D
		public JobGiver_WanderNearFallbackLocation()
		{
			this.wanderRadius = 7f;
			this.ticksBetweenWandersRange = new IntRange(125, 200);
		}

		// Token: 0x060028DA RID: 10458 RVA: 0x000EFEBF File Offset: 0x000EE0BF
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			return WanderUtility.BestCloseWanderRoot(pawn.mindState.duty.focusSecond.Cell, pawn);
		}
	}
}
