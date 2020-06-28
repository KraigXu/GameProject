using System;

namespace Verse.AI
{
	// Token: 0x020005AF RID: 1455
	public class JobGiver_WanderAnywhere : JobGiver_Wander
	{
		// Token: 0x060028CC RID: 10444 RVA: 0x000EFC48 File Offset: 0x000EDE48
		public JobGiver_WanderAnywhere()
		{
			this.wanderRadius = 7f;
			this.locomotionUrgency = LocomotionUrgency.Walk;
			this.ticksBetweenWandersRange = new IntRange(125, 200);
		}

		// Token: 0x060028CD RID: 10445 RVA: 0x000EFC74 File Offset: 0x000EDE74
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			return pawn.Position;
		}
	}
}
