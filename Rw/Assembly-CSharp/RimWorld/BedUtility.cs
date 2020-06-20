using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C73 RID: 3187
	public static class BedUtility
	{
		// Token: 0x06004C6C RID: 19564 RVA: 0x0019A9B5 File Offset: 0x00198BB5
		public static int GetSleepingSlotsCount(IntVec2 bedSize)
		{
			return bedSize.x;
		}

		// Token: 0x06004C6D RID: 19565 RVA: 0x0019A9C0 File Offset: 0x00198BC0
		public static IntVec3 GetSleepingSlotPos(int index, IntVec3 bedCenter, Rot4 bedRot, IntVec2 bedSize)
		{
			int sleepingSlotsCount = BedUtility.GetSleepingSlotsCount(bedSize);
			if (index < 0 || index >= sleepingSlotsCount)
			{
				Log.Error(string.Concat(new object[]
				{
					"Tried to get sleeping slot pos with index ",
					index,
					", but there are only ",
					sleepingSlotsCount,
					" sleeping slots available."
				}), false);
				return bedCenter;
			}
			CellRect cellRect = GenAdj.OccupiedRect(bedCenter, bedRot, bedSize);
			if (bedRot == Rot4.North)
			{
				return new IntVec3(cellRect.minX + index, bedCenter.y, cellRect.minZ);
			}
			if (bedRot == Rot4.East)
			{
				return new IntVec3(cellRect.minX, bedCenter.y, cellRect.maxZ - index);
			}
			if (bedRot == Rot4.South)
			{
				return new IntVec3(cellRect.minX + index, bedCenter.y, cellRect.maxZ);
			}
			return new IntVec3(cellRect.maxX, bedCenter.y, cellRect.maxZ - index);
		}
	}
}
