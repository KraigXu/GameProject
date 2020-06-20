using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007E1 RID: 2017
	public class ThinkNode_ConditionalCanReachMapEdge : ThinkNode_Conditional
	{
		// Token: 0x060033B5 RID: 13237 RVA: 0x0011DC7E File Offset: 0x0011BE7E
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.CanReachMapEdge();
		}
	}
}
