using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x020005B9 RID: 1465
	public static class WanderUtility
	{
		// Token: 0x060028E1 RID: 10465 RVA: 0x000EFF94 File Offset: 0x000EE194
		public static IntVec3 BestCloseWanderRoot(IntVec3 trueWanderRoot, Pawn pawn)
		{
			for (int i = 0; i < 50; i++)
			{
				IntVec3 intVec;
				if (i < 8)
				{
					intVec = trueWanderRoot + GenRadial.RadialPattern[i];
				}
				else
				{
					intVec = trueWanderRoot + GenRadial.RadialPattern[i - 8 + 1] * 7;
				}
				if (intVec.InBounds(pawn.Map) && intVec.Walkable(pawn.Map) && pawn.CanReach(intVec, PathEndMode.OnCell, Danger.Some, false, TraverseMode.ByPawn))
				{
					return intVec;
				}
			}
			return IntVec3.Invalid;
		}

		// Token: 0x060028E2 RID: 10466 RVA: 0x000F0018 File Offset: 0x000EE218
		public static bool InSameRoom(IntVec3 locA, IntVec3 locB, Map map)
		{
			Room room = locA.GetRoom(map, RegionType.Set_All);
			return room == null || room == locB.GetRoom(map, RegionType.Set_All);
		}

		// Token: 0x060028E3 RID: 10467 RVA: 0x000F0040 File Offset: 0x000EE240
		public static IntVec3 GetColonyWanderRoot(Pawn pawn)
		{
			if (pawn.RaceProps.Humanlike)
			{
				WanderUtility.gatherSpots.Clear();
				for (int i = 0; i < pawn.Map.gatherSpotLister.activeSpots.Count; i++)
				{
					IntVec3 position = pawn.Map.gatherSpotLister.activeSpots[i].parent.Position;
					if (!position.IsForbidden(pawn) && pawn.CanReach(position, PathEndMode.Touch, Danger.None, false, TraverseMode.ByPawn))
					{
						WanderUtility.gatherSpots.Add(position);
					}
				}
				if (WanderUtility.gatherSpots.Count > 0)
				{
					return WanderUtility.gatherSpots.RandomElement<IntVec3>();
				}
			}
			List<Building> allBuildingsColonist = pawn.Map.listerBuildings.allBuildingsColonist;
			if (allBuildingsColonist.Count > 0)
			{
				int num = 0;
				IntVec3 intVec;
				for (;;)
				{
					num++;
					if (num > 20)
					{
						goto IL_1F7;
					}
					Building building = allBuildingsColonist.RandomElement<Building>();
					if ((building.def == ThingDefOf.Wall || building.def.building.ai_chillDestination) && !building.Position.IsForbidden(pawn) && pawn.Map.areaManager.Home[building.Position])
					{
						int num2 = 15 + num * 2;
						if ((pawn.Position - building.Position).LengthHorizontalSquared <= num2 * num2)
						{
							intVec = GenAdjFast.AdjacentCells8Way(building).RandomElement<IntVec3>();
							if (intVec.Standable(building.Map) && !intVec.IsForbidden(pawn) && pawn.CanReach(intVec, PathEndMode.OnCell, Danger.None, false, TraverseMode.ByPawn) && !intVec.IsInPrisonCell(pawn.Map))
							{
								break;
							}
						}
					}
				}
				return intVec;
			}
			IL_1F7:
			Pawn pawn2;
			if ((from c in pawn.Map.mapPawns.FreeColonistsSpawned
			where !c.Position.IsForbidden(pawn) && pawn.CanReach(c.Position, PathEndMode.Touch, Danger.None, false, TraverseMode.ByPawn)
			select c).TryRandomElement(out pawn2))
			{
				return pawn2.Position;
			}
			return pawn.Position;
		}

		// Token: 0x04001877 RID: 6263
		private static List<IntVec3> gatherSpots = new List<IntVec3>();
	}
}
