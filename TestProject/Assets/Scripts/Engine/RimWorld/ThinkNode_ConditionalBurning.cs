using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007D7 RID: 2007
	public class ThinkNode_ConditionalBurning : ThinkNode_Conditional
	{
		// Token: 0x060033A1 RID: 13217 RVA: 0x0011DB7C File Offset: 0x0011BD7C
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.HasAttachment(ThingDefOf.Fire);
		}
	}
}
