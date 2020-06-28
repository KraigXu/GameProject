using System;

namespace Verse.AI
{
	// Token: 0x0200056C RID: 1388
	public class MentalStateWorker_Slaughterer : MentalStateWorker
	{
		// Token: 0x06002740 RID: 10048 RVA: 0x000E546B File Offset: 0x000E366B
		public override bool StateCanOccur(Pawn pawn)
		{
			return base.StateCanOccur(pawn) && SlaughtererMentalStateUtility.FindAnimal(pawn) != null;
		}
	}
}
