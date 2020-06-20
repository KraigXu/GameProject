using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007F4 RID: 2036
	public class ThinkNode_ConditionalWildManNeedsToReachOutside : ThinkNode_Conditional
	{
		// Token: 0x060033DF RID: 13279 RVA: 0x0011E072 File Offset: 0x0011C272
		protected override bool Satisfied(Pawn pawn)
		{
			return WildManUtility.WildManShouldReachOutsideNow(pawn);
		}
	}
}
