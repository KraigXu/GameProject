using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007E7 RID: 2023
	public class ThinkNode_ConditionalAtDutyLocation : ThinkNode_Conditional
	{
		// Token: 0x060033C2 RID: 13250 RVA: 0x0011DD52 File Offset: 0x0011BF52
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.mindState.duty != null && pawn.Position == pawn.mindState.duty.focus.Cell;
		}
	}
}
