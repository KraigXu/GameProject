using System;
using System.Collections.Generic;
using Verse.AI;

namespace Verse
{
	// Token: 0x020001B5 RID: 437
	public static class ReachabilityUtility
	{
		// Token: 0x06000C2B RID: 3115 RVA: 0x00045399 File Offset: 0x00043599
		public static bool CanReach(this Pawn pawn, LocalTargetInfo dest, PathEndMode peMode, Danger maxDanger, bool canBash = false, TraverseMode mode = TraverseMode.ByPawn)
		{
			return pawn.Spawned && pawn.Map.reachability.CanReach(pawn.Position, dest, peMode, TraverseParms.For(pawn, maxDanger, mode, canBash));
		}

		// Token: 0x06000C2C RID: 3116 RVA: 0x000453C8 File Offset: 0x000435C8
		public static bool CanReachNonLocal(this Pawn pawn, TargetInfo dest, PathEndMode peMode, Danger maxDanger, bool canBash = false, TraverseMode mode = TraverseMode.ByPawn)
		{
			return pawn.Spawned && pawn.Map.reachability.CanReachNonLocal(pawn.Position, dest, peMode, TraverseParms.For(pawn, maxDanger, mode, canBash));
		}

		// Token: 0x06000C2D RID: 3117 RVA: 0x000453F7 File Offset: 0x000435F7
		public static bool CanReachMapEdge(this Pawn p)
		{
			return p.Spawned && p.Map.reachability.CanReachMapEdge(p.Position, TraverseParms.For(p, Danger.Deadly, TraverseMode.ByPawn, false));
		}

		// Token: 0x06000C2E RID: 3118 RVA: 0x00045424 File Offset: 0x00043624
		public static void ClearCache()
		{
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				maps[i].reachability.ClearCache();
			}
		}

		// Token: 0x06000C2F RID: 3119 RVA: 0x0004545C File Offset: 0x0004365C
		public static void ClearCacheFor(Pawn p)
		{
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				maps[i].reachability.ClearCacheFor(p);
			}
		}
	}
}
