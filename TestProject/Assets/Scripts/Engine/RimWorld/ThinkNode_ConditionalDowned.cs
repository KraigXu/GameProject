using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007C4 RID: 1988
	public class ThinkNode_ConditionalDowned : ThinkNode_Conditional
	{
		// Token: 0x06003379 RID: 13177 RVA: 0x0011D8B2 File Offset: 0x0011BAB2
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.Downed;
		}
	}
}
