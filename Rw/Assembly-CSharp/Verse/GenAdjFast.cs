using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000019 RID: 25
	public static class GenAdjFast
	{
		// Token: 0x060001AA RID: 426 RVA: 0x00007CA0 File Offset: 0x00005EA0
		public static List<IntVec3> AdjacentCells8Way(LocalTargetInfo pack)
		{
			if (pack.HasThing)
			{
				return GenAdjFast.AdjacentCells8Way((Thing)pack);
			}
			return GenAdjFast.AdjacentCells8Way((IntVec3)pack);
		}

		// Token: 0x060001AB RID: 427 RVA: 0x00007CC4 File Offset: 0x00005EC4
		public static List<IntVec3> AdjacentCells8Way(IntVec3 root)
		{
			if (GenAdjFast.working)
			{
				throw new InvalidOperationException("GenAdjFast is already working.");
			}
			GenAdjFast.resultList.Clear();
			GenAdjFast.working = true;
			for (int i = 0; i < 8; i++)
			{
				GenAdjFast.resultList.Add(root + GenAdj.AdjacentCells[i]);
			}
			GenAdjFast.working = false;
			return GenAdjFast.resultList;
		}

		// Token: 0x060001AC RID: 428 RVA: 0x00007D25 File Offset: 0x00005F25
		private static List<IntVec3> AdjacentCells8Way(Thing t)
		{
			return GenAdjFast.AdjacentCells8Way(t.Position, t.Rotation, t.def.size);
		}

		// Token: 0x060001AD RID: 429 RVA: 0x00007D44 File Offset: 0x00005F44
		public static List<IntVec3> AdjacentCells8Way(IntVec3 thingCenter, Rot4 thingRot, IntVec2 thingSize)
		{
			if (thingSize.x == 1 && thingSize.z == 1)
			{
				return GenAdjFast.AdjacentCells8Way(thingCenter);
			}
			if (GenAdjFast.working)
			{
				throw new InvalidOperationException("GenAdjFast is already working.");
			}
			GenAdjFast.resultList.Clear();
			GenAdjFast.working = true;
			GenAdj.AdjustForRotation(ref thingCenter, ref thingSize, thingRot);
			int num = thingCenter.x - (thingSize.x - 1) / 2 - 1;
			int num2 = num + thingSize.x + 1;
			int num3 = thingCenter.z - (thingSize.z - 1) / 2 - 1;
			int num4 = num3 + thingSize.z + 1;
			IntVec3 intVec = new IntVec3(num - 1, 0, num3);
			do
			{
				intVec.x++;
				GenAdjFast.resultList.Add(intVec);
			}
			while (intVec.x < num2);
			do
			{
				intVec.z++;
				GenAdjFast.resultList.Add(intVec);
			}
			while (intVec.z < num4);
			do
			{
				intVec.x--;
				GenAdjFast.resultList.Add(intVec);
			}
			while (intVec.x > num);
			do
			{
				intVec.z--;
				GenAdjFast.resultList.Add(intVec);
			}
			while (intVec.z > num3 + 1);
			GenAdjFast.working = false;
			return GenAdjFast.resultList;
		}

		// Token: 0x060001AE RID: 430 RVA: 0x00007E75 File Offset: 0x00006075
		public static List<IntVec3> AdjacentCellsCardinal(LocalTargetInfo pack)
		{
			if (pack.HasThing)
			{
				return GenAdjFast.AdjacentCellsCardinal((Thing)pack);
			}
			return GenAdjFast.AdjacentCellsCardinal((IntVec3)pack);
		}

		// Token: 0x060001AF RID: 431 RVA: 0x00007E98 File Offset: 0x00006098
		public static List<IntVec3> AdjacentCellsCardinal(IntVec3 root)
		{
			if (GenAdjFast.working)
			{
				throw new InvalidOperationException("GenAdjFast is already working.");
			}
			GenAdjFast.resultList.Clear();
			GenAdjFast.working = true;
			for (int i = 0; i < 4; i++)
			{
				GenAdjFast.resultList.Add(root + GenAdj.CardinalDirections[i]);
			}
			GenAdjFast.working = false;
			return GenAdjFast.resultList;
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x00007EF9 File Offset: 0x000060F9
		private static List<IntVec3> AdjacentCellsCardinal(Thing t)
		{
			return GenAdjFast.AdjacentCellsCardinal(t.Position, t.Rotation, t.def.size);
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x00007F18 File Offset: 0x00006118
		public static List<IntVec3> AdjacentCellsCardinal(IntVec3 thingCenter, Rot4 thingRot, IntVec2 thingSize)
		{
			if (thingSize.x == 1 && thingSize.z == 1)
			{
				return GenAdjFast.AdjacentCellsCardinal(thingCenter);
			}
			if (GenAdjFast.working)
			{
				throw new InvalidOperationException("GenAdjFast is already working.");
			}
			GenAdjFast.resultList.Clear();
			GenAdjFast.working = true;
			GenAdj.AdjustForRotation(ref thingCenter, ref thingSize, thingRot);
			int num = thingCenter.x - (thingSize.x - 1) / 2 - 1;
			int num2 = num + thingSize.x + 1;
			int num3 = thingCenter.z - (thingSize.z - 1) / 2 - 1;
			int num4 = num3 + thingSize.z + 1;
			IntVec3 intVec = new IntVec3(num, 0, num3);
			do
			{
				intVec.x++;
				GenAdjFast.resultList.Add(intVec);
			}
			while (intVec.x < num2 - 1);
			intVec.x++;
			do
			{
				intVec.z++;
				GenAdjFast.resultList.Add(intVec);
			}
			while (intVec.z < num4 - 1);
			intVec.z++;
			do
			{
				intVec.x--;
				GenAdjFast.resultList.Add(intVec);
			}
			while (intVec.x > num + 1);
			intVec.x--;
			do
			{
				intVec.z--;
				GenAdjFast.resultList.Add(intVec);
			}
			while (intVec.z > num3 + 1);
			GenAdjFast.working = false;
			return GenAdjFast.resultList;
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x00008074 File Offset: 0x00006274
		public static void AdjacentThings8Way(Thing thing, List<Thing> outThings)
		{
			outThings.Clear();
			if (!thing.Spawned)
			{
				return;
			}
			Map map = thing.Map;
			List<IntVec3> list = GenAdjFast.AdjacentCells8Way(thing);
			for (int i = 0; i < list.Count; i++)
			{
				List<Thing> thingList = list[i].GetThingList(map);
				for (int j = 0; j < thingList.Count; j++)
				{
					if (!outThings.Contains(thingList[j]))
					{
						outThings.Add(thingList[j]);
					}
				}
			}
		}

		// Token: 0x04000049 RID: 73
		private static List<IntVec3> resultList = new List<IntVec3>();

		// Token: 0x0400004A RID: 74
		private static bool working = false;
	}
}
