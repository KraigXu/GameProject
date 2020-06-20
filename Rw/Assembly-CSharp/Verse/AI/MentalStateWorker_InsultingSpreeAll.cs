using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000564 RID: 1380
	public class MentalStateWorker_InsultingSpreeAll : MentalStateWorker
	{
		// Token: 0x0600272A RID: 10026 RVA: 0x000E51F1 File Offset: 0x000E33F1
		public override bool StateCanOccur(Pawn pawn)
		{
			if (!base.StateCanOccur(pawn))
			{
				return false;
			}
			InsultingSpreeMentalStateUtility.GetInsultCandidatesFor(pawn, MentalStateWorker_InsultingSpreeAll.candidates, true);
			bool result = MentalStateWorker_InsultingSpreeAll.candidates.Count >= 2;
			MentalStateWorker_InsultingSpreeAll.candidates.Clear();
			return result;
		}

		// Token: 0x0400175B RID: 5979
		private static List<Pawn> candidates = new List<Pawn>();
	}
}
