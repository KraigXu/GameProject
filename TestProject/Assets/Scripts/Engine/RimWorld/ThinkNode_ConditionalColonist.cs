using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007CA RID: 1994
	public class ThinkNode_ConditionalColonist : ThinkNode_Conditional
	{
		// Token: 0x06003385 RID: 13189 RVA: 0x0011D95A File Offset: 0x0011BB5A
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.IsColonist;
		}
	}
}
