using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000566 RID: 1382
	public class MentalStateWorker_TantrumAll : MentalStateWorker
	{
		// Token: 0x06002730 RID: 10032 RVA: 0x000E526C File Offset: 0x000E346C
		public override bool StateCanOccur(Pawn pawn)
		{
			if (!base.StateCanOccur(pawn))
			{
				return false;
			}
			MentalStateWorker_TantrumAll.tmpThings.Clear();
			TantrumMentalStateUtility.GetSmashableThingsNear(pawn, pawn.Position, MentalStateWorker_TantrumAll.tmpThings, null, 0, 40);
			bool result = MentalStateWorker_TantrumAll.tmpThings.Count >= 2;
			MentalStateWorker_TantrumAll.tmpThings.Clear();
			return result;
		}

		// Token: 0x0400175D RID: 5981
		private static List<Thing> tmpThings = new List<Thing>();
	}
}
