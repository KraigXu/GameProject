using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000568 RID: 1384
	public class MentalStateWorker_TargetedTantrum : MentalStateWorker
	{
		// Token: 0x06002736 RID: 10038 RVA: 0x000E534C File Offset: 0x000E354C
		public override bool StateCanOccur(Pawn pawn)
		{
			if (!base.StateCanOccur(pawn))
			{
				return false;
			}
			MentalStateWorker_TargetedTantrum.tmpThings.Clear();
			TantrumMentalStateUtility.GetSmashableThingsNear(pawn, pawn.Position, MentalStateWorker_TargetedTantrum.tmpThings, null, 300, 40);
			bool result = MentalStateWorker_TargetedTantrum.tmpThings.Any<Thing>();
			MentalStateWorker_TargetedTantrum.tmpThings.Clear();
			return result;
		}

		// Token: 0x0400175F RID: 5983
		private static List<Thing> tmpThings = new List<Thing>();
	}
}
