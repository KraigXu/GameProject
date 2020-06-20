using System;
using Verse.AI;

namespace Verse
{
	// Token: 0x020001B7 RID: 439
	public static class ReachabilityWithinRegion
	{
		// Token: 0x06000C34 RID: 3124 RVA: 0x0004563C File Offset: 0x0004383C
		public static bool ThingFromRegionListerReachable(Thing thing, Region region, PathEndMode peMode, Pawn traveler)
		{
			Map map = region.Map;
			if (peMode == PathEndMode.ClosestTouch)
			{
				peMode = GenPath.ResolveClosestTouchPathMode(traveler, map, thing.Position);
			}
			switch (peMode)
			{
			case PathEndMode.None:
				return false;
			case PathEndMode.OnCell:
				if (thing.def.size.x != 1 || thing.def.size.z != 1)
				{
					using (CellRect.Enumerator enumerator = thing.OccupiedRect().GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (enumerator.Current.GetRegion(map, RegionType.Set_Passable) == region)
							{
								return true;
							}
						}
					}
					return false;
				}
				if (thing.Position.GetRegion(map, RegionType.Set_Passable) == region)
				{
					return true;
				}
				return false;
			case PathEndMode.Touch:
				return true;
			case PathEndMode.InteractionCell:
				return thing.InteractionCell.GetRegion(map, RegionType.Set_Passable) == region;
			}
			Log.Error("Unsupported PathEndMode: " + peMode, false);
			return false;
		}
	}
}
