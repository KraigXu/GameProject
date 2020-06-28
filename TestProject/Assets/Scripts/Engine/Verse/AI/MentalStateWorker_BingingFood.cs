using System;

namespace Verse.AI
{
	// Token: 0x02000562 RID: 1378
	public class MentalStateWorker_BingingFood : MentalStateWorker
	{
		// Token: 0x06002726 RID: 10022 RVA: 0x000E5182 File Offset: 0x000E3382
		public override bool StateCanOccur(Pawn pawn)
		{
			return base.StateCanOccur(pawn) && (!pawn.Spawned || pawn.Map.resourceCounter.TotalHumanEdibleNutrition > 10f);
		}
	}
}
