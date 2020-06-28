using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A71 RID: 2673
	public static class SiteGenStepUtility
	{
		// Token: 0x06003F02 RID: 16130 RVA: 0x0014F568 File Offset: 0x0014D768
		public static bool TryFindRootToSpawnAroundRectOfInterest(out CellRect rectToDefend, out IntVec3 singleCellToSpawnNear, Map map)
		{
			singleCellToSpawnNear = IntVec3.Invalid;
			if (!MapGenerator.TryGetVar<CellRect>("RectOfInterest", out rectToDefend))
			{
				rectToDefend = CellRect.Empty;
				if (!RCellFinder.TryFindRandomCellNearTheCenterOfTheMapWith((IntVec3 x) => x.Standable(map) && !x.Fogged(map) && x.GetRoom(map, RegionType.Set_Passable).CellCount >= 225, map, out singleCellToSpawnNear))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06003F03 RID: 16131 RVA: 0x0014F5C4 File Offset: 0x0014D7C4
		public static bool TryFindSpawnCellAroundOrNear(CellRect around, IntVec3 near, Map map, out IntVec3 spawnCell)
		{
			if (near.IsValid)
			{
				if (!CellFinder.TryFindRandomSpawnCellForPawnNear(near, map, out spawnCell, 10))
				{
					return false;
				}
			}
			else if (!CellFinder.TryFindRandomCellInsideWith(around.ExpandedBy(8), (IntVec3 x) => !around.Contains(x) && x.InBounds(map) && x.Standable(map) && !x.Fogged(map), out spawnCell))
			{
				return false;
			}
			return true;
		}
	}
}
