using System;

namespace Verse.AI
{
	// Token: 0x020005B0 RID: 1456
	public class JobGiver_WanderCurrentRoom : JobGiver_Wander
	{
		// Token: 0x060028CE RID: 10446 RVA: 0x000EFC7C File Offset: 0x000EDE7C
		public JobGiver_WanderCurrentRoom()
		{
			this.wanderRadius = 7f;
			this.ticksBetweenWandersRange = new IntRange(125, 200);
			this.locomotionUrgency = LocomotionUrgency.Amble;
			this.wanderDestValidator = ((Pawn pawn, IntVec3 loc, IntVec3 root) => WanderRoomUtility.IsValidWanderDest(pawn, loc, root));
		}

		// Token: 0x060028CF RID: 10447 RVA: 0x000EFC74 File Offset: 0x000EDE74
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			return pawn.Position;
		}
	}
}
