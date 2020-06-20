using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200045A RID: 1114
	public static class LargestAreaFinder
	{
		// Token: 0x06002133 RID: 8499 RVA: 0x000CB994 File Offset: 0x000C9B94
		public static CellRect FindLargestRect(Map map, Predicate<IntVec3> predicate, int breakEarlyOn = -1)
		{
			LargestAreaFinder.<>c__DisplayClass3_0 <>c__DisplayClass3_;
			<>c__DisplayClass3_.breakEarlyOn = breakEarlyOn;
			if (LargestAreaFinder.visited == null)
			{
				LargestAreaFinder.visited = new BoolGrid(map);
			}
			LargestAreaFinder.visited.ClearAndResizeTo(map);
			Rand.PushState(map.uniqueID ^ 484111219);
			<>c__DisplayClass3_.largestRect = CellRect.Empty;
			for (int i = 0; i < 3; i++)
			{
				LargestAreaFinder.tmpProcessed.Clear();
				foreach (IntVec3 c in map.cellsInRandomOrder.GetAll().InRandomOrder(LargestAreaFinder.randomOrderWorkingList))
				{
					CellRect largestRect = LargestAreaFinder.FindLargestRectAt(c, map, LargestAreaFinder.tmpProcessed, predicate);
					if (largestRect.Area > <>c__DisplayClass3_.largestRect.Area)
					{
						<>c__DisplayClass3_.largestRect = largestRect;
						if (LargestAreaFinder.<FindLargestRect>g__ShouldBreakEarly|3_0(ref <>c__DisplayClass3_))
						{
							break;
						}
					}
				}
				if (LargestAreaFinder.<FindLargestRect>g__ShouldBreakEarly|3_0(ref <>c__DisplayClass3_))
				{
					break;
				}
			}
			Rand.PopState();
			return <>c__DisplayClass3_.largestRect;
		}

		// Token: 0x06002134 RID: 8500 RVA: 0x000CBA90 File Offset: 0x000C9C90
		private static CellRect FindLargestRectAt(IntVec3 c, Map map, HashSet<IntVec3> processed, Predicate<IntVec3> predicate)
		{
			LargestAreaFinder.<>c__DisplayClass4_0 <>c__DisplayClass4_;
			<>c__DisplayClass4_.processed = processed;
			<>c__DisplayClass4_.predicate = predicate;
			if (<>c__DisplayClass4_.processed.Contains(c) || !<>c__DisplayClass4_.predicate(c))
			{
				return CellRect.Empty;
			}
			<>c__DisplayClass4_.rect = CellRect.SingleCell(c);
			bool flag;
			do
			{
				flag = false;
				if (<>c__DisplayClass4_.rect.Width <= <>c__DisplayClass4_.rect.Height)
				{
					if (<>c__DisplayClass4_.rect.maxX + 1 < map.Size.x && LargestAreaFinder.<FindLargestRectAt>g__CanExpand|4_0(Rot4.East, ref <>c__DisplayClass4_))
					{
						<>c__DisplayClass4_.rect.maxX = <>c__DisplayClass4_.rect.maxX + 1;
						flag = true;
					}
					if (<>c__DisplayClass4_.rect.minX > 0 && LargestAreaFinder.<FindLargestRectAt>g__CanExpand|4_0(Rot4.West, ref <>c__DisplayClass4_))
					{
						<>c__DisplayClass4_.rect.minX = <>c__DisplayClass4_.rect.minX - 1;
						flag = true;
					}
				}
				if (<>c__DisplayClass4_.rect.Height <= <>c__DisplayClass4_.rect.Width)
				{
					if (<>c__DisplayClass4_.rect.maxZ + 1 < map.Size.z && LargestAreaFinder.<FindLargestRectAt>g__CanExpand|4_0(Rot4.North, ref <>c__DisplayClass4_))
					{
						<>c__DisplayClass4_.rect.maxZ = <>c__DisplayClass4_.rect.maxZ + 1;
						flag = true;
					}
					if (<>c__DisplayClass4_.rect.minZ > 0 && LargestAreaFinder.<FindLargestRectAt>g__CanExpand|4_0(Rot4.South, ref <>c__DisplayClass4_))
					{
						<>c__DisplayClass4_.rect.minZ = <>c__DisplayClass4_.rect.minZ - 1;
						flag = true;
					}
				}
			}
			while (flag);
			foreach (IntVec3 item in <>c__DisplayClass4_.rect)
			{
				<>c__DisplayClass4_.processed.Add(item);
			}
			return <>c__DisplayClass4_.rect;
		}

		// Token: 0x04001434 RID: 5172
		private static BoolGrid visited;

		// Token: 0x04001435 RID: 5173
		private static List<IntVec3> randomOrderWorkingList = new List<IntVec3>();

		// Token: 0x04001436 RID: 5174
		private static HashSet<IntVec3> tmpProcessed = new HashSet<IntVec3>();
	}
}
