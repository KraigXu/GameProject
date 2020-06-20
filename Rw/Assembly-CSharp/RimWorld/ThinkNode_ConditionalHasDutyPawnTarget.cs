using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007E6 RID: 2022
	public class ThinkNode_ConditionalHasDutyPawnTarget : ThinkNode_Conditional
	{
		// Token: 0x060033C0 RID: 13248 RVA: 0x0011DD24 File Offset: 0x0011BF24
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.mindState.duty != null && pawn.mindState.duty.focus.Thing is Pawn;
		}
	}
}
