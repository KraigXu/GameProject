﻿using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A91 RID: 2705
	public class RoomRoleWorker_Laboratory : RoomRoleWorker
	{
		// Token: 0x06003FE7 RID: 16359 RVA: 0x001541E0 File Offset: 0x001523E0
		public override float GetScore(Room room)
		{
			int num = 0;
			List<Thing> containedAndAdjacentThings = room.ContainedAndAdjacentThings;
			for (int i = 0; i < containedAndAdjacentThings.Count; i++)
			{
				if (containedAndAdjacentThings[i] is Building_ResearchBench)
				{
					num++;
				}
			}
			return 30f * (float)num;
		}
	}
}
