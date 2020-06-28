using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020010B3 RID: 4275
	public class SymbolResolver_EnsureCanReachMapEdge : SymbolResolver
	{
		// Token: 0x06006518 RID: 25880 RVA: 0x00234234 File Offset: 0x00232434
		public override void Resolve(ResolveParams rp)
		{
			SymbolResolver_EnsureCanReachMapEdge.cellsInRandomOrder.Clear();
			foreach (IntVec3 item in rp.rect)
			{
				SymbolResolver_EnsureCanReachMapEdge.cellsInRandomOrder.Add(item);
			}
			SymbolResolver_EnsureCanReachMapEdge.cellsInRandomOrder.Shuffle<IntVec3>();
			this.TryMakeAllCellsReachable(false, rp);
			this.TryMakeAllCellsReachable(true, rp);
		}

		// Token: 0x06006519 RID: 25881 RVA: 0x002342B0 File Offset: 0x002324B0
		private void TryMakeAllCellsReachable(bool canPathThroughNonStandable, ResolveParams rp)
		{
			Map map = BaseGen.globalSettings.map;
			SymbolResolver_EnsureCanReachMapEdge.visited.Clear();
			for (int i = 0; i < SymbolResolver_EnsureCanReachMapEdge.cellsInRandomOrder.Count; i++)
			{
				IntVec3 intVec = SymbolResolver_EnsureCanReachMapEdge.cellsInRandomOrder[i];
				if (this.CanTraverse(intVec, canPathThroughNonStandable))
				{
					Room room = intVec.GetRoom(map, RegionType.Set_Passable);
					if (room != null && !SymbolResolver_EnsureCanReachMapEdge.visited.Contains(room))
					{
						SymbolResolver_EnsureCanReachMapEdge.visited.Add(room);
						TraverseParms traverseParms = TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false);
						if (!map.reachability.CanReachMapEdge(intVec, traverseParms))
						{
							bool found = false;
							IntVec3 foundDest = IntVec3.Invalid;
							map.floodFiller.FloodFill(intVec, (IntVec3 x) => !found && this.CanTraverse(x, canPathThroughNonStandable), delegate(IntVec3 x)
							{
								if (found)
								{
									return;
								}
								if (map.reachability.CanReachMapEdge(x, traverseParms))
								{
									found = true;
									foundDest = x;
								}
							}, int.MaxValue, true, null);
							if (found)
							{
								this.ReconstructPathAndDestroyWalls(foundDest, room, rp);
							}
						}
						room = intVec.GetRoom(map, RegionType.Set_Passable);
						if (room != null)
						{
							RegionTraverser.BreadthFirstTraverse(room.Regions[0], (Region from, Region r) => r.Allows(traverseParms, false), delegate(Region r)
							{
								if (r.Room.TouchesMapEdge)
								{
									MapGenerator.rootsToUnfog.Add(r.AnyCell);
									return true;
								}
								return false;
							}, 9999, RegionType.Set_Passable);
						}
					}
				}
			}
			SymbolResolver_EnsureCanReachMapEdge.visited.Clear();
		}

		// Token: 0x0600651A RID: 25882 RVA: 0x0023447C File Offset: 0x0023267C
		private void ReconstructPathAndDestroyWalls(IntVec3 foundDest, Room room, ResolveParams rp)
		{
			Map map = BaseGen.globalSettings.map;
			map.floodFiller.ReconstructLastFloodFillPath(foundDest, SymbolResolver_EnsureCanReachMapEdge.path);
			while (SymbolResolver_EnsureCanReachMapEdge.path.Count >= 2 && SymbolResolver_EnsureCanReachMapEdge.path[0].AdjacentToCardinal(room) && SymbolResolver_EnsureCanReachMapEdge.path[1].AdjacentToCardinal(room))
			{
				SymbolResolver_EnsureCanReachMapEdge.path.RemoveAt(0);
			}
			IntVec3 intVec = IntVec3.Invalid;
			ThingDef thingDef = null;
			IntVec3 intVec2 = IntVec3.Invalid;
			ThingDef thingDef2 = null;
			for (int i = 0; i < SymbolResolver_EnsureCanReachMapEdge.path.Count; i++)
			{
				Building edifice = SymbolResolver_EnsureCanReachMapEdge.path[i].GetEdifice(map);
				if (this.IsWallOrRock(edifice))
				{
					if (!intVec.IsValid)
					{
						intVec = SymbolResolver_EnsureCanReachMapEdge.path[i];
						thingDef = edifice.Stuff;
					}
					intVec2 = SymbolResolver_EnsureCanReachMapEdge.path[i];
					thingDef2 = edifice.Stuff;
					edifice.Destroy(DestroyMode.Vanish);
				}
			}
			if (intVec.IsValid)
			{
				ThingDef thingDef3;
				if ((thingDef3 = thingDef) == null)
				{
					thingDef3 = (rp.wallStuff ?? BaseGenUtility.RandomCheapWallStuff(rp.faction, false));
				}
				ThingDef stuff = thingDef3;
				Thing thing = ThingMaker.MakeThing(ThingDefOf.Door, stuff);
				thing.SetFaction(rp.faction, null);
				GenSpawn.Spawn(thing, intVec, map, WipeMode.Vanish);
			}
			if (intVec2.IsValid && intVec2 != intVec && !intVec2.AdjacentToCardinal(intVec))
			{
				ThingDef thingDef4;
				if ((thingDef4 = thingDef2) == null)
				{
					thingDef4 = (rp.wallStuff ?? BaseGenUtility.RandomCheapWallStuff(rp.faction, false));
				}
				ThingDef stuff2 = thingDef4;
				Thing thing2 = ThingMaker.MakeThing(ThingDefOf.Door, stuff2);
				thing2.SetFaction(rp.faction, null);
				GenSpawn.Spawn(thing2, intVec2, map, WipeMode.Vanish);
			}
		}

		// Token: 0x0600651B RID: 25883 RVA: 0x00234618 File Offset: 0x00232818
		private bool CanTraverse(IntVec3 c, bool canPathThroughNonStandable)
		{
			Map map = BaseGen.globalSettings.map;
			Building edifice = c.GetEdifice(map);
			return this.IsWallOrRock(edifice) || ((canPathThroughNonStandable || c.Standable(map)) && !c.Impassable(map));
		}

		// Token: 0x0600651C RID: 25884 RVA: 0x0023465D File Offset: 0x0023285D
		private bool IsWallOrRock(Building b)
		{
			return b != null && (b.def == ThingDefOf.Wall || b.def.building.isNaturalRock);
		}

		// Token: 0x04003DB1 RID: 15793
		private static HashSet<Room> visited = new HashSet<Room>();

		// Token: 0x04003DB2 RID: 15794
		private static List<IntVec3> path = new List<IntVec3>();

		// Token: 0x04003DB3 RID: 15795
		private static List<IntVec3> cellsInRandomOrder = new List<IntVec3>();
	}
}
