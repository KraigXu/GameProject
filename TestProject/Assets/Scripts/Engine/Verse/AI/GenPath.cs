using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000571 RID: 1393
	public static class GenPath
	{
		// Token: 0x0600274C RID: 10060 RVA: 0x000E58A0 File Offset: 0x000E3AA0
		public static TargetInfo ResolvePathMode(Pawn pawn, TargetInfo dest, ref PathEndMode peMode)
		{
			if (dest.HasThing && !dest.Thing.Spawned)
			{
				peMode = PathEndMode.Touch;
				return dest;
			}
			if (peMode == PathEndMode.InteractionCell)
			{
				if (!dest.HasThing)
				{
					Log.Error("Pathed to cell " + dest + " with PathEndMode.InteractionCell.", false);
				}
				peMode = PathEndMode.OnCell;
				return new TargetInfo(dest.Thing.InteractionCell, dest.Thing.Map, false);
			}
			if (peMode == PathEndMode.ClosestTouch)
			{
				peMode = GenPath.ResolveClosestTouchPathMode(pawn, dest.Map, dest.Cell);
			}
			return dest;
		}

		// Token: 0x0600274D RID: 10061 RVA: 0x000E5930 File Offset: 0x000E3B30
		public static PathEndMode ResolveClosestTouchPathMode(Pawn pawn, Map map, IntVec3 target)
		{
			if (GenPath.ShouldNotEnterCell(pawn, map, target))
			{
				return PathEndMode.Touch;
			}
			return PathEndMode.OnCell;
		}

		// Token: 0x0600274E RID: 10062 RVA: 0x000E5940 File Offset: 0x000E3B40
		private static bool ShouldNotEnterCell(Pawn pawn, Map map, IntVec3 dest)
		{
			if (map.pathGrid.PerceivedPathCostAt(dest) > 30)
			{
				return true;
			}
			if (!dest.Walkable(map))
			{
				return true;
			}
			if (pawn != null)
			{
				if (dest.IsForbidden(pawn))
				{
					return true;
				}
				Building edifice = dest.GetEdifice(map);
				if (edifice != null)
				{
					Building_Door building_Door = edifice as Building_Door;
					if (building_Door != null)
					{
						if (building_Door.IsForbidden(pawn))
						{
							return true;
						}
						if (!building_Door.PawnCanOpen(pawn))
						{
							return true;
						}
					}
				}
			}
			return false;
		}
	}
}
