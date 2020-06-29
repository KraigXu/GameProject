﻿using System;

namespace Verse.AI
{
	
	public class ThinkNode_ChancePerHour_InsectDigChance : ThinkNode_ChancePerHour
	{
		
		protected override float MtbHours(Pawn pawn)
		{
			Room room = pawn.GetRoom(RegionType.Set_Passable);
			if (room == null)
			{
				return 18f;
			}
			int num = room.IsHuge ? 9999 : room.CellCount;
			float num2 = GenMath.LerpDoubleClamped(2f, 25f, 6f, 1f, (float)num);
			return 18f / num2;
		}

		
		private const float BaseMtbHours = 18f;
	}
}
