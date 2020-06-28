using System;
using Verse.AI;

namespace Verse
{
	// Token: 0x020001B6 RID: 438
	public static class ReachabilityImmediate
	{
		// Token: 0x06000C30 RID: 3120 RVA: 0x00045494 File Offset: 0x00043694
		public static bool CanReachImmediate(IntVec3 start, LocalTargetInfo target, Map map, PathEndMode peMode, Pawn pawn)
		{
			if (!target.IsValid)
			{
				return false;
			}
			target = (LocalTargetInfo)GenPath.ResolvePathMode(pawn, target.ToTargetInfo(map), ref peMode);
			if (target.HasThing)
			{
				Thing thing = target.Thing;
				if (!thing.Spawned)
				{
					if (pawn != null)
					{
						if (pawn.carryTracker.innerContainer.Contains(thing))
						{
							return true;
						}
						if (pawn.inventory.innerContainer.Contains(thing))
						{
							return true;
						}
						if (pawn.apparel != null && pawn.apparel.Contains(thing))
						{
							return true;
						}
						if (pawn.equipment != null && pawn.equipment.Contains(thing))
						{
							return true;
						}
					}
					return false;
				}
				if (thing.Map != map)
				{
					return false;
				}
			}
			if (!target.HasThing || (target.Thing.def.size.x == 1 && target.Thing.def.size.z == 1))
			{
				if (start == target.Cell)
				{
					return true;
				}
			}
			else if (start.IsInside(target.Thing))
			{
				return true;
			}
			return peMode == PathEndMode.Touch && TouchPathEndModeUtility.IsAdjacentOrInsideAndAllowedToTouch(start, target, map);
		}

		// Token: 0x06000C31 RID: 3121 RVA: 0x000455BF File Offset: 0x000437BF
		public static bool CanReachImmediate(this Pawn pawn, LocalTargetInfo target, PathEndMode peMode)
		{
			return pawn.Spawned && ReachabilityImmediate.CanReachImmediate(pawn.Position, target, pawn.Map, peMode, pawn);
		}

		// Token: 0x06000C32 RID: 3122 RVA: 0x000455DF File Offset: 0x000437DF
		public static bool CanReachImmediateNonLocal(this Pawn pawn, TargetInfo target, PathEndMode peMode)
		{
			return pawn.Spawned && (target.Map == null || target.Map == pawn.Map) && pawn.CanReachImmediate((LocalTargetInfo)target, peMode);
		}

		// Token: 0x06000C33 RID: 3123 RVA: 0x00045614 File Offset: 0x00043814
		public static bool CanReachImmediate(IntVec3 start, CellRect rect, Map map, PathEndMode peMode, Pawn pawn)
		{
			IntVec3 c = rect.ClosestCellTo(start);
			return ReachabilityImmediate.CanReachImmediate(start, c, map, peMode, pawn);
		}
	}
}
