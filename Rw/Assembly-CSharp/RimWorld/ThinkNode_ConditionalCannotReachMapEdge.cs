using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007E2 RID: 2018
	public class ThinkNode_ConditionalCannotReachMapEdge : ThinkNode_Conditional
	{
		// Token: 0x060033B7 RID: 13239 RVA: 0x0011DC86 File Offset: 0x0011BE86
		protected override bool Satisfied(Pawn pawn)
		{
			return !pawn.CanReachMapEdge();
		}
	}
}
