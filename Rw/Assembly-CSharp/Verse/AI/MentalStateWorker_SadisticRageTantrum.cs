using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000569 RID: 1385
	public class MentalStateWorker_SadisticRageTantrum : MentalStateWorker
	{
		// Token: 0x06002739 RID: 10041 RVA: 0x000E53A8 File Offset: 0x000E35A8
		public override bool StateCanOccur(Pawn pawn)
		{
			if (!base.StateCanOccur(pawn))
			{
				return false;
			}
			MentalStateWorker_SadisticRageTantrum.tmpThings.Clear();
			TantrumMentalStateUtility.GetSmashableThingsNear(pawn, pawn.Position, MentalStateWorker_SadisticRageTantrum.tmpThings, (Thing x) => TantrumMentalStateUtility.CanAttackPrisoner(pawn, x), 0, 40);
			bool result = MentalStateWorker_SadisticRageTantrum.tmpThings.Any<Thing>();
			MentalStateWorker_SadisticRageTantrum.tmpThings.Clear();
			return result;
		}

		// Token: 0x04001760 RID: 5984
		private static List<Thing> tmpThings = new List<Thing>();
	}
}
