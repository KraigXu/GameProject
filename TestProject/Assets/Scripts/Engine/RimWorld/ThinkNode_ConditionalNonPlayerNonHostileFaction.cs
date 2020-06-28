using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007D1 RID: 2001
	public class ThinkNode_ConditionalNonPlayerNonHostileFaction : ThinkNode_Conditional
	{
		// Token: 0x06003394 RID: 13204 RVA: 0x0011DA27 File Offset: 0x0011BC27
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.Faction != null && pawn.Faction != Faction.OfPlayer && !pawn.Faction.HostileTo(Faction.OfPlayer);
		}
	}
}
