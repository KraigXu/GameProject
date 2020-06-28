using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007EB RID: 2027
	public class ThinkNode_ConditionalCanDoConstantThinkTreeJobNow : ThinkNode_Conditional
	{
		// Token: 0x060033CB RID: 13259 RVA: 0x0011DE87 File Offset: 0x0011C087
		protected override bool Satisfied(Pawn pawn)
		{
			return !pawn.Downed && !pawn.IsBurning() && !pawn.InMentalState && !pawn.Drafted && pawn.Awake();
		}
	}
}
