using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007D9 RID: 2009
	public class ThinkNode_ConditionalHasFaction : ThinkNode_Conditional
	{
		// Token: 0x060033A5 RID: 13221 RVA: 0x0011DBB3 File Offset: 0x0011BDB3
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.Faction != null;
		}
	}
}
