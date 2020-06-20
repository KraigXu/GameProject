using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007E4 RID: 2020
	public class ThinkNode_ConditionalExhausted : ThinkNode_Conditional
	{
		// Token: 0x060033BC RID: 13244 RVA: 0x0011DCD7 File Offset: 0x0011BED7
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.needs.rest != null && pawn.needs.rest.CurCategory >= RestCategory.Exhausted;
		}
	}
}
