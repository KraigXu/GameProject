using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020006AC RID: 1708
	public class JobGiver_AIDefendEscortee : JobGiver_AIDefendPawn
	{
		// Token: 0x06002E33 RID: 11827 RVA: 0x00103E0E File Offset: 0x0010200E
		protected override Pawn GetDefendee(Pawn pawn)
		{
			return ((Thing)pawn.mindState.duty.focus) as Pawn;
		}

		// Token: 0x06002E34 RID: 11828 RVA: 0x00102550 File Offset: 0x00100750
		protected override float GetFlagRadius(Pawn pawn)
		{
			return pawn.mindState.duty.radius;
		}
	}
}
