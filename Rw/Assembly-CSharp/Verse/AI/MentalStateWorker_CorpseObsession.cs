using System;

namespace Verse.AI
{
	// Token: 0x0200056A RID: 1386
	public class MentalStateWorker_CorpseObsession : MentalStateWorker
	{
		// Token: 0x0600273C RID: 10044 RVA: 0x000E5426 File Offset: 0x000E3626
		public override bool StateCanOccur(Pawn pawn)
		{
			return base.StateCanOccur(pawn) && CorpseObsessionMentalStateUtility.GetClosestCorpseToDigUp(pawn) != null;
		}
	}
}
