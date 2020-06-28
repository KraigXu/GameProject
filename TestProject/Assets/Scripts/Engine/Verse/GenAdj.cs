using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000018 RID: 24
	public static class GenAdj
	{
		// Token: 0x06000187 RID: 391 RVA: 0x00006F3C File Offset: 0x0000513C
		static GenAdj()
		{
			GenAdj.SetupAdjacencyTables();
		}

		// Token: 0x06000188 RID: 392 RVA: 0x00006FC0 File Offset: 0x000051C0
		private static void SetupAdjacencyTables()
		{
			GenAdj.CardinalDirections[0] = new IntVec3(0, 0, 1);
			GenAdj.CardinalDirections[1] = new IntVec3(1, 0, 0);
			GenAdj.CardinalDirections[2] = new IntVec3(0, 0, -1);
			GenAdj.CardinalDirections[3] = new IntVec3(-1, 0, 0);
			GenAdj.CardinalDirectionsAndInside[0] = new IntVec3(0, 0, 1);
			GenAdj.CardinalDirectionsAndInside[1] = new IntVec3(1, 0, 0);
			GenAdj.CardinalDirectionsAndInside[2] = new IntVec3(0, 0, -1);
			GenAdj.CardinalDirectionsAndInside[3] = new IntVec3(-1, 0, 0);
			GenAdj.CardinalDirectionsAndInside[4] = new IntVec3(0, 0, 0);
			GenAdj.CardinalDirectionsAround[0] = new IntVec3(0, 0, -1);
			GenAdj.CardinalDirectionsAround[1] = new IntVec3(-1, 0, 0);
			GenAdj.CardinalDirectionsAround[2] = new IntVec3(0, 0, 1);
			GenAdj.CardinalDirectionsAround[3] = new IntVec3(1, 0, 0);
			GenAdj.DiagonalDirections[0] = new IntVec3(-1, 0, -1);
			GenAdj.DiagonalDirections[1] = new IntVec3(-1, 0, 1);
			GenAdj.DiagonalDirections[2] = new IntVec3(1, 0, 1);
			GenAdj.DiagonalDirections[3] = new IntVec3(1, 0, -1);
			GenAdj.DiagonalDirectionsAround[0] = new IntVec3(-1, 0, -1);
			GenAdj.DiagonalDirectionsAround[1] = new IntVec3(-1, 0, 1);
			GenAdj.DiagonalDirectionsAround[2] = new IntVec3(1, 0, 1);
			GenAdj.DiagonalDirectionsAround[3] = new IntVec3(1, 0, -1);
			GenAdj.AdjacentCells[0] = new IntVec3(0, 0, 1);
			GenAdj.AdjacentCells[1] = new IntVec3(1, 0, 0);
			GenAdj.AdjacentCells[2] = new IntVec3(0, 0, -1);
			GenAdj.AdjacentCells[3] = new IntVec3(-1, 0, 0);
			GenAdj.AdjacentCells[4] = new IntVec3(1, 0, -1);
			GenAdj.AdjacentCells[5] = new IntVec3(1, 0, 1);
			GenAdj.AdjacentCells[6] = new IntVec3(-1, 0, 1);
			GenAdj.AdjacentCells[7] = new IntVec3(-1, 0, -1);
			GenAdj.AdjacentCellsAndInside[0] = new IntVec3(0, 0, 1);
			GenAdj.AdjacentCellsAndInside[1] = new IntVec3(1, 0, 0);
			GenAdj.AdjacentCellsAndInside[2] = new IntVec3(0, 0, -1);
			GenAdj.AdjacentCellsAndInside[3] = new IntVec3(-1, 0, 0);
			GenAdj.AdjacentCellsAndInside[4] = new IntVec3(1, 0, -1);
			GenAdj.AdjacentCellsAndInside[5] = new IntVec3(1, 0, 1);
			GenAdj.AdjacentCellsAndInside[6] = new IntVec3(-1, 0, 1);
			GenAdj.AdjacentCellsAndInside[7] = new IntVec3(-1, 0, -1);
			GenAdj.AdjacentCellsAndInside[8] = new IntVec3(0, 0, 0);
			GenAdj.AdjacentCellsAround[0] = new IntVec3(0, 0, 1);
			GenAdj.AdjacentCellsAround[1] = new IntVec3(1, 0, 1);
			GenAdj.AdjacentCellsAround[2] = new IntVec3(1, 0, 0);
			GenAdj.AdjacentCellsAround[3] = new IntVec3(1, 0, -1);
			GenAdj.AdjacentCellsAround[4] = new IntVec3(0, 0, -1);
			GenAdj.AdjacentCellsAround[5] = new IntVec3(-1, 0, -1);
			GenAdj.AdjacentCellsAround[6] = new IntVec3(-1, 0, 0);
			GenAdj.AdjacentCellsAround[7] = new IntVec3(-1, 0, 1);
			GenAdj.AdjacentCellsAroundBottom[0] = new IntVec3(0, 0, -1);
			GenAdj.AdjacentCellsAroundBottom[1] = new IntVec3(-1, 0, -1);
			GenAdj.AdjacentCellsAroundBottom[2] = new IntVec3(-1, 0, 0);
			GenAdj.AdjacentCellsAroundBottom[3] = new IntVec3(-1, 0, 1);
			GenAdj.AdjacentCellsAroundBottom[4] = new IntVec3(0, 0, 1);
			GenAdj.AdjacentCellsAroundBottom[5] = new IntVec3(1, 0, 1);
			GenAdj.AdjacentCellsAroundBottom[6] = new IntVec3(1, 0, 0);
			GenAdj.AdjacentCellsAroundBottom[7] = new IntVec3(1, 0, -1);
			GenAdj.AdjacentCellsAroundBottom[8] = new IntVec3(0, 0, 0);
		}

		// Token: 0x06000189 RID: 393 RVA: 0x000073E4 File Offset: 0x000055E4
		public static List<IntVec3> AdjacentCells8WayRandomized()
		{
			if (GenAdj.adjRandomOrderList == null)
			{
				GenAdj.adjRandomOrderList = new List<IntVec3>();
				for (int i = 0; i < 8; i++)
				{
					GenAdj.adjRandomOrderList.Add(GenAdj.AdjacentCells[i]);
				}
			}
			GenAdj.adjRandomOrderList.Shuffle<IntVec3>();
			return GenAdj.adjRandomOrderList;
		}

		// Token: 0x0600018A RID: 394 RVA: 0x00007432 File Offset: 0x00005632
		public static IEnumerable<IntVec3> CellsOccupiedBy(Thing t)
		{
			if (t.def.size.x == 1 && t.def.size.z == 1)
			{
				yield return t.Position;
			}
			else
			{
				foreach (IntVec3 intVec in GenAdj.CellsOccupiedBy(t.Position, t.Rotation, t.def.size))
				{
					yield return intVec;
				}
				IEnumerator<IntVec3> enumerator = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x0600018B RID: 395 RVA: 0x00007442 File Offset: 0x00005642
		public static IEnumerable<IntVec3> CellsOccupiedBy(IntVec3 center, Rot4 rotation, IntVec2 size)
		{
			GenAdj.AdjustForRotation(ref center, ref size, rotation);
			int num = center.x - (size.x - 1) / 2;
			int minZ = center.z - (size.z - 1) / 2;
			int maxX = num + size.x - 1;
			int maxZ = minZ + size.z - 1;
			int num2;
			for (int i = num; i <= maxX; i = num2 + 1)
			{
				for (int j = minZ; j <= maxZ; j = num2 + 1)
				{
					yield return new IntVec3(i, 0, j);
					num2 = j;
				}
				num2 = i;
			}
			yield break;
		}

		// Token: 0x0600018C RID: 396 RVA: 0x00007460 File Offset: 0x00005660
		public static IEnumerable<IntVec3> CellsAdjacent8Way(TargetInfo pack)
		{
			if (pack.HasThing)
			{
				foreach (IntVec3 intVec in GenAdj.CellsAdjacent8Way(pack.Thing))
				{
					yield return intVec;
				}
				IEnumerator<IntVec3> enumerator = null;
			}
			else
			{
				int num;
				for (int i = 0; i < 8; i = num + 1)
				{
					yield return pack.Cell + GenAdj.AdjacentCells[i];
					num = i;
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x0600018D RID: 397 RVA: 0x00007470 File Offset: 0x00005670
		public static IEnumerable<IntVec3> CellsAdjacent8Way(Thing t)
		{
			return GenAdj.CellsAdjacent8Way(t.Position, t.Rotation, t.def.size);
		}

		// Token: 0x0600018E RID: 398 RVA: 0x0000748E File Offset: 0x0000568E
		public static IEnumerable<IntVec3> CellsAdjacent8Way(IntVec3 thingCenter, Rot4 thingRot, IntVec2 thingSize)
		{
			GenAdj.AdjustForRotation(ref thingCenter, ref thingSize, thingRot);
			int minX = thingCenter.x - (thingSize.x - 1) / 2 - 1;
			int maxX = minX + thingSize.x + 1;
			int minZ = thingCenter.z - (thingSize.z - 1) / 2 - 1;
			int maxZ = minZ + thingSize.z + 1;
			IntVec3 cur = new IntVec3(minX - 1, 0, minZ);
			do
			{
				cur.x++;
				yield return cur;
			}
			while (cur.x < maxX);
			do
			{
				cur.z++;
				yield return cur;
			}
			while (cur.z < maxZ);
			do
			{
				cur.x--;
				yield return cur;
			}
			while (cur.x > minX);
			do
			{
				cur.z--;
				yield return cur;
			}
			while (cur.z > minZ + 1);
			yield break;
		}

		// Token: 0x0600018F RID: 399 RVA: 0x000074AC File Offset: 0x000056AC
		public static IEnumerable<IntVec3> CellsAdjacentCardinal(Thing t)
		{
			return GenAdj.CellsAdjacentCardinal(t.Position, t.Rotation, t.def.size);
		}

		// Token: 0x06000190 RID: 400 RVA: 0x000074CA File Offset: 0x000056CA
		public static IEnumerable<IntVec3> CellsAdjacentCardinal(IntVec3 center, Rot4 rot, IntVec2 size)
		{
			GenAdj.AdjustForRotation(ref center, ref size, rot);
			int minX = center.x - (size.x - 1) / 2 - 1;
			int maxX = minX + size.x + 1;
			int minZ = center.z - (size.z - 1) / 2 - 1;
			int maxZ = minZ + size.z + 1;
			IntVec3 cur = new IntVec3(minX, 0, minZ);
			do
			{
				cur.x++;
				yield return cur;
			}
			while (cur.x < maxX - 1);
			cur.x++;
			do
			{
				cur.z++;
				yield return cur;
			}
			while (cur.z < maxZ - 1);
			cur.z++;
			do
			{
				cur.x--;
				yield return cur;
			}
			while (cur.x > minX + 1);
			cur.x--;
			do
			{
				cur.z--;
				yield return cur;
			}
			while (cur.z > minZ + 1);
			yield break;
		}

		// Token: 0x06000191 RID: 401 RVA: 0x000074E8 File Offset: 0x000056E8
		public static IEnumerable<IntVec3> CellsAdjacentAlongEdge(IntVec3 thingCent, Rot4 thingRot, IntVec2 thingSize, LinkDirections dir)
		{
			GenAdj.AdjustForRotation(ref thingCent, ref thingSize, thingRot);
			int minX = thingCent.x - (thingSize.x - 1) / 2 - 1;
			int minZ = thingCent.z - (thingSize.z - 1) / 2 - 1;
			int maxX = minX + thingSize.x + 1;
			int maxZ = minZ + thingSize.z + 1;
			if (dir == LinkDirections.Down)
			{
				int num;
				for (int x = minX; x <= maxX; x = num + 1)
				{
					yield return new IntVec3(x, thingCent.y, minZ - 1);
					num = x;
				}
			}
			if (dir == LinkDirections.Up)
			{
				int num;
				for (int x = minX; x <= maxX; x = num + 1)
				{
					yield return new IntVec3(x, thingCent.y, maxZ + 1);
					num = x;
				}
			}
			if (dir == LinkDirections.Left)
			{
				int num;
				for (int x = minZ; x <= maxZ; x = num + 1)
				{
					yield return new IntVec3(minX - 1, thingCent.y, x);
					num = x;
				}
			}
			if (dir == LinkDirections.Right)
			{
				int num;
				for (int x = minZ; x <= maxZ; x = num + 1)
				{
					yield return new IntVec3(maxX + 1, thingCent.y, x);
					num = x;
				}
			}
			yield break;
		}

		// Token: 0x06000192 RID: 402 RVA: 0x0000750D File Offset: 0x0000570D
		public static IEnumerable<IntVec3> CellsAdjacent8WayAndInside(this Thing thing)
		{
			IntVec3 position = thing.Position;
			IntVec2 size = thing.def.size;
			Rot4 rotation = thing.Rotation;
			GenAdj.AdjustForRotation(ref position, ref size, rotation);
			int num = position.x - (size.x - 1) / 2 - 1;
			int minZ = position.z - (size.z - 1) / 2 - 1;
			int maxX = num + size.x + 1;
			int maxZ = minZ + size.z + 1;
			int num2;
			for (int i = num; i <= maxX; i = num2 + 1)
			{
				for (int j = minZ; j <= maxZ; j = num2 + 1)
				{
					yield return new IntVec3(i, 0, j);
					num2 = j;
				}
				num2 = i;
			}
			yield break;
		}

		// Token: 0x06000193 RID: 403 RVA: 0x0000751D File Offset: 0x0000571D
		public static void GetAdjacentCorners(LocalTargetInfo target, out IntVec3 BL, out IntVec3 TL, out IntVec3 TR, out IntVec3 BR)
		{
			if (target.HasThing)
			{
				GenAdj.GetAdjacentCorners(target.Thing.OccupiedRect(), out BL, out TL, out TR, out BR);
				return;
			}
			GenAdj.GetAdjacentCorners(CellRect.SingleCell(target.Cell), out BL, out TL, out TR, out BR);
		}

		// Token: 0x06000194 RID: 404 RVA: 0x00007558 File Offset: 0x00005758
		private static void GetAdjacentCorners(CellRect rect, out IntVec3 BL, out IntVec3 TL, out IntVec3 TR, out IntVec3 BR)
		{
			BL = new IntVec3(rect.minX - 1, 0, rect.minZ - 1);
			TL = new IntVec3(rect.minX - 1, 0, rect.maxZ + 1);
			TR = new IntVec3(rect.maxX + 1, 0, rect.maxZ + 1);
			BR = new IntVec3(rect.maxX + 1, 0, rect.minZ - 1);
		}

		// Token: 0x06000195 RID: 405 RVA: 0x000075D6 File Offset: 0x000057D6
		public static IntVec3 RandomAdjacentCell8Way(this IntVec3 root)
		{
			return root + GenAdj.AdjacentCells[Rand.RangeInclusive(0, 7)];
		}

		// Token: 0x06000196 RID: 406 RVA: 0x000075EF File Offset: 0x000057EF
		public static IntVec3 RandomAdjacentCellCardinal(this IntVec3 root)
		{
			return root + GenAdj.CardinalDirections[Rand.RangeInclusive(0, 3)];
		}

		// Token: 0x06000197 RID: 407 RVA: 0x00007608 File Offset: 0x00005808
		public static IntVec3 RandomAdjacentCell8Way(this Thing t)
		{
			CellRect cellRect = t.OccupiedRect();
			CellRect cellRect2 = cellRect.ExpandedBy(1);
			IntVec3 randomCell;
			do
			{
				randomCell = cellRect2.RandomCell;
			}
			while (cellRect.Contains(randomCell));
			return randomCell;
		}

		// Token: 0x06000198 RID: 408 RVA: 0x00007638 File Offset: 0x00005838
		public static IntVec3 RandomAdjacentCellCardinal(this Thing t)
		{
			CellRect cellRect = t.OccupiedRect();
			IntVec3 randomCell = cellRect.RandomCell;
			if (Rand.Value < 0.5f)
			{
				if (Rand.Value < 0.5f)
				{
					randomCell.x = cellRect.minX - 1;
				}
				else
				{
					randomCell.x = cellRect.maxX + 1;
				}
			}
			else if (Rand.Value < 0.5f)
			{
				randomCell.z = cellRect.minZ - 1;
			}
			else
			{
				randomCell.z = cellRect.maxZ + 1;
			}
			return randomCell;
		}

		// Token: 0x06000199 RID: 409 RVA: 0x000076BB File Offset: 0x000058BB
		public static bool TryFindRandomAdjacentCell8WayWithRoomGroup(Thing t, out IntVec3 result)
		{
			return GenAdj.TryFindRandomAdjacentCell8WayWithRoomGroup(t.Position, t.Rotation, t.def.size, t.Map, out result);
		}

		// Token: 0x0600019A RID: 410 RVA: 0x000076E0 File Offset: 0x000058E0
		public static bool TryFindRandomAdjacentCell8WayWithRoomGroup(IntVec3 center, Rot4 rot, IntVec2 size, Map map, out IntVec3 result)
		{
			GenAdj.AdjustForRotation(ref center, ref size, rot);
			GenAdj.validCells.Clear();
			foreach (IntVec3 intVec in GenAdj.CellsAdjacent8Way(center, rot, size))
			{
				if (intVec.InBounds(map) && intVec.GetRoomGroup(map) != null)
				{
					GenAdj.validCells.Add(intVec);
				}
			}
			return GenAdj.validCells.TryRandomElement(out result);
		}

		// Token: 0x0600019B RID: 411 RVA: 0x00007768 File Offset: 0x00005968
		public static bool AdjacentTo8WayOrInside(this IntVec3 me, LocalTargetInfo other)
		{
			if (other.HasThing)
			{
				return me.AdjacentTo8WayOrInside(other.Thing);
			}
			return me.AdjacentTo8WayOrInside(other.Cell);
		}

		// Token: 0x0600019C RID: 412 RVA: 0x00007790 File Offset: 0x00005990
		public static bool AdjacentTo8Way(this IntVec3 me, IntVec3 other)
		{
			int num = me.x - other.x;
			int num2 = me.z - other.z;
			if (num == 0 && num2 == 0)
			{
				return false;
			}
			if (num < 0)
			{
				num *= -1;
			}
			if (num2 < 0)
			{
				num2 *= -1;
			}
			return num <= 1 && num2 <= 1;
		}

		// Token: 0x0600019D RID: 413 RVA: 0x000077E0 File Offset: 0x000059E0
		public static bool AdjacentTo8WayOrInside(this IntVec3 me, IntVec3 other)
		{
			int num = me.x - other.x;
			int num2 = me.z - other.z;
			if (num < 0)
			{
				num *= -1;
			}
			if (num2 < 0)
			{
				num2 *= -1;
			}
			return num <= 1 && num2 <= 1;
		}

		// Token: 0x0600019E RID: 414 RVA: 0x00007828 File Offset: 0x00005A28
		public static bool IsAdjacentToCardinalOrInside(this IntVec3 me, CellRect other)
		{
			if (other.IsEmpty)
			{
				return false;
			}
			CellRect cellRect = other.ExpandedBy(1);
			return cellRect.Contains(me) && !cellRect.IsCorner(me);
		}

		// Token: 0x0600019F RID: 415 RVA: 0x00007860 File Offset: 0x00005A60
		public static bool IsAdjacentToCardinalOrInside(this Thing t1, Thing t2)
		{
			return GenAdj.IsAdjacentToCardinalOrInside(t1.OccupiedRect(), t2.OccupiedRect());
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x00007874 File Offset: 0x00005A74
		public static bool IsAdjacentToCardinalOrInside(CellRect rect1, CellRect rect2)
		{
			if (rect1.IsEmpty || rect2.IsEmpty)
			{
				return false;
			}
			CellRect cellRect = rect1.ExpandedBy(1);
			int minX = cellRect.minX;
			int maxX = cellRect.maxX;
			int minZ = cellRect.minZ;
			int maxZ = cellRect.maxZ;
			int i = minX;
			int j = minZ;
			while (i <= maxX)
			{
				if (rect2.Contains(new IntVec3(i, 0, j)) && (i != minX || j != minZ) && (i != minX || j != maxZ) && (i != maxX || j != minZ) && (i != maxX || j != maxZ))
				{
					return true;
				}
				i++;
			}
			i--;
			for (j++; j <= maxZ; j++)
			{
				if (rect2.Contains(new IntVec3(i, 0, j)) && (i != minX || j != minZ) && (i != minX || j != maxZ) && (i != maxX || j != minZ) && (i != maxX || j != maxZ))
				{
					return true;
				}
			}
			j--;
			for (i--; i >= minX; i--)
			{
				if (rect2.Contains(new IntVec3(i, 0, j)) && (i != minX || j != minZ) && (i != minX || j != maxZ) && (i != maxX || j != minZ) && (i != maxX || j != maxZ))
				{
					return true;
				}
			}
			i++;
			for (j--; j > minZ; j--)
			{
				if (rect2.Contains(new IntVec3(i, 0, j)) && (i != minX || j != minZ) && (i != minX || j != maxZ) && (i != maxX || j != minZ) && (i != maxX || j != maxZ))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x00007A0B File Offset: 0x00005C0B
		public static bool AdjacentTo8WayOrInside(this IntVec3 root, Thing t)
		{
			return root.AdjacentTo8WayOrInside(t.Position, t.Rotation, t.def.size);
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x00007A2C File Offset: 0x00005C2C
		public static bool AdjacentTo8WayOrInside(this IntVec3 root, IntVec3 center, Rot4 rot, IntVec2 size)
		{
			GenAdj.AdjustForRotation(ref center, ref size, rot);
			int num = center.x - (size.x - 1) / 2 - 1;
			int num2 = center.z - (size.z - 1) / 2 - 1;
			int num3 = num + size.x + 1;
			int num4 = num2 + size.z + 1;
			return root.x >= num && root.x <= num3 && root.z >= num2 && root.z <= num4;
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x00007AA8 File Offset: 0x00005CA8
		public static bool AdjacentTo8WayOrInside(this Thing a, Thing b)
		{
			return GenAdj.AdjacentTo8WayOrInside(a.OccupiedRect(), b.OccupiedRect());
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x00007ABC File Offset: 0x00005CBC
		public static bool AdjacentTo8WayOrInside(CellRect rect1, CellRect rect2)
		{
			return !rect1.IsEmpty && !rect2.IsEmpty && rect1.ExpandedBy(1).Overlaps(rect2);
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x00007AEE File Offset: 0x00005CEE
		public static bool IsInside(this IntVec3 root, Thing t)
		{
			return GenAdj.IsInside(root, t.Position, t.Rotation, t.def.size);
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x00007B10 File Offset: 0x00005D10
		public static bool IsInside(IntVec3 root, IntVec3 center, Rot4 rot, IntVec2 size)
		{
			GenAdj.AdjustForRotation(ref center, ref size, rot);
			int num = center.x - (size.x - 1) / 2;
			int num2 = center.z - (size.z - 1) / 2;
			int num3 = num + size.x - 1;
			int num4 = num2 + size.z - 1;
			return root.x >= num && root.x <= num3 && root.z >= num2 && root.z <= num4;
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x00007B88 File Offset: 0x00005D88
		public static CellRect OccupiedRect(this Thing t)
		{
			return GenAdj.OccupiedRect(t.Position, t.Rotation, t.def.size);
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x00007BA6 File Offset: 0x00005DA6
		public static CellRect OccupiedRect(IntVec3 center, Rot4 rot, IntVec2 size)
		{
			GenAdj.AdjustForRotation(ref center, ref size, rot);
			return new CellRect(center.x - (size.x - 1) / 2, center.z - (size.z - 1) / 2, size.x, size.z);
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x00007BE8 File Offset: 0x00005DE8
		public static void AdjustForRotation(ref IntVec3 center, ref IntVec2 size, Rot4 rot)
		{
			if (size.x == 1 && size.z == 1)
			{
				return;
			}
			if (rot.IsHorizontal)
			{
				int x = size.x;
				size.x = size.z;
				size.z = x;
			}
			switch (rot.AsInt)
			{
			case 0:
				break;
			case 1:
				if (size.z % 2 == 0)
				{
					center.z--;
					return;
				}
				break;
			case 2:
				if (size.x % 2 == 0)
				{
					center.x--;
				}
				if (size.z % 2 == 0)
				{
					center.z--;
					return;
				}
				break;
			case 3:
				if (size.x % 2 == 0)
				{
					center.x--;
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x0400003E RID: 62
		public static IntVec3[] CardinalDirections = new IntVec3[4];

		// Token: 0x0400003F RID: 63
		public static IntVec3[] CardinalDirectionsAndInside = new IntVec3[5];

		// Token: 0x04000040 RID: 64
		public static IntVec3[] CardinalDirectionsAround = new IntVec3[4];

		// Token: 0x04000041 RID: 65
		public static IntVec3[] DiagonalDirections = new IntVec3[4];

		// Token: 0x04000042 RID: 66
		public static IntVec3[] DiagonalDirectionsAround = new IntVec3[4];

		// Token: 0x04000043 RID: 67
		public static IntVec3[] AdjacentCells = new IntVec3[8];

		// Token: 0x04000044 RID: 68
		public static IntVec3[] AdjacentCellsAndInside = new IntVec3[9];

		// Token: 0x04000045 RID: 69
		public static IntVec3[] AdjacentCellsAround = new IntVec3[8];

		// Token: 0x04000046 RID: 70
		public static IntVec3[] AdjacentCellsAroundBottom = new IntVec3[9];

		// Token: 0x04000047 RID: 71
		private static List<IntVec3> adjRandomOrderList;

		// Token: 0x04000048 RID: 72
		private static List<IntVec3> validCells = new List<IntVec3>();
	}
}
