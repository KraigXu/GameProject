using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007E5 RID: 2021
	public class ThinkNode_ConditionalHasDutyTarget : ThinkNode_Conditional
	{
		// Token: 0x060033BE RID: 13246 RVA: 0x0011DCFE File Offset: 0x0011BEFE
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.mindState.duty != null && pawn.mindState.duty.focus.IsValid;
		}
	}
}
