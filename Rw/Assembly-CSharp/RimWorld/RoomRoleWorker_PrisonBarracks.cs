﻿using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A93 RID: 2707
	public class RoomRoleWorker_PrisonBarracks : RoomRoleWorker
	{
		// Token: 0x06003FEB RID: 16363 RVA: 0x00154224 File Offset: 0x00152424
		public override float GetScore(Room room)
		{
			int num = 0;
			int num2 = 0;
			List<Thing> containedAndAdjacentThings = room.ContainedAndAdjacentThings;
			for (int i = 0; i < containedAndAdjacentThings.Count; i++)
			{
				Building_Bed building_Bed = containedAndAdjacentThings[i] as Building_Bed;
				if (building_Bed != null && building_Bed.def.building.bed_humanlike)
				{
					if (!building_Bed.ForPrisoners)
					{
						return 0f;
					}
					if (building_Bed.Medical)
					{
						num++;
					}
					else
					{
						num2++;
					}
				}
			}
			if (num2 + num <= 1)
			{
				return 0f;
			}
			return (float)num2 * 100100f + (float)num * 50001f;
		}
	}
}
