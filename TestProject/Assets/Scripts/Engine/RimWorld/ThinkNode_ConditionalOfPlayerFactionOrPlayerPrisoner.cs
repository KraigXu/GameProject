using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007CC RID: 1996
	public class ThinkNode_ConditionalOfPlayerFactionOrPlayerPrisoner : ThinkNode_Conditional
	{
		// Token: 0x06003389 RID: 13193 RVA: 0x0011D971 File Offset: 0x0011BB71
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.Faction == Faction.OfPlayer || (pawn.HostFaction == Faction.OfPlayer && pawn.guest.IsPrisoner);
		}
	}
}
