using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200122F RID: 4655
	public static class CaravanDrugPolicyUtility
	{
		// Token: 0x06006C5F RID: 27743 RVA: 0x0025C63D File Offset: 0x0025A83D
		public static void CheckTakeScheduledDrugs(Caravan caravan)
		{
			if (caravan.IsHashIntervalTick(120))
			{
				CaravanDrugPolicyUtility.TryTakeScheduledDrugs(caravan);
			}
		}

		// Token: 0x06006C60 RID: 27744 RVA: 0x0025C650 File Offset: 0x0025A850
		public static void TryTakeScheduledDrugs(Caravan caravan)
		{
			List<Pawn> pawnsListForReading = caravan.PawnsListForReading;
			for (int i = 0; i < pawnsListForReading.Count; i++)
			{
				CaravanDrugPolicyUtility.TryTakeScheduledDrugs(pawnsListForReading[i], caravan);
			}
		}

		// Token: 0x06006C61 RID: 27745 RVA: 0x0025C684 File Offset: 0x0025A884
		private static void TryTakeScheduledDrugs(Pawn pawn, Caravan caravan)
		{
			if (pawn.drugs == null)
			{
				return;
			}
			DrugPolicy currentPolicy = pawn.drugs.CurrentPolicy;
			for (int i = 0; i < currentPolicy.Count; i++)
			{
				Thing drug;
				Pawn drugOwner;
				if (pawn.drugs.ShouldTryToTakeScheduledNow(currentPolicy[i].drug) && CaravanInventoryUtility.TryGetThingOfDef(caravan, currentPolicy[i].drug, out drug, out drugOwner))
				{
					caravan.needs.IngestDrug(pawn, drug, drugOwner);
				}
			}
		}

		// Token: 0x04004379 RID: 17273
		private const int TryTakeScheduledDrugsIntervalTicks = 120;
	}
}
