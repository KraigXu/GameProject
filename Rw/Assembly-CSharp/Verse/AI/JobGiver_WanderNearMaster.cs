using System;

namespace Verse.AI
{
	// Token: 0x020005B7 RID: 1463
	public class JobGiver_WanderNearMaster : JobGiver_Wander
	{
		// Token: 0x060028DB RID: 10459 RVA: 0x000EFEDC File Offset: 0x000EE0DC
		public JobGiver_WanderNearMaster()
		{
			this.wanderRadius = 3f;
			this.ticksBetweenWandersRange = new IntRange(125, 200);
			this.wanderDestValidator = ((Pawn p, IntVec3 c, IntVec3 root) => !this.MustUseRootRoom(p) || root.GetRoom(p.Map, RegionType.Set_Passable) == null || WanderRoomUtility.IsValidWanderDest(p, c, root));
		}

		// Token: 0x060028DC RID: 10460 RVA: 0x000EFF13 File Offset: 0x000EE113
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			return WanderUtility.BestCloseWanderRoot(pawn.playerSettings.Master.PositionHeld, pawn);
		}

		// Token: 0x060028DD RID: 10461 RVA: 0x000EFF2B File Offset: 0x000EE12B
		private bool MustUseRootRoom(Pawn pawn)
		{
			return !pawn.playerSettings.Master.playerSettings.animalsReleased;
		}
	}
}
