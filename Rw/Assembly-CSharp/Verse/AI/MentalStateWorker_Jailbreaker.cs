using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x0200056B RID: 1387
	public class MentalStateWorker_Jailbreaker : MentalStateWorker
	{
		// Token: 0x0600273E RID: 10046 RVA: 0x000E543C File Offset: 0x000E363C
		public override bool StateCanOccur(Pawn pawn)
		{
			return base.StateCanOccur(pawn) && pawn.health.capacities.CapableOf(PawnCapacityDefOf.Talking) && JailbreakerMentalStateUtility.FindPrisoner(pawn) != null;
		}
	}
}
