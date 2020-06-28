using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020006A5 RID: 1701
	public class JobGiver_BingeFood : JobGiver_Binge
	{
		// Token: 0x06002E14 RID: 11796 RVA: 0x0010348D File Offset: 0x0010168D
		protected override int IngestInterval(Pawn pawn)
		{
			return 1100;
		}

		// Token: 0x06002E15 RID: 11797 RVA: 0x00103494 File Offset: 0x00101694
		protected override Thing BestIngestTarget(Pawn pawn)
		{
			Thing result;
			ThingDef thingDef;
			if (FoodUtility.TryFindBestFoodSourceFor(pawn, pawn, true, out result, out thingDef, false, true, true, true, true, false, false, false, FoodPreferability.Undefined))
			{
				return result;
			}
			return null;
		}

		// Token: 0x04001A58 RID: 6744
		private const int BaseIngestInterval = 1100;
	}
}
