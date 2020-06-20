using System;

namespace Verse.AI
{
	// Token: 0x020005B4 RID: 1460
	public class JobGiver_WanderOwnRoom : JobGiver_Wander
	{
		// Token: 0x060028D5 RID: 10453 RVA: 0x000EFDF4 File Offset: 0x000EDFF4
		public JobGiver_WanderOwnRoom()
		{
			this.wanderRadius = 7f;
			this.ticksBetweenWandersRange = new IntRange(300, 600);
			this.locomotionUrgency = LocomotionUrgency.Amble;
			this.wanderDestValidator = ((Pawn pawn, IntVec3 loc, IntVec3 root) => WanderRoomUtility.IsValidWanderDest(pawn, loc, root));
		}

		// Token: 0x060028D6 RID: 10454 RVA: 0x000EFE54 File Offset: 0x000EE054
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			MentalState_WanderOwnRoom mentalState_WanderOwnRoom = pawn.MentalState as MentalState_WanderOwnRoom;
			if (mentalState_WanderOwnRoom != null)
			{
				return mentalState_WanderOwnRoom.target;
			}
			return pawn.Position;
		}
	}
}
