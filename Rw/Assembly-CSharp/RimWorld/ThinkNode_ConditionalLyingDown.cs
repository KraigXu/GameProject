using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007E9 RID: 2025
	public class ThinkNode_ConditionalLyingDown : ThinkNode_Conditional
	{
		// Token: 0x060033C7 RID: 13255 RVA: 0x0011DE02 File Offset: 0x0011C002
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.GetPosture().Laying();
		}
	}
}
