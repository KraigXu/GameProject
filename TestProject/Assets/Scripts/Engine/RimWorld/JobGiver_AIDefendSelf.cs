using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020006AD RID: 1709
	public class JobGiver_AIDefendSelf : JobGiver_AIDefendPawn
	{
		// Token: 0x06002E36 RID: 11830 RVA: 0x0002D90A File Offset: 0x0002BB0A
		protected override Pawn GetDefendee(Pawn pawn)
		{
			return pawn;
		}

		// Token: 0x06002E37 RID: 11831 RVA: 0x00102550 File Offset: 0x00100750
		protected override float GetFlagRadius(Pawn pawn)
		{
			return pawn.mindState.duty.radius;
		}
	}
}
