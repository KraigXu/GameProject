using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007EE RID: 2030
	public class ThinkNode_ConditionalInNonPlayerHomeMap : ThinkNode_Conditional
	{
		// Token: 0x060033D2 RID: 13266 RVA: 0x0011DF34 File Offset: 0x0011C134
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.MapHeld != null && !pawn.MapHeld.IsPlayerHome;
		}
	}
}
