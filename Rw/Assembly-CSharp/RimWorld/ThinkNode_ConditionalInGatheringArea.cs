using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007ED RID: 2029
	public class ThinkNode_ConditionalInGatheringArea : ThinkNode_Conditional
	{
		// Token: 0x060033D0 RID: 13264 RVA: 0x0011DEF0 File Offset: 0x0011C0F0
		protected override bool Satisfied(Pawn pawn)
		{
			if (pawn.mindState.duty == null)
			{
				return false;
			}
			IntVec3 cell = pawn.mindState.duty.focus.Cell;
			return GatheringsUtility.InGatheringArea(pawn.Position, cell, pawn.Map);
		}
	}
}
