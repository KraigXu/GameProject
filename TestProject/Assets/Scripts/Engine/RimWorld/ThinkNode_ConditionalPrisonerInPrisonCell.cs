using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007C7 RID: 1991
	public class ThinkNode_ConditionalPrisonerInPrisonCell : ThinkNode_Conditional
	{
		// Token: 0x0600337F RID: 13183 RVA: 0x0011D904 File Offset: 0x0011BB04
		protected override bool Satisfied(Pawn pawn)
		{
			if (!pawn.IsPrisoner)
			{
				return false;
			}
			Room room = pawn.GetRoom(RegionType.Set_Passable);
			return room != null && room.isPrisonCell;
		}
	}
}
