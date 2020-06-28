using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x02000451 RID: 1105
	public static class GenSpawn
	{
		// Token: 0x06002105 RID: 8453 RVA: 0x000CA4C9 File Offset: 0x000C86C9
		public static Thing Spawn(ThingDef def, IntVec3 loc, Map map, WipeMode wipeMode = WipeMode.Vanish)
		{
			return GenSpawn.Spawn(ThingMaker.MakeThing(def, null), loc, map, wipeMode);
		}

		// Token: 0x06002106 RID: 8454 RVA: 0x000CA4DA File Offset: 0x000C86DA
		public static Thing Spawn(Thing newThing, IntVec3 loc, Map map, WipeMode wipeMode = WipeMode.Vanish)
		{
			return GenSpawn.Spawn(newThing, loc, map, Rot4.North, wipeMode, false);
		}

		// Token: 0x06002107 RID: 8455 RVA: 0x000CA4EC File Offset: 0x000C86EC
		public static Thing Spawn(Thing newThing, IntVec3 loc, Map map, Rot4 rot, WipeMode wipeMode = WipeMode.Vanish, bool respawningAfterLoad = false)
		{
			if (map == null)
			{
				Log.Error("Tried to spawn " + newThing.ToStringSafe<Thing>() + " in a null map.", false);
				return null;
			}
			if (!loc.InBounds(map))
			{
				Log.Error(string.Concat(new object[]
				{
					"Tried to spawn ",
					newThing.ToStringSafe<Thing>(),
					" out of bounds at ",
					loc,
					"."
				}), false);
				return null;
			}
			if (newThing.def.randomizeRotationOnSpawn)
			{
				rot = Rot4.Random;
			}
			CellRect occupiedRect = GenAdj.OccupiedRect(loc, rot, newThing.def.Size);
			if (!occupiedRect.InBounds(map))
			{
				Log.Error(string.Concat(new object[]
				{
					"Tried to spawn ",
					newThing.ToStringSafe<Thing>(),
					" out of bounds at ",
					loc,
					" (out of bounds because size is ",
					newThing.def.Size,
					")."
				}), false);
				return null;
			}
			if (newThing.Spawned)
			{
				Log.Error("Tried to spawn " + newThing + " but it's already spawned.", false);
				return newThing;
			}
			if (wipeMode == WipeMode.Vanish)
			{
				GenSpawn.WipeExistingThings(loc, rot, newThing.def, map, DestroyMode.Vanish);
			}
			else if (wipeMode == WipeMode.FullRefund)
			{
				GenSpawn.WipeAndRefundExistingThings(loc, rot, newThing.def, map);
			}
			else if (wipeMode == WipeMode.VanishOrMoveAside)
			{
				GenSpawn.CheckMoveItemsAside(loc, rot, newThing.def, map);
				GenSpawn.WipeExistingThings(loc, rot, newThing.def, map, DestroyMode.Vanish);
			}
			if (newThing.def.category == ThingCategory.Item)
			{
				Predicate<IntVec3> <>9__0;
				foreach (IntVec3 intVec in occupiedRect)
				{
					foreach (Thing thing in intVec.GetThingList(map).ToList<Thing>())
					{
						if (thing != newThing && thing.def.category == ThingCategory.Item)
						{
							thing.DeSpawn(DestroyMode.Vanish);
							Thing thing2 = thing;
							IntVec3 center = intVec;
							ThingPlaceMode mode = ThingPlaceMode.Near;
							Action<Thing, int> placedAction = null;
							Predicate<IntVec3> nearPlaceValidator;
							if ((nearPlaceValidator = <>9__0) == null)
							{
								nearPlaceValidator = (<>9__0 = ((IntVec3 x) => !occupiedRect.Contains(x)));
							}
							if (!GenPlace.TryPlaceThing(thing2, center, map, mode, placedAction, nearPlaceValidator, default(Rot4)))
							{
								thing.Destroy(DestroyMode.Vanish);
							}
						}
					}
				}
			}
			newThing.Rotation = rot;
			newThing.Position = loc;
			if (newThing.holdingOwner != null)
			{
				newThing.holdingOwner.Remove(newThing);
			}
			newThing.SpawnSetup(map, respawningAfterLoad);
			if (newThing.Spawned && newThing.stackCount == 0)
			{
				Log.Error("Spawned thing with 0 stackCount: " + newThing, false);
				newThing.Destroy(DestroyMode.Vanish);
				return null;
			}
			if (newThing.def.passability == Traversability.Impassable)
			{
				foreach (IntVec3 c in occupiedRect)
				{
					foreach (Thing thing3 in c.GetThingList(map).ToList<Thing>())
					{
						if (thing3 != newThing)
						{
							Pawn pawn = thing3 as Pawn;
							if (pawn != null)
							{
								pawn.pather.TryRecoverFromUnwalkablePosition(false);
							}
						}
					}
				}
			}
			return newThing;
		}

		// Token: 0x06002108 RID: 8456 RVA: 0x000CA85C File Offset: 0x000C8A5C
		public static void SpawnBuildingAsPossible(Building building, Map map, bool respawningAfterLoad = false)
		{
			bool flag = false;
			if (!building.OccupiedRect().InBounds(map))
			{
				flag = true;
			}
			else
			{
				foreach (IntVec3 c in building.OccupiedRect())
				{
					List<Thing> thingList = c.GetThingList(map);
					for (int i = 0; i < thingList.Count; i++)
					{
						if (thingList[i] is Pawn && building.def.passability == Traversability.Impassable)
						{
							flag = true;
							break;
						}
						if ((thingList[i].def.category == ThingCategory.Building || thingList[i].def.category == ThingCategory.Item) && GenSpawn.SpawningWipes(building.def, thingList[i].def))
						{
							flag = true;
							break;
						}
					}
					if (flag)
					{
						break;
					}
				}
			}
			if (flag)
			{
				GenSpawn.Refund(building, map, CellRect.Empty);
				return;
			}
			GenSpawn.Spawn(building, building.Position, map, building.Rotation, WipeMode.FullRefund, respawningAfterLoad);
		}

		// Token: 0x06002109 RID: 8457 RVA: 0x000CA97C File Offset: 0x000C8B7C
		public static void Refund(Thing thing, Map map, CellRect avoidThisRect)
		{
			bool flag = false;
			if (thing.def.Minifiable)
			{
				MinifiedThing minifiedThing = thing.MakeMinified();
				if (GenPlace.TryPlaceThing(minifiedThing, thing.Position, map, ThingPlaceMode.Near, null, (IntVec3 x) => !avoidThisRect.Contains(x), default(Rot4)))
				{
					flag = true;
				}
				else
				{
					minifiedThing.GetDirectlyHeldThings().Clear();
					minifiedThing.Destroy(DestroyMode.Vanish);
				}
			}
			if (!flag)
			{
				GenLeaving.DoLeavingsFor(thing, map, DestroyMode.Refund, thing.OccupiedRect(), (IntVec3 x) => !avoidThisRect.Contains(x), null);
				thing.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x0600210A RID: 8458 RVA: 0x000CAA10 File Offset: 0x000C8C10
		public static void WipeExistingThings(IntVec3 thingPos, Rot4 thingRot, BuildableDef thingDef, Map map, DestroyMode mode)
		{
			foreach (IntVec3 c in GenAdj.CellsOccupiedBy(thingPos, thingRot, thingDef.Size))
			{
				foreach (Thing thing in map.thingGrid.ThingsAt(c).ToList<Thing>())
				{
					if (GenSpawn.SpawningWipes(thingDef, thing.def))
					{
						thing.Destroy(mode);
					}
				}
			}
		}

		// Token: 0x0600210B RID: 8459 RVA: 0x000CAABC File Offset: 0x000C8CBC
		public static void WipeAndRefundExistingThings(IntVec3 thingPos, Rot4 thingRot, BuildableDef thingDef, Map map)
		{
			CellRect occupiedRect = GenAdj.OccupiedRect(thingPos, thingRot, thingDef.Size);
			Predicate<IntVec3> <>9__0;
			foreach (IntVec3 intVec in occupiedRect)
			{
				foreach (Thing thing in intVec.GetThingList(map).ToList<Thing>())
				{
					if (GenSpawn.SpawningWipes(thingDef, thing.def))
					{
						if (thing.def.category == ThingCategory.Item)
						{
							thing.DeSpawn(DestroyMode.Vanish);
							Thing thing2 = thing;
							IntVec3 center = intVec;
							ThingPlaceMode mode = ThingPlaceMode.Near;
							Action<Thing, int> placedAction = null;
							Predicate<IntVec3> nearPlaceValidator;
							if ((nearPlaceValidator = <>9__0) == null)
							{
								nearPlaceValidator = (<>9__0 = ((IntVec3 x) => !occupiedRect.Contains(x)));
							}
							if (!GenPlace.TryPlaceThing(thing2, center, map, mode, placedAction, nearPlaceValidator, default(Rot4)))
							{
								thing.Destroy(DestroyMode.Vanish);
							}
						}
						else
						{
							GenSpawn.Refund(thing, map, occupiedRect);
						}
					}
				}
			}
		}

		// Token: 0x0600210C RID: 8460 RVA: 0x000CABE4 File Offset: 0x000C8DE4
		public static void CheckMoveItemsAside(IntVec3 thingPos, Rot4 thingRot, ThingDef thingDef, Map map)
		{
			if (thingDef.surfaceType != SurfaceType.None || thingDef.passability == Traversability.Standable)
			{
				return;
			}
			CellRect occupiedRect = GenAdj.OccupiedRect(thingPos, thingRot, thingDef.Size);
			Predicate<IntVec3> <>9__0;
			foreach (IntVec3 intVec in occupiedRect)
			{
				foreach (Thing thing in intVec.GetThingList(map).ToList<Thing>())
				{
					if (thing.def.category == ThingCategory.Item)
					{
						thing.DeSpawn(DestroyMode.Vanish);
						Thing thing2 = thing;
						IntVec3 center = intVec;
						ThingPlaceMode mode = ThingPlaceMode.Near;
						Action<Thing, int> placedAction = null;
						Predicate<IntVec3> nearPlaceValidator;
						if ((nearPlaceValidator = <>9__0) == null)
						{
							nearPlaceValidator = (<>9__0 = ((IntVec3 x) => !occupiedRect.Contains(x)));
						}
						if (!GenPlace.TryPlaceThing(thing2, center, map, mode, placedAction, nearPlaceValidator, default(Rot4)))
						{
							thing.Destroy(DestroyMode.Vanish);
						}
					}
				}
			}
		}

		// Token: 0x0600210D RID: 8461 RVA: 0x000CACFC File Offset: 0x000C8EFC
		public static bool WouldWipeAnythingWith(IntVec3 thingPos, Rot4 thingRot, BuildableDef thingDef, Map map, Predicate<Thing> predicate)
		{
			return GenSpawn.WouldWipeAnythingWith(GenAdj.OccupiedRect(thingPos, thingRot, thingDef.Size), thingDef, map, predicate);
		}

		// Token: 0x0600210E RID: 8462 RVA: 0x000CAD14 File Offset: 0x000C8F14
		public static bool WouldWipeAnythingWith(CellRect cellRect, BuildableDef thingDef, Map map, Predicate<Thing> predicate)
		{
			foreach (IntVec3 c in cellRect)
			{
				foreach (Thing thing in map.thingGrid.ThingsAt(c).ToList<Thing>())
				{
					if (GenSpawn.SpawningWipes(thingDef, thing.def) && predicate(thing))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600210F RID: 8463 RVA: 0x000CADC4 File Offset: 0x000C8FC4
		public static bool SpawningWipes(BuildableDef newEntDef, BuildableDef oldEntDef)
		{
			ThingDef thingDef = newEntDef as ThingDef;
			ThingDef thingDef2 = oldEntDef as ThingDef;
			if (thingDef == null || thingDef2 == null)
			{
				return false;
			}
			if (thingDef.category == ThingCategory.Attachment || thingDef.category == ThingCategory.Mote || thingDef.category == ThingCategory.Filth || thingDef.category == ThingCategory.Projectile)
			{
				return false;
			}
			if (!thingDef2.destroyable)
			{
				return false;
			}
			if (thingDef.category == ThingCategory.Plant)
			{
				return false;
			}
			if (thingDef2.category == ThingCategory.Filth && thingDef.passability != Traversability.Standable)
			{
				return true;
			}
			if (thingDef2.category == ThingCategory.Item && thingDef.passability == Traversability.Impassable && thingDef.surfaceType == SurfaceType.None)
			{
				return true;
			}
			if (thingDef.EverTransmitsPower && thingDef2 == ThingDefOf.PowerConduit)
			{
				return true;
			}
			if (thingDef.IsFrame && GenSpawn.SpawningWipes(thingDef.entityDefToBuild, oldEntDef))
			{
				return true;
			}
			BuildableDef buildableDef = GenConstruct.BuiltDefOf(thingDef);
			BuildableDef buildableDef2 = GenConstruct.BuiltDefOf(thingDef2);
			if (buildableDef == null || buildableDef2 == null)
			{
				return false;
			}
			ThingDef thingDef3 = thingDef.entityDefToBuild as ThingDef;
			if (thingDef2.IsBlueprint)
			{
				if (thingDef.IsBlueprint)
				{
					if (thingDef3 != null && thingDef3.building != null && thingDef3.building.canPlaceOverWall && thingDef2.entityDefToBuild is ThingDef && (ThingDef)thingDef2.entityDefToBuild == ThingDefOf.Wall)
					{
						return true;
					}
					if (thingDef2.entityDefToBuild is TerrainDef)
					{
						if (thingDef.entityDefToBuild is ThingDef && ((ThingDef)thingDef.entityDefToBuild).coversFloor)
						{
							return true;
						}
						if (thingDef.entityDefToBuild is TerrainDef)
						{
							return true;
						}
					}
				}
				return thingDef2.entityDefToBuild == ThingDefOf.PowerConduit && thingDef.entityDefToBuild is ThingDef && (thingDef.entityDefToBuild as ThingDef).EverTransmitsPower;
			}
			if ((thingDef2.IsFrame || thingDef2.IsBlueprint) && thingDef2.entityDefToBuild is TerrainDef)
			{
				ThingDef thingDef4 = buildableDef as ThingDef;
				if (thingDef4 != null && !thingDef4.CoexistsWithFloors)
				{
					return true;
				}
			}
			if (thingDef2 == ThingDefOf.ActiveDropPod || thingDef == ThingDefOf.ActiveDropPod)
			{
				return false;
			}
			if (thingDef.IsEdifice())
			{
				if (thingDef.BlockPlanting && thingDef2.category == ThingCategory.Plant)
				{
					return true;
				}
				if (!(buildableDef is TerrainDef) && buildableDef2.IsEdifice())
				{
					return true;
				}
			}
			return false;
		}
	}
}
