using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007DB RID: 2011
	public class ThinkNode_ConditionalExitTimedOut : ThinkNode_Conditional
	{
		// Token: 0x060033A9 RID: 13225 RVA: 0x0011DBCB File Offset: 0x0011BDCB
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.mindState.exitMapAfterTick >= 0 && Find.TickManager.TicksGame > pawn.mindState.exitMapAfterTick;
		}
	}
}
