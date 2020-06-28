using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020001C8 RID: 456
	public static class RegionTypeUtility
	{
		// Token: 0x06000CB4 RID: 3252 RVA: 0x000489D0 File Offset: 0x00046BD0
		public static bool IsOneCellRegion(this RegionType regionType)
		{
			return regionType == RegionType.Portal;
		}

		// Token: 0x06000CB5 RID: 3253 RVA: 0x000489D6 File Offset: 0x00046BD6
		public static bool AllowsMultipleRegionsPerRoom(this RegionType regionType)
		{
			return regionType != RegionType.Portal;
		}

		// Token: 0x06000CB6 RID: 3254 RVA: 0x000489E0 File Offset: 0x00046BE0
		public static RegionType GetExpectedRegionType(this IntVec3 c, Map map)
		{
			if (!c.InBounds(map))
			{
				return RegionType.None;
			}
			if (c.GetDoor(map) != null)
			{
				return RegionType.Portal;
			}
			if (c.Walkable(map))
			{
				return RegionType.Normal;
			}
			List<Thing> thingList = c.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (thingList[i].def.Fillage == FillCategory.Full)
				{
					return RegionType.None;
				}
			}
			return RegionType.ImpassableFreeAirExchange;
		}

		// Token: 0x06000CB7 RID: 3255 RVA: 0x00048A40 File Offset: 0x00046C40
		public static RegionType GetRegionType(this IntVec3 c, Map map)
		{
			Region region = c.GetRegion(map, RegionType.Set_All);
			if (region == null)
			{
				return RegionType.None;
			}
			return region.type;
		}

		// Token: 0x06000CB8 RID: 3256 RVA: 0x00048A61 File Offset: 0x00046C61
		public static bool Passable(this RegionType regionType)
		{
			return (regionType & RegionType.Set_Passable) > RegionType.None;
		}
	}
}
