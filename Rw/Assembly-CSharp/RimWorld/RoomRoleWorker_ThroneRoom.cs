using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A97 RID: 2711
	public class RoomRoleWorker_ThroneRoom : RoomRoleWorker
	{
		// Token: 0x06003FF3 RID: 16371 RVA: 0x001543DA File Offset: 0x001525DA
		public static string Validate(Room room)
		{
			if (room == null || room.OutdoorsForWork)
			{
				return "ThroneMustBePlacedInside".Translate();
			}
			return null;
		}

		// Token: 0x06003FF4 RID: 16372 RVA: 0x001543F8 File Offset: 0x001525F8
		public override float GetScore(Room room)
		{
			List<Thing> containedAndAdjacentThings = room.ContainedAndAdjacentThings;
			bool flag = false;
			for (int i = 0; i < containedAndAdjacentThings.Count; i++)
			{
				if (containedAndAdjacentThings[i] is Building_Throne)
				{
					flag = true;
					break;
				}
			}
			return (float)((flag && RoomRoleWorker_ThroneRoom.Validate(room) == null) ? 10000 : 0);
		}
	}
}
