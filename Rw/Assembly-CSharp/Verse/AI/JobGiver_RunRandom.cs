using System;

namespace Verse.AI
{
	// Token: 0x020005B8 RID: 1464
	public class JobGiver_RunRandom : JobGiver_Wander
	{
		// Token: 0x060028DF RID: 10463 RVA: 0x000EFF6C File Offset: 0x000EE16C
		public JobGiver_RunRandom()
		{
			this.wanderRadius = 7f;
			this.ticksBetweenWandersRange = new IntRange(5, 10);
			this.locomotionUrgency = LocomotionUrgency.Sprint;
		}

		// Token: 0x060028E0 RID: 10464 RVA: 0x000EFC74 File Offset: 0x000EDE74
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			return pawn.Position;
		}
	}
}
