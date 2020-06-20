using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007D2 RID: 2002
	public class ThinkNode_ConditionalNonPlayerNonHostileFactionOrFactionless : ThinkNode_Conditional
	{
		// Token: 0x06003396 RID: 13206 RVA: 0x0011DA53 File Offset: 0x0011BC53
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.Faction == null || (pawn.Faction != Faction.OfPlayer && !pawn.Faction.HostileTo(Faction.OfPlayer));
		}
	}
}
