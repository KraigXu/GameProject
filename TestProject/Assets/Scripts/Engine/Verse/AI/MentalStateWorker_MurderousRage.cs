using System;

namespace Verse.AI
{
	// Token: 0x0200056D RID: 1389
	public class MentalStateWorker_MurderousRage : MentalStateWorker
	{
		// Token: 0x06002742 RID: 10050 RVA: 0x000E5481 File Offset: 0x000E3681
		public override bool StateCanOccur(Pawn pawn)
		{
			return base.StateCanOccur(pawn) && MurderousRageMentalStateUtility.FindPawnToKill(pawn) != null;
		}
	}
}
