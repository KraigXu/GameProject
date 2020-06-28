using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A87 RID: 2695
	public static class PowerNetMaker
	{
		// Token: 0x06003FB4 RID: 16308 RVA: 0x00153000 File Offset: 0x00151200
		private static IEnumerable<CompPower> ContiguousPowerBuildings(Building root)
		{
			PowerNetMaker.closedSet.Clear();
			PowerNetMaker.openSet.Clear();
			PowerNetMaker.currentSet.Clear();
			PowerNetMaker.openSet.Add(root);
			do
			{
				foreach (Building item in PowerNetMaker.openSet)
				{
					PowerNetMaker.closedSet.Add(item);
				}
				HashSet<Building> hashSet = PowerNetMaker.currentSet;
				PowerNetMaker.currentSet = PowerNetMaker.openSet;
				PowerNetMaker.openSet = hashSet;
				PowerNetMaker.openSet.Clear();
				foreach (Building building in PowerNetMaker.currentSet)
				{
					foreach (IntVec3 c in GenAdj.CellsAdjacentCardinal(building))
					{
						if (c.InBounds(building.Map))
						{
							List<Thing> thingList = c.GetThingList(building.Map);
							for (int i = 0; i < thingList.Count; i++)
							{
								Building building2 = thingList[i] as Building;
								if (building2 != null && building2.TransmitsPowerNow && !PowerNetMaker.openSet.Contains(building2) && !PowerNetMaker.currentSet.Contains(building2) && !PowerNetMaker.closedSet.Contains(building2))
								{
									PowerNetMaker.openSet.Add(building2);
									break;
								}
							}
						}
					}
				}
			}
			while (PowerNetMaker.openSet.Count > 0);
			IEnumerable<CompPower> result = (from b in PowerNetMaker.closedSet
			select b.PowerComp).ToArray<CompPower>();
			PowerNetMaker.closedSet.Clear();
			PowerNetMaker.openSet.Clear();
			PowerNetMaker.currentSet.Clear();
			return result;
		}

		// Token: 0x06003FB5 RID: 16309 RVA: 0x00153204 File Offset: 0x00151404
		public static PowerNet NewPowerNetStartingFrom(Building root)
		{
			return new PowerNet(PowerNetMaker.ContiguousPowerBuildings(root));
		}

		// Token: 0x06003FB6 RID: 16310 RVA: 0x00002681 File Offset: 0x00000881
		public static void UpdateVisualLinkagesFor(PowerNet net)
		{
		}

		// Token: 0x0400251C RID: 9500
		private static HashSet<Building> closedSet = new HashSet<Building>();

		// Token: 0x0400251D RID: 9501
		private static HashSet<Building> openSet = new HashSet<Building>();

		// Token: 0x0400251E RID: 9502
		private static HashSet<Building> currentSet = new HashSet<Building>();
	}
}
