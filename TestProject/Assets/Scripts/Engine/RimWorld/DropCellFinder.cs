using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000F39 RID: 3897
	public static class DropCellFinder
	{
		// Token: 0x06005F71 RID: 24433 RVA: 0x002103C0 File Offset: 0x0020E5C0
		public static IntVec3 RandomDropSpot(Map map)
		{
			return CellFinderLoose.RandomCellWith((IntVec3 c) => c.Standable(map) && !c.Roofed(map) && !c.Fogged(map), map, 1000);
		}

		// Token: 0x06005F72 RID: 24434 RVA: 0x002103F8 File Offset: 0x0020E5F8
		public static IntVec3 TradeDropSpot(Map map)
		{
			IEnumerable<Building> collection = from b in map.listerBuildings.allBuildingsColonist
			where b.def.IsCommsConsole
			select b;
			IEnumerable<Building> enumerable = from b in map.listerBuildings.allBuildingsColonist
			where b.def.IsOrbitalTradeBeacon
			select b;
			Building building = enumerable.FirstOrDefault((Building b) => !map.roofGrid.Roofed(b.Position) && DropCellFinder.AnyAdjacentGoodDropSpot(b.Position, map, false, false));
			IntVec3 position;
			if (building != null)
			{
				position = building.Position;
				IntVec3 result;
				if (!DropCellFinder.TryFindDropSpotNear(position, map, out result, false, false, true, null))
				{
					Log.Error("Could find no good TradeDropSpot near dropCenter " + position + ". Using a random standable unfogged cell.", false);
					result = CellFinderLoose.RandomCellWith((IntVec3 c) => c.Standable(map) && !c.Fogged(map), map, 1000);
				}
				return result;
			}
			List<Building> list = new List<Building>();
			list.AddRange(enumerable);
			list.AddRange(collection);
			list.RemoveAll(delegate(Building b)
			{
				CompPowerTrader compPowerTrader = b.TryGetComp<CompPowerTrader>();
				return compPowerTrader != null && !compPowerTrader.PowerOn;
			});
			Predicate<IntVec3> validator = (IntVec3 c) => DropCellFinder.IsGoodDropSpot(c, map, false, false, true);
			if (!list.Any<Building>())
			{
				list.AddRange(map.listerBuildings.allBuildingsColonist);
				list.Shuffle<Building>();
				if (!list.Any<Building>())
				{
					return CellFinderLoose.RandomCellWith(validator, map, 1000);
				}
			}
			int num = 8;
			for (;;)
			{
				for (int i = 0; i < list.Count; i++)
				{
					if (CellFinder.TryFindRandomCellNear(list[i].Position, map, num, validator, out position, -1))
					{
						return position;
					}
				}
				num = Mathf.RoundToInt((float)num * 1.1f);
				if (num > map.Size.x)
				{
					goto Block_9;
				}
			}
			return position;
			Block_9:
			Log.Error("Failed to generate trade drop center. Giving random.", false);
			return CellFinderLoose.RandomCellWith(validator, map, 1000);
		}

		// Token: 0x06005F73 RID: 24435 RVA: 0x00210608 File Offset: 0x0020E808
		public static IntVec3 TryFindSafeLandingSpotCloseToColony(Map map, IntVec2 size, Faction faction = null, int borderWidth = 2)
		{
			DropCellFinder.<>c__DisplayClass3_0 <>c__DisplayClass3_ = new DropCellFinder.<>c__DisplayClass3_0();
			<>c__DisplayClass3_.map = map;
			<>c__DisplayClass3_.size = size;
			<>c__DisplayClass3_.faction = faction;
			DropCellFinder.<>c__DisplayClass3_0 <>c__DisplayClass3_2 = <>c__DisplayClass3_;
			<>c__DisplayClass3_2.size.x = <>c__DisplayClass3_2.size.x + borderWidth;
			DropCellFinder.<>c__DisplayClass3_0 <>c__DisplayClass3_3 = <>c__DisplayClass3_;
			<>c__DisplayClass3_3.size.z = <>c__DisplayClass3_3.size.z + borderWidth;
			DropCellFinder.tmpColonyBuildings.Clear();
			DropCellFinder.tmpColonyBuildings.AddRange(<>c__DisplayClass3_.map.listerBuildings.allBuildingsColonist);
			if (!DropCellFinder.tmpColonyBuildings.Any<Building>())
			{
				return CellFinderLoose.RandomCellWith(delegate(IntVec3 c)
				{
					if (!DropCellFinder.SkyfallerCanLandAt(c, <>c__DisplayClass3_.map, <>c__DisplayClass3_.size, <>c__DisplayClass3_.faction))
					{
						return false;
					}
					if (ModsConfig.RoyaltyActive)
					{
						List<Thing> list = <>c__DisplayClass3_.map.listerThings.ThingsOfDef(ThingDefOf.ActivatorProximity);
						for (int j = 0; j < list.Count; j++)
						{
							if (list[j].Faction != null && list[j].Faction.HostileTo(<>c__DisplayClass3_.faction))
							{
								CompSendSignalOnPawnProximity compSendSignalOnPawnProximity = list[j].TryGetComp<CompSendSignalOnPawnProximity>();
								if (compSendSignalOnPawnProximity != null && c.InHorDistOf(list[j].Position, compSendSignalOnPawnProximity.Props.radius + 10f))
								{
									return false;
								}
							}
						}
					}
					return true;
				}, <>c__DisplayClass3_.map, 1000);
			}
			DropCellFinder.tmpColonyBuildings.Shuffle<Building>();
			for (int i = 0; i < DropCellFinder.tmpColonyBuildings.Count; i++)
			{
				IntVec3 intVec;
				if (DropCellFinder.TryFindDropSpotNear(DropCellFinder.tmpColonyBuildings[i].Position, <>c__DisplayClass3_.map, out intVec, false, false, false, new IntVec2?(<>c__DisplayClass3_.size)) && DropCellFinder.SkyfallerCanLandAt(intVec, <>c__DisplayClass3_.map, <>c__DisplayClass3_.size, <>c__DisplayClass3_.faction))
				{
					DropCellFinder.tmpColonyBuildings.Clear();
					return intVec;
				}
			}
			DropCellFinder.tmpColonyBuildings.Clear();
			return CellFinderLoose.RandomCellWith(delegate(IntVec3 c)
			{
				if (!DropCellFinder.SkyfallerCanLandAt(c, <>c__DisplayClass3_.map, <>c__DisplayClass3_.size, <>c__DisplayClass3_.faction))
				{
					return false;
				}
				if (ModsConfig.RoyaltyActive)
				{
					List<Thing> list = <>c__DisplayClass3_.map.listerThings.ThingsOfDef(ThingDefOf.ActivatorProximity);
					for (int j = 0; j < list.Count; j++)
					{
						if (list[j].Faction != null && list[j].Faction.HostileTo(<>c__DisplayClass3_.faction))
						{
							CompSendSignalOnPawnProximity compSendSignalOnPawnProximity = list[j].TryGetComp<CompSendSignalOnPawnProximity>();
							if (compSendSignalOnPawnProximity != null && c.InHorDistOf(list[j].Position, compSendSignalOnPawnProximity.Props.radius + 10f))
							{
								return false;
							}
						}
					}
				}
				return true;
			}, <>c__DisplayClass3_.map, 1000);
		}

		// Token: 0x06005F74 RID: 24436 RVA: 0x00210738 File Offset: 0x0020E938
		public static bool SkyfallerCanLandAt(IntVec3 c, Map map, IntVec2 size, Faction faction = null)
		{
			if (!DropCellFinder.IsSafeDropSpot(c, map, faction, new IntVec2?(size), 25, 35, 15))
			{
				return false;
			}
			foreach (IntVec3 c2 in GenAdj.OccupiedRect(c, Rot4.North, size))
			{
				List<Thing> thingList = c2.GetThingList(map);
				for (int i = 0; i < thingList.Count; i++)
				{
					Thing thing = thingList[i];
					if (thing is IActiveDropPod || thing is Skyfaller)
					{
						return false;
					}
					PlantProperties plant = thing.def.plant;
					if (plant != null && plant.IsTree)
					{
						return false;
					}
					if (thing.def.category == ThingCategory.Item || thing.def.category == ThingCategory.Building)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06005F75 RID: 24437 RVA: 0x0021082C File Offset: 0x0020EA2C
		public static bool TryFindShipLandingArea(Map map, out IntVec3 result, out Thing firstBlockingThing)
		{
			DropCellFinder.tmpShipLandingAreas.Clear();
			List<ShipLandingArea> landingZones = ShipLandingBeaconUtility.GetLandingZones(map);
			if (landingZones.Any<ShipLandingArea>())
			{
				for (int i = 0; i < landingZones.Count; i++)
				{
					if (landingZones[i].Clear)
					{
						DropCellFinder.tmpShipLandingAreas.Add(landingZones[i]);
					}
				}
				if (DropCellFinder.tmpShipLandingAreas.Any<ShipLandingArea>())
				{
					result = DropCellFinder.tmpShipLandingAreas.RandomElement<ShipLandingArea>().CenterCell;
					firstBlockingThing = null;
					DropCellFinder.tmpShipLandingAreas.Clear();
					return true;
				}
				firstBlockingThing = landingZones[0].FirstBlockingThing;
			}
			else
			{
				firstBlockingThing = null;
			}
			result = IntVec3.Invalid;
			DropCellFinder.tmpShipLandingAreas.Clear();
			return false;
		}

		// Token: 0x06005F76 RID: 24438 RVA: 0x002108DC File Offset: 0x0020EADC
		public static bool TryFindDropSpotNear(IntVec3 center, Map map, out IntVec3 result, bool allowFogged, bool canRoofPunch, bool allowIndoors = true, IntVec2? size = null)
		{
			DropCellFinder.<>c__DisplayClass7_0 <>c__DisplayClass7_ = new DropCellFinder.<>c__DisplayClass7_0();
			<>c__DisplayClass7_.size = size;
			<>c__DisplayClass7_.map = map;
			<>c__DisplayClass7_.allowFogged = allowFogged;
			<>c__DisplayClass7_.canRoofPunch = canRoofPunch;
			<>c__DisplayClass7_.allowIndoors = allowIndoors;
			<>c__DisplayClass7_.center = center;
			if (DebugViewSettings.drawDestSearch)
			{
				<>c__DisplayClass7_.map.debugDrawer.FlashCell(<>c__DisplayClass7_.center, 1f, "center", 50);
			}
			<>c__DisplayClass7_.centerRoom = <>c__DisplayClass7_.center.GetRoom(<>c__DisplayClass7_.map, RegionType.Set_Passable);
			<>c__DisplayClass7_.validator = delegate(IntVec3 c)
			{
				if (<>c__DisplayClass7_.size != null)
				{
					using (CellRect.Enumerator enumerator = GenAdj.OccupiedRect(c, Rot4.North, <>c__DisplayClass7_.size.Value).GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (!DropCellFinder.IsGoodDropSpot(enumerator.Current, <>c__DisplayClass7_.map, <>c__DisplayClass7_.allowFogged, <>c__DisplayClass7_.canRoofPunch, <>c__DisplayClass7_.allowIndoors))
							{
								return false;
							}
						}
						goto IL_93;
					}
				}
				if (!DropCellFinder.IsGoodDropSpot(c, <>c__DisplayClass7_.map, <>c__DisplayClass7_.allowFogged, <>c__DisplayClass7_.canRoofPunch, <>c__DisplayClass7_.allowIndoors))
				{
					return false;
				}
				IL_93:
				return <>c__DisplayClass7_.map.reachability.CanReach(<>c__DisplayClass7_.center, c, PathEndMode.OnCell, TraverseMode.PassDoors, Danger.Deadly);
			};
			if ((<>c__DisplayClass7_.allowIndoors & <>c__DisplayClass7_.canRoofPunch) && <>c__DisplayClass7_.centerRoom != null && !<>c__DisplayClass7_.centerRoom.PsychologicallyOutdoors)
			{
				Predicate<IntVec3> v = (IntVec3 c) => <>c__DisplayClass7_.validator(c) && c.GetRoom(<>c__DisplayClass7_.map, RegionType.Set_Passable) == <>c__DisplayClass7_.centerRoom;
				if (<>c__DisplayClass7_.<TryFindDropSpotNear>g__TryFindCell|1(v, out result))
				{
					return true;
				}
				Predicate<IntVec3> v2 = delegate(IntVec3 c)
				{
					if (!<>c__DisplayClass7_.validator(c))
					{
						return false;
					}
					Room room = c.GetRoom(<>c__DisplayClass7_.map, RegionType.Set_Passable);
					return room != null && !room.PsychologicallyOutdoors;
				};
				if (<>c__DisplayClass7_.<TryFindDropSpotNear>g__TryFindCell|1(v2, out result))
				{
					return true;
				}
			}
			return <>c__DisplayClass7_.<TryFindDropSpotNear>g__TryFindCell|1(<>c__DisplayClass7_.validator, out result);
		}

		// Token: 0x06005F77 RID: 24439 RVA: 0x002109D4 File Offset: 0x0020EBD4
		public static bool IsGoodDropSpot(IntVec3 c, Map map, bool allowFogged, bool canRoofPunch, bool allowIndoors = true)
		{
			if (!c.InBounds(map) || !c.Standable(map))
			{
				return false;
			}
			if (!DropCellFinder.CanPhysicallyDropInto(c, map, canRoofPunch, allowIndoors))
			{
				if (DebugViewSettings.drawDestSearch)
				{
					map.debugDrawer.FlashCell(c, 0f, "phys", 50);
				}
				return false;
			}
			if (Current.ProgramState == ProgramState.Playing && !allowFogged && c.Fogged(map))
			{
				return false;
			}
			List<Thing> thingList = c.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				Thing thing = thingList[i];
				if (thing is IActiveDropPod || thing is Skyfaller)
				{
					return false;
				}
				if (thing.def.IsEdifice())
				{
					return false;
				}
				if (thing.def.category != ThingCategory.Plant && GenSpawn.SpawningWipes(ThingDefOf.ActiveDropPod, thing.def))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06005F78 RID: 24440 RVA: 0x00210AA0 File Offset: 0x0020ECA0
		private static bool AnyAdjacentGoodDropSpot(IntVec3 c, Map map, bool allowFogged, bool canRoofPunch)
		{
			return DropCellFinder.IsGoodDropSpot(c + IntVec3.North, map, allowFogged, canRoofPunch, true) || DropCellFinder.IsGoodDropSpot(c + IntVec3.East, map, allowFogged, canRoofPunch, true) || DropCellFinder.IsGoodDropSpot(c + IntVec3.South, map, allowFogged, canRoofPunch, true) || DropCellFinder.IsGoodDropSpot(c + IntVec3.West, map, allowFogged, canRoofPunch, true);
		}

		// Token: 0x06005F79 RID: 24441 RVA: 0x00210B05 File Offset: 0x0020ED05
		[Obsolete]
		public static IntVec3 FindRaidDropCenterDistant(Map map)
		{
			return DropCellFinder.FindRaidDropCenterDistant_NewTemp(map, false);
		}

		// Token: 0x06005F7A RID: 24442 RVA: 0x00210B10 File Offset: 0x0020ED10
		public static IntVec3 FindRaidDropCenterDistant_NewTemp(Map map, bool allowRoofed = false)
		{
			Faction hostFaction = map.ParentFaction ?? Faction.OfPlayer;
			IEnumerable<Thing> enumerable = map.mapPawns.FreeHumanlikesSpawnedOfFaction(hostFaction).Cast<Thing>();
			if (hostFaction == Faction.OfPlayer)
			{
				enumerable = enumerable.Concat(map.listerBuildings.allBuildingsColonist.Cast<Thing>());
			}
			else
			{
				enumerable = enumerable.Concat(from x in map.listerThings.ThingsInGroup(ThingRequestGroup.BuildingArtificial)
				where x.Faction == hostFaction
				select x);
			}
			int num = 0;
			float num2 = 65f;
			IntVec3 intVec;
			for (;;)
			{
				intVec = CellFinder.RandomCell(map);
				num++;
				if (DropCellFinder.CanPhysicallyDropInto(intVec, map, true, false) && !intVec.Fogged(map))
				{
					if (num > 300)
					{
						break;
					}
					if (allowRoofed || !intVec.Roofed(map))
					{
						num2 -= 0.2f;
						bool flag = false;
						foreach (Thing thing in enumerable)
						{
							if ((float)(intVec - thing.Position).LengthHorizontalSquared < num2 * num2)
							{
								flag = true;
								break;
							}
						}
						if (!flag && map.reachability.CanReachFactionBase(intVec, hostFaction))
						{
							return intVec;
						}
					}
				}
			}
			return intVec;
		}

		// Token: 0x06005F7B RID: 24443 RVA: 0x00210C64 File Offset: 0x0020EE64
		public static bool TryFindRaidDropCenterClose(out IntVec3 spot, Map map, bool canRoofPunch = true, bool allowIndoors = true, bool closeWalk = true, int maxRadius = -1)
		{
			Faction parentFaction = map.ParentFaction;
			if (parentFaction == null)
			{
				return RCellFinder.TryFindRandomCellNearTheCenterOfTheMapWith((IntVec3 x) => DropCellFinder.CanPhysicallyDropInto(x, map, canRoofPunch, allowIndoors) && !x.Fogged(map) && x.Standable(map), map, out spot);
			}
			int num = 0;
			Predicate<IntVec3> <>9__1;
			for (;;)
			{
				IntVec3 root = IntVec3.Invalid;
				if (map.mapPawns.FreeHumanlikesSpawnedOfFaction(parentFaction).Count<Pawn>() > 0)
				{
					root = map.mapPawns.FreeHumanlikesSpawnedOfFaction(parentFaction).RandomElement<Pawn>().Position;
				}
				else
				{
					if (parentFaction == Faction.OfPlayer)
					{
						List<Building> allBuildingsColonist = map.listerBuildings.allBuildingsColonist;
						for (int i = 0; i < allBuildingsColonist.Count; i++)
						{
							if (DropCellFinder.TryFindDropSpotNear(allBuildingsColonist[i].Position, map, out root, true, canRoofPunch, allowIndoors, null))
							{
								break;
							}
						}
					}
					else
					{
						List<Thing> list = map.listerThings.ThingsInGroup(ThingRequestGroup.BuildingArtificial);
						int num2 = 0;
						while (num2 < list.Count && (list[num2].Faction != parentFaction || !DropCellFinder.TryFindDropSpotNear(list[num2].Position, map, out root, true, canRoofPunch, allowIndoors, null)))
						{
							num2++;
						}
					}
					if (!root.IsValid)
					{
						Predicate<IntVec3> validator;
						if ((validator = <>9__1) == null)
						{
							validator = (<>9__1 = ((IntVec3 x) => DropCellFinder.CanPhysicallyDropInto(x, map, canRoofPunch, allowIndoors) && !x.Fogged(map) && x.Standable(map)));
						}
						RCellFinder.TryFindRandomCellNearTheCenterOfTheMapWith(validator, map, out root);
					}
				}
				int num3 = (maxRadius >= 0) ? maxRadius : 10;
				if (!closeWalk)
				{
					CellFinder.TryFindRandomCellNear(root, map, num3 * num3, null, out spot, 50);
				}
				else
				{
					spot = CellFinder.RandomClosewalkCellNear(root, map, num3, null);
				}
				if (DropCellFinder.CanPhysicallyDropInto(spot, map, canRoofPunch, allowIndoors) && !spot.Fogged(map))
				{
					break;
				}
				num++;
				if (num > 300)
				{
					goto Block_13;
				}
			}
			return true;
			Block_13:
			Predicate<IntVec3> <>9__2;
			Predicate<IntVec3> validator2;
			if ((validator2 = <>9__2) == null)
			{
				validator2 = (<>9__2 = ((IntVec3 c) => DropCellFinder.CanPhysicallyDropInto(c, map, canRoofPunch, allowIndoors)));
			}
			spot = CellFinderLoose.RandomCellWith(validator2, map, 1000);
			return false;
		}

		// Token: 0x06005F7C RID: 24444 RVA: 0x00210EC0 File Offset: 0x0020F0C0
		public static bool FindSafeLandingSpot(out IntVec3 spot, Faction faction, Map map, int distToHostiles = 35, int distToFires = 15, int distToEdge = 25, IntVec2? size = null)
		{
			spot = IntVec3.Invalid;
			int num = 200;
			while (num-- > 0)
			{
				IntVec3 intVec = DropCellFinder.RandomDropSpot(map);
				if (DropCellFinder.IsSafeDropSpot(intVec, map, faction, size, distToEdge, distToHostiles, distToFires))
				{
					spot = intVec;
					return true;
				}
			}
			return false;
		}

		// Token: 0x06005F7D RID: 24445 RVA: 0x00210F0C File Offset: 0x0020F10C
		public static bool CanPhysicallyDropInto(IntVec3 c, Map map, bool canRoofPunch, bool allowedIndoors = true)
		{
			if (!c.Walkable(map))
			{
				return false;
			}
			RoofDef roof = c.GetRoof(map);
			if (roof != null)
			{
				if (!canRoofPunch)
				{
					return false;
				}
				if (roof.isThickRoof)
				{
					return false;
				}
			}
			if (!allowedIndoors)
			{
				Room room = c.GetRoom(map, RegionType.Set_Passable);
				if (room != null && !room.PsychologicallyOutdoors)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06005F7E RID: 24446 RVA: 0x00210F58 File Offset: 0x0020F158
		private static bool IsSafeDropSpot(IntVec3 cell, Map map, Faction faction, IntVec2? size = null, int distToEdge = 25, int distToHostiles = 35, int distToFires = 15)
		{
			DropCellFinder.<>c__DisplayClass15_0 <>c__DisplayClass15_ = new DropCellFinder.<>c__DisplayClass15_0();
			<>c__DisplayClass15_.map = map;
			<>c__DisplayClass15_.cell = cell;
			Faction factionBaseFaction = <>c__DisplayClass15_.map.ParentFaction ?? Faction.OfPlayer;
			if (size != null)
			{
				using (CellRect.Enumerator enumerator = GenAdj.OccupiedRect(<>c__DisplayClass15_.cell, Rot4.North, size.Value).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (!DropCellFinder.IsGoodDropSpot(enumerator.Current, <>c__DisplayClass15_.map, false, false, false))
						{
							return false;
						}
					}
					goto IL_A4;
				}
			}
			if (!DropCellFinder.IsGoodDropSpot(<>c__DisplayClass15_.cell, <>c__DisplayClass15_.map, false, false, false))
			{
				return false;
			}
			IL_A4:
			if (distToEdge > 0 && <>c__DisplayClass15_.cell.CloseToEdge(<>c__DisplayClass15_.map, distToEdge))
			{
				return false;
			}
			if (faction != null)
			{
				foreach (IAttackTarget attackTarget in <>c__DisplayClass15_.map.attackTargetsCache.TargetsHostileToFaction(faction))
				{
					if (!attackTarget.ThreatDisabled(null) && attackTarget.Thing.Position.InHorDistOf(<>c__DisplayClass15_.cell, (float)distToHostiles))
					{
						return false;
					}
				}
			}
			if (!<>c__DisplayClass15_.map.reachability.CanReachFactionBase(<>c__DisplayClass15_.cell, factionBaseFaction))
			{
				return false;
			}
			if (size != null)
			{
				using (IEnumerator<IntVec3> enumerator3 = CellRect.CenteredOn(<>c__DisplayClass15_.cell, size.Value.x, size.Value.z).Cells.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						IntVec3 c = enumerator3.Current;
						if (<>c__DisplayClass15_.<IsSafeDropSpot>g__CellHasCrops|0(c))
						{
							return false;
						}
					}
					goto IL_1CC;
				}
			}
			if (<>c__DisplayClass15_.<IsSafeDropSpot>g__CellHasCrops|0(<>c__DisplayClass15_.cell))
			{
				return false;
			}
			IL_1CC:
			<>c__DisplayClass15_.minDistToFiresSq = (float)(distToFires * distToFires);
			<>c__DisplayClass15_.closestDistSq = float.MaxValue;
			<>c__DisplayClass15_.firesCount = 0;
			RegionTraverser.BreadthFirstTraverse(<>c__DisplayClass15_.cell, <>c__DisplayClass15_.map, (Region from, Region to) => true, delegate(Region x)
			{
				List<Thing> list = x.ListerThings.ThingsInGroup(ThingRequestGroup.Fire);
				for (int i = 0; i < list.Count; i++)
				{
					float num = (float)<>c__DisplayClass15_.cell.DistanceToSquared(list[i].Position);
					if (num <= <>c__DisplayClass15_.minDistToFiresSq)
					{
						if (num < <>c__DisplayClass15_.closestDistSq)
						{
							<>c__DisplayClass15_.closestDistSq = num;
						}
						int firesCount = <>c__DisplayClass15_.firesCount;
						<>c__DisplayClass15_.firesCount = firesCount + 1;
					}
				}
				return <>c__DisplayClass15_.closestDistSq <= <>c__DisplayClass15_.minDistToFiresSq && <>c__DisplayClass15_.firesCount >= 5;
			}, 15, RegionType.Set_Passable);
			return <>c__DisplayClass15_.closestDistSq > <>c__DisplayClass15_.minDistToFiresSq || <>c__DisplayClass15_.firesCount < 5;
		}

		// Token: 0x040033D3 RID: 13267
		private static List<Building> tmpColonyBuildings = new List<Building>();

		// Token: 0x040033D4 RID: 13268
		public static List<ShipLandingArea> tmpShipLandingAreas = new List<ShipLandingArea>();
	}
}
