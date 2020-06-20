using System;

namespace Verse.AI
{
	// Token: 0x020005B5 RID: 1461
	public class JobGiver_WanderNearDutyLocation : JobGiver_Wander
	{
		// Token: 0x060028D7 RID: 10455 RVA: 0x000EFE7D File Offset: 0x000EE07D
		public JobGiver_WanderNearDutyLocation()
		{
			this.wanderRadius = 7f;
			this.ticksBetweenWandersRange = new IntRange(125, 200);
		}

		// Token: 0x060028D8 RID: 10456 RVA: 0x000EFEA2 File Offset: 0x000EE0A2
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			return WanderUtility.BestCloseWanderRoot(pawn.mindState.duty.focus.Cell, pawn);
		}
	}
}
