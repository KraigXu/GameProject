using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000447 RID: 1095
	public static class GenGrid
	{
		// Token: 0x060020D5 RID: 8405 RVA: 0x000C8FE3 File Offset: 0x000C71E3
		public static bool InNoBuildEdgeArea(this IntVec3 c, Map map)
		{
			return c.CloseToEdge(map, 10);
		}

		// Token: 0x060020D6 RID: 8406 RVA: 0x000C8FEE File Offset: 0x000C71EE
		public static bool InNoZoneEdgeArea(this IntVec3 c, Map map)
		{
			return c.CloseToEdge(map, 5);
		}

		// Token: 0x060020D7 RID: 8407 RVA: 0x000C8FF8 File Offset: 0x000C71F8
		public static bool CloseToEdge(this IntVec3 c, Map map, int edgeDist)
		{
			IntVec3 size = map.Size;
			return c.x < edgeDist || c.z < edgeDist || c.x >= size.x - edgeDist || c.z >= size.z - edgeDist;
		}

		// Token: 0x060020D8 RID: 8408 RVA: 0x000C9044 File Offset: 0x000C7244
		public static bool OnEdge(this IntVec3 c, Map map)
		{
			IntVec3 size = map.Size;
			return c.x == 0 || c.x == size.x - 1 || c.z == 0 || c.z == size.z - 1;
		}

		// Token: 0x060020D9 RID: 8409 RVA: 0x000C908C File Offset: 0x000C728C
		public static bool OnEdge(this IntVec3 c, Map map, Rot4 dir)
		{
			if (dir == Rot4.North)
			{
				return c.z == 0;
			}
			if (dir == Rot4.South)
			{
				return c.z == map.Size.z - 1;
			}
			if (dir == Rot4.West)
			{
				return c.x == 0;
			}
			if (dir == Rot4.East)
			{
				return c.x == map.Size.x - 1;
			}
			Log.ErrorOnce("Invalid edge direction", 55370769, false);
			return false;
		}

		// Token: 0x060020DA RID: 8410 RVA: 0x000C9120 File Offset: 0x000C7320
		public static bool InBounds(this IntVec3 c, Map map)
		{
			IntVec3 size = map.Size;
			return (ulong)c.x < (ulong)((long)size.x) && (ulong)c.z < (ulong)((long)size.z);
		}

		// Token: 0x060020DB RID: 8411 RVA: 0x000C9158 File Offset: 0x000C7358
		public static bool InBounds(this Vector3 v, Map map)
		{
			IntVec3 size = map.Size;
			return v.x >= 0f && v.z >= 0f && v.x < (float)size.x && v.z < (float)size.z;
		}

		// Token: 0x060020DC RID: 8412 RVA: 0x000C91A6 File Offset: 0x000C73A6
		public static bool Walkable(this IntVec3 c, Map map)
		{
			return map.pathGrid.Walkable(c);
		}

		// Token: 0x060020DD RID: 8413 RVA: 0x000C91B4 File Offset: 0x000C73B4
		public static bool Standable(this IntVec3 c, Map map)
		{
			if (!map.pathGrid.Walkable(c))
			{
				return false;
			}
			List<Thing> list = map.thingGrid.ThingsListAt(c);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].def.passability != Traversability.Standable)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060020DE RID: 8414 RVA: 0x000C9208 File Offset: 0x000C7408
		public static bool Impassable(this IntVec3 c, Map map)
		{
			List<Thing> list = map.thingGrid.ThingsListAtFast(c);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].def.passability == Traversability.Impassable)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060020DF RID: 8415 RVA: 0x000C924A File Offset: 0x000C744A
		public static bool SupportsStructureType(this IntVec3 c, Map map, TerrainAffordanceDef surfaceType)
		{
			return c.GetTerrain(map).affordances.Contains(surfaceType);
		}

		// Token: 0x060020E0 RID: 8416 RVA: 0x000C9260 File Offset: 0x000C7460
		public static bool CanBeSeenOver(this IntVec3 c, Map map)
		{
			if (!c.InBounds(map))
			{
				return false;
			}
			Building edifice = c.GetEdifice(map);
			return edifice == null || edifice.CanBeSeenOver();
		}

		// Token: 0x060020E1 RID: 8417 RVA: 0x000C9290 File Offset: 0x000C7490
		public static bool CanBeSeenOverFast(this IntVec3 c, Map map)
		{
			Building edifice = c.GetEdifice(map);
			return edifice == null || edifice.CanBeSeenOver();
		}

		// Token: 0x060020E2 RID: 8418 RVA: 0x000C92B4 File Offset: 0x000C74B4
		public static bool CanBeSeenOver(this Building b)
		{
			if (b.def.Fillage == FillCategory.Full)
			{
				Building_Door building_Door = b as Building_Door;
				return building_Door != null && building_Door.Open;
			}
			return true;
		}

		// Token: 0x060020E3 RID: 8419 RVA: 0x000C92E8 File Offset: 0x000C74E8
		public static SurfaceType GetSurfaceType(this IntVec3 c, Map map)
		{
			if (!c.InBounds(map))
			{
				return SurfaceType.None;
			}
			List<Thing> thingList = c.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (thingList[i].def.surfaceType != SurfaceType.None)
				{
					return thingList[i].def.surfaceType;
				}
			}
			return SurfaceType.None;
		}

		// Token: 0x060020E4 RID: 8420 RVA: 0x000C933F File Offset: 0x000C753F
		public static bool HasEatSurface(this IntVec3 c, Map map)
		{
			return c.GetSurfaceType(map) == SurfaceType.Eat;
		}

		// Token: 0x0400140D RID: 5133
		public const int NoBuildEdgeWidth = 10;

		// Token: 0x0400140E RID: 5134
		public const int NoZoneEdgeWidth = 5;
	}
}
