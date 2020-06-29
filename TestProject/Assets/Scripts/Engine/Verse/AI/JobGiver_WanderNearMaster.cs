using System;

namespace Verse.AI
{
	
	public class JobGiver_WanderNearMaster : JobGiver_Wander
	{
		
		public JobGiver_WanderNearMaster()
		{
			this.wanderRadius = 3f;
			this.ticksBetweenWandersRange = new IntRange(125, 200);
			this.wanderDestValidator = ((Pawn p, IntVec3 c, IntVec3 root) => !this.MustUseRootRoom(p) || root.GetRoom(p.Map, RegionType.Set_Passable) == null || WanderRoomUtility.IsValidWanderDest(p, c, root));
		}

		
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			return WanderUtility.BestCloseWanderRoot(pawn.playerSettings.Master.PositionHeld, pawn);
		}

		
		private bool MustUseRootRoom(Pawn pawn)
		{
			return !pawn.playerSettings.Master.playerSettings.animalsReleased;
		}
	}
}
