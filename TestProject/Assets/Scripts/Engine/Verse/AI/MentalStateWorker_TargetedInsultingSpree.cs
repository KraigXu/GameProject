using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000565 RID: 1381
	public class MentalStateWorker_TargetedInsultingSpree : MentalStateWorker
	{
		// Token: 0x0600272D RID: 10029 RVA: 0x000E5230 File Offset: 0x000E3430
		public override bool StateCanOccur(Pawn pawn)
		{
			if (!base.StateCanOccur(pawn))
			{
				return false;
			}
			InsultingSpreeMentalStateUtility.GetInsultCandidatesFor(pawn, MentalStateWorker_TargetedInsultingSpree.candidates, false);
			bool result = MentalStateWorker_TargetedInsultingSpree.candidates.Any<Pawn>();
			MentalStateWorker_TargetedInsultingSpree.candidates.Clear();
			return result;
		}

		// Token: 0x0400175C RID: 5980
		private static List<Pawn> candidates = new List<Pawn>();
	}
}
