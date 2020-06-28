using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007C8 RID: 1992
	public class ThinkNode_ConditionalGuest : ThinkNode_Conditional
	{
		// Token: 0x06003381 RID: 13185 RVA: 0x0011D92E File Offset: 0x0011BB2E
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.HostFaction != null && !pawn.IsPrisoner;
		}
	}
}
