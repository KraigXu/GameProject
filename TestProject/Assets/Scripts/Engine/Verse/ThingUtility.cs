using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000316 RID: 790
	public static class ThingUtility
	{
		// Token: 0x060016E2 RID: 5858 RVA: 0x00083D02 File Offset: 0x00081F02
		public static bool DestroyedOrNull(this Thing t)
		{
			return t == null || t.Destroyed;
		}

		// Token: 0x060016E3 RID: 5859 RVA: 0x00083D10 File Offset: 0x00081F10
		public static void DestroyOrPassToWorld(this Thing t, DestroyMode mode = DestroyMode.Vanish)
		{
			Pawn pawn = t as Pawn;
			if (pawn != null)
			{
				if (!Find.WorldPawns.Contains(pawn))
				{
					Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
					return;
				}
			}
			else
			{
				t.Destroy(mode);
			}
		}

		// Token: 0x060016E4 RID: 5860 RVA: 0x00083D48 File Offset: 0x00081F48
		public static int TryAbsorbStackNumToTake(Thing thing, Thing other, bool respectStackLimit)
		{
			int result;
			if (respectStackLimit)
			{
				result = Mathf.Min(other.stackCount, thing.def.stackLimit - thing.stackCount);
			}
			else
			{
				result = other.stackCount;
			}
			return result;
		}

		// Token: 0x060016E5 RID: 5861 RVA: 0x00083D80 File Offset: 0x00081F80
		public static int RoundedResourceStackCount(int stackCount)
		{
			if (stackCount > 200)
			{
				return GenMath.RoundTo(stackCount, 10);
			}
			if (stackCount > 100)
			{
				return GenMath.RoundTo(stackCount, 5);
			}
			return stackCount;
		}

		// Token: 0x060016E6 RID: 5862 RVA: 0x00083DA4 File Offset: 0x00081FA4
		public static IntVec3 InteractionCellWhenAt(ThingDef def, IntVec3 center, Rot4 rot, Map map)
		{
			if (def.hasInteractionCell)
			{
				IntVec3 b = def.interactionCellOffset.RotatedBy(rot);
				return center + b;
			}
			if (def.Size.x == 1 && def.Size.z == 1)
			{
				for (int i = 0; i < 8; i++)
				{
					IntVec3 intVec = center + GenAdj.AdjacentCells[i];
					if (intVec.Standable(map) && intVec.GetDoor(map) == null && ReachabilityImmediate.CanReachImmediate(intVec, center, map, PathEndMode.Touch, null))
					{
						return intVec;
					}
				}
				for (int j = 0; j < 8; j++)
				{
					IntVec3 intVec2 = center + GenAdj.AdjacentCells[j];
					if (intVec2.Standable(map) && ReachabilityImmediate.CanReachImmediate(intVec2, center, map, PathEndMode.Touch, null))
					{
						return intVec2;
					}
				}
				for (int k = 0; k < 8; k++)
				{
					IntVec3 intVec3 = center + GenAdj.AdjacentCells[k];
					if (intVec3.Walkable(map) && ReachabilityImmediate.CanReachImmediate(intVec3, center, map, PathEndMode.Touch, null))
					{
						return intVec3;
					}
				}
				return center;
			}
			List<IntVec3> list = GenAdjFast.AdjacentCells8Way(center, rot, def.size);
			CellRect rect = GenAdj.OccupiedRect(center, rot, def.size);
			for (int l = 0; l < list.Count; l++)
			{
				if (list[l].Standable(map) && list[l].GetDoor(map) == null && ReachabilityImmediate.CanReachImmediate(list[l], rect, map, PathEndMode.Touch, null))
				{
					return list[l];
				}
			}
			for (int m = 0; m < list.Count; m++)
			{
				if (list[m].Standable(map) && ReachabilityImmediate.CanReachImmediate(list[m], rect, map, PathEndMode.Touch, null))
				{
					return list[m];
				}
			}
			for (int n = 0; n < list.Count; n++)
			{
				if (list[n].Walkable(map) && ReachabilityImmediate.CanReachImmediate(list[n], rect, map, PathEndMode.Touch, null))
				{
					return list[n];
				}
			}
			return center;
		}

		// Token: 0x060016E7 RID: 5863 RVA: 0x00083FB8 File Offset: 0x000821B8
		public static DamageDef PrimaryMeleeWeaponDamageType(ThingDef thing)
		{
			return ThingUtility.PrimaryMeleeWeaponDamageType(thing.tools);
		}

		// Token: 0x060016E8 RID: 5864 RVA: 0x00083FC8 File Offset: 0x000821C8
		public static DamageDef PrimaryMeleeWeaponDamageType(List<Tool> tools)
		{
			if (tools.NullOrEmpty<Tool>())
			{
				return null;
			}
			ManeuverDef maneuverDef = tools.MaxBy((Tool tool) => tool.power).Maneuvers.FirstOrDefault<ManeuverDef>();
			if (maneuverDef == null)
			{
				return null;
			}
			return maneuverDef.verb.meleeDamageDef;
		}

		// Token: 0x060016E9 RID: 5865 RVA: 0x00084020 File Offset: 0x00082220
		public static void CheckAutoRebuildOnDestroyed(Thing thing, DestroyMode mode, Map map, BuildableDef buildingDef)
		{
			if (Find.PlaySettings.autoRebuild && mode == DestroyMode.KillFinalize && thing.Faction == Faction.OfPlayer && buildingDef.blueprintDef != null && buildingDef.IsResearchFinished && map.areaManager.Home[thing.Position] && GenConstruct.CanPlaceBlueprintAt(buildingDef, thing.Position, thing.Rotation, map, false, null, null, thing.Stuff).Accepted)
			{
				GenConstruct.PlaceBlueprintForBuild(buildingDef, thing.Position, map, thing.Rotation, Faction.OfPlayer, thing.Stuff);
			}
		}

		// Token: 0x060016EA RID: 5866 RVA: 0x000840B8 File Offset: 0x000822B8
		public static Pawn FindPawn(List<Thing> things)
		{
			for (int i = 0; i < things.Count; i++)
			{
				Pawn pawn = things[i] as Pawn;
				if (pawn != null)
				{
					return pawn;
				}
				Corpse corpse = things[i] as Corpse;
				if (corpse != null)
				{
					return corpse.InnerPawn;
				}
			}
			return null;
		}

		// Token: 0x060016EB RID: 5867 RVA: 0x00084100 File Offset: 0x00082300
		public static TerrainAffordanceDef GetTerrainAffordanceNeed(this BuildableDef def, ThingDef stuffDef = null)
		{
			TerrainAffordanceDef terrainAffordanceNeeded = def.terrainAffordanceNeeded;
			if (stuffDef != null && def.useStuffTerrainAffordance && stuffDef.terrainAffordanceNeeded != null)
			{
				terrainAffordanceNeeded = stuffDef.terrainAffordanceNeeded;
			}
			return terrainAffordanceNeeded;
		}
	}
}
