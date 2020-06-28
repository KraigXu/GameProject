using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007CD RID: 1997
	public class ThinkNode_ConditionalOfPlayerFactionOrPlayerGuest : ThinkNode_Conditional
	{
		// Token: 0x0600338B RID: 13195 RVA: 0x0011D99C File Offset: 0x0011BB9C
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.Faction == Faction.OfPlayer || (pawn.HostFaction == Faction.OfPlayer && !pawn.guest.IsPrisoner);
		}
	}
}
