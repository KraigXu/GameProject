using System;

namespace Verse.AI
{
	// Token: 0x020005B1 RID: 1457
	public static class WanderRoomUtility
	{
		// Token: 0x060028D0 RID: 10448 RVA: 0x000EFCD8 File Offset: 0x000EDED8
		public static bool IsValidWanderDest(Pawn pawn, IntVec3 loc, IntVec3 root)
		{
			Room room = root.GetRoom(pawn.Map, RegionType.Set_Passable);
			return room == null || room.RegionType == RegionType.Portal || WanderUtility.InSameRoom(root, loc, pawn.Map);
		}
	}
}
