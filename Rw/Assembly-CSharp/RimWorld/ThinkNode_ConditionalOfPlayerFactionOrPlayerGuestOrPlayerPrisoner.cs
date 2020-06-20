using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007CE RID: 1998
	public class ThinkNode_ConditionalOfPlayerFactionOrPlayerGuestOrPlayerPrisoner : ThinkNode_Conditional
	{
		// Token: 0x0600338D RID: 13197 RVA: 0x0011D9CA File Offset: 0x0011BBCA
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.Faction == Faction.OfPlayer || pawn.HostFaction == Faction.OfPlayer;
		}
	}
}
