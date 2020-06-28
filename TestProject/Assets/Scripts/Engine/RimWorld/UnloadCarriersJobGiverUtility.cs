using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020006C0 RID: 1728
	public static class UnloadCarriersJobGiverUtility
	{
		// Token: 0x06002E7F RID: 11903 RVA: 0x0010566C File Offset: 0x0010386C
		public static bool HasJobOnThing(Pawn pawn, Thing t, bool forced)
		{
			Pawn pawn2 = t as Pawn;
			return pawn2 != null && pawn2 != pawn && !pawn2.IsFreeColonist && pawn2.inventory.UnloadEverything && (pawn2.Faction == pawn.Faction || pawn2.HostFaction == pawn.Faction) && !t.IsForbidden(pawn) && !t.IsBurning() && pawn.CanReserve(t, 1, -1, null, forced);
		}
	}
}
