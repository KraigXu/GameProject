using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007DC RID: 2012
	public class ThinkNode_ConditionalForcedGoto : ThinkNode_Conditional
	{
		// Token: 0x060033AB RID: 13227 RVA: 0x0011DBF4 File Offset: 0x0011BDF4
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.mindState.forcedGotoPosition.IsValid;
		}
	}
}
