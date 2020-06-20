using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020007D3 RID: 2003
	public class ThinkNode_ConditionalHasLord : ThinkNode_Conditional
	{
		// Token: 0x06003398 RID: 13208 RVA: 0x0011DA81 File Offset: 0x0011BC81
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.GetLord() != null;
		}
	}
}
