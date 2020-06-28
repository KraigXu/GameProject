using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007C9 RID: 1993
	public class ThinkNode_ConditionalReleased : ThinkNode_Conditional
	{
		// Token: 0x06003383 RID: 13187 RVA: 0x0011D943 File Offset: 0x0011BB43
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.guest != null && pawn.guest.Released;
		}
	}
}
