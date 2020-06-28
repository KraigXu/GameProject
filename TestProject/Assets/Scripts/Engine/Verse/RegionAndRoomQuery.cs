using System;

namespace Verse
{
	// Token: 0x020001BA RID: 442
	public static class RegionAndRoomQuery
	{
		// Token: 0x06000C53 RID: 3155 RVA: 0x000464E4 File Offset: 0x000446E4
		public static Region RegionAt(IntVec3 c, Map map, RegionType allowedRegionTypes = RegionType.Set_Passable)
		{
			if (!c.InBounds(map))
			{
				return null;
			}
			Region validRegionAt = map.regionGrid.GetValidRegionAt(c);
			if (validRegionAt != null && (validRegionAt.type & allowedRegionTypes) != RegionType.None)
			{
				return validRegionAt;
			}
			return null;
		}

		// Token: 0x06000C54 RID: 3156 RVA: 0x00046519 File Offset: 0x00044719
		public static Region GetRegion(this Thing thing, RegionType allowedRegionTypes = RegionType.Set_Passable)
		{
			if (!thing.Spawned)
			{
				return null;
			}
			return RegionAndRoomQuery.RegionAt(thing.Position, thing.Map, allowedRegionTypes);
		}

		// Token: 0x06000C55 RID: 3157 RVA: 0x00046538 File Offset: 0x00044738
		public static Room RoomAt(IntVec3 c, Map map, RegionType allowedRegionTypes = RegionType.Set_Passable)
		{
			Region region = RegionAndRoomQuery.RegionAt(c, map, allowedRegionTypes);
			if (region == null)
			{
				return null;
			}
			return region.Room;
		}

		// Token: 0x06000C56 RID: 3158 RVA: 0x0004655C File Offset: 0x0004475C
		public static RoomGroup RoomGroupAt(IntVec3 c, Map map)
		{
			Room room = RegionAndRoomQuery.RoomAt(c, map, RegionType.Set_All);
			if (room == null)
			{
				return null;
			}
			return room.Group;
		}

		// Token: 0x06000C57 RID: 3159 RVA: 0x0004657D File Offset: 0x0004477D
		public static Room GetRoom(this Thing thing, RegionType allowedRegionTypes = RegionType.Set_Passable)
		{
			if (!thing.Spawned)
			{
				return null;
			}
			return RegionAndRoomQuery.RoomAt(thing.Position, thing.Map, allowedRegionTypes);
		}

		// Token: 0x06000C58 RID: 3160 RVA: 0x0004659C File Offset: 0x0004479C
		public static RoomGroup GetRoomGroup(this Thing thing)
		{
			Room room = thing.GetRoom(RegionType.Set_All);
			if (room == null)
			{
				return null;
			}
			return room.Group;
		}

		// Token: 0x06000C59 RID: 3161 RVA: 0x000465BC File Offset: 0x000447BC
		public static Room RoomAtFast(IntVec3 c, Map map, RegionType allowedRegionTypes = RegionType.Set_Passable)
		{
			Region validRegionAt = map.regionGrid.GetValidRegionAt(c);
			if (validRegionAt != null && (validRegionAt.type & allowedRegionTypes) != RegionType.None)
			{
				return validRegionAt.Room;
			}
			return null;
		}

		// Token: 0x06000C5A RID: 3162 RVA: 0x000465EC File Offset: 0x000447EC
		public static Room RoomAtOrAdjacent(IntVec3 c, Map map, RegionType allowedRegionTypes = RegionType.Set_Passable)
		{
			Room room = RegionAndRoomQuery.RoomAt(c, map, allowedRegionTypes);
			if (room != null)
			{
				return room;
			}
			for (int i = 0; i < 8; i++)
			{
				room = RegionAndRoomQuery.RoomAt(c + GenAdj.AdjacentCells[i], map, allowedRegionTypes);
				if (room != null)
				{
					return room;
				}
			}
			return room;
		}
	}
}
