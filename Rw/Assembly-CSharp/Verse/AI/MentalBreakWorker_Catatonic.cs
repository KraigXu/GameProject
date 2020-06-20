using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000541 RID: 1345
	public class MentalBreakWorker_Catatonic : MentalBreakWorker
	{
		// Token: 0x06002680 RID: 9856 RVA: 0x000E2E79 File Offset: 0x000E1079
		public override bool BreakCanOccur(Pawn pawn)
		{
			return pawn.IsColonist && pawn.Spawned && base.BreakCanOccur(pawn);
		}

		// Token: 0x06002681 RID: 9857 RVA: 0x000E2E94 File Offset: 0x000E1094
		public override bool TryStart(Pawn pawn, string reason, bool causedByMood)
		{
			pawn.health.AddHediff(HediffDefOf.CatatonicBreakdown, null, null, null);
			base.TrySendLetter(pawn, "LetterCatatonicMentalBreak", reason);
			return true;
		}
	}
}
