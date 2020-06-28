using System;

namespace Verse.AI
{
	// Token: 0x0200058B RID: 1419
	public class ThinkNode_ChancePerHour_InsectDigChance : ThinkNode_ChancePerHour
	{
		// Token: 0x06002858 RID: 10328 RVA: 0x000EE924 File Offset: 0x000ECB24
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

		// Token: 0x0400183D RID: 6205
		private const float BaseMtbHours = 18f;
	}
}
