using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	
	public static class CaravanDrugPolicyUtility
	{
		
		public static void CheckTakeScheduledDrugs(Caravan caravan)
		{
			if (caravan.IsHashIntervalTick(120))
			{
				CaravanDrugPolicyUtility.TryTakeScheduledDrugs(caravan);
			}
		}

		
		public static void TryTakeScheduledDrugs(Caravan caravan)
		{
			List<Pawn> pawnsListForReading = caravan.PawnsListForReading;
			for (int i = 0; i < pawnsListForReading.Count; i++)
			{
				CaravanDrugPolicyUtility.TryTakeScheduledDrugs(pawnsListForReading[i], caravan);
			}
		}

		
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

		
		private const int TryTakeScheduledDrugsIntervalTicks = 120;
	}
}
