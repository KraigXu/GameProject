using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007C6 RID: 1990
	public class ThinkNode_ConditionalPrisoner : ThinkNode_Conditional
	{
		// Token: 0x0600337D RID: 13181 RVA: 0x0011D8FA File Offset: 0x0011BAFA
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.IsPrisoner;
		}
	}
}
