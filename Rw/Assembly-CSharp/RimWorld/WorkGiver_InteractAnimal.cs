using System;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200071D RID: 1821
	public abstract class WorkGiver_InteractAnimal : WorkGiver_Scanner
	{
		// Token: 0x170008A6 RID: 2214
		// (get) Token: 0x06002FF6 RID: 12278 RVA: 0x0001028D File Offset: 0x0000E48D
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.OnCell;
			}
		}

		// Token: 0x06002FF7 RID: 12279 RVA: 0x0010E178 File Offset: 0x0010C378
		public static void ResetStaticData()
		{
			WorkGiver_InteractAnimal.NoUsableFoodTrans = "NoUsableFood".Translate();
			WorkGiver_InteractAnimal.AnimalInteractedTooRecentlyTrans = "AnimalInteractedTooRecently".Translate();
			WorkGiver_InteractAnimal.CantInteractAnimalDownedTrans = "CantInteractAnimalDowned".Translate();
			WorkGiver_InteractAnimal.CantInteractAnimalAsleepTrans = "CantInteractAnimalAsleep".Translate();
			WorkGiver_InteractAnimal.CantInteractAnimalBusyTrans = "CantInteractAnimalBusy".Translate();
		}

		// Token: 0x06002FF8 RID: 12280 RVA: 0x0010E1EC File Offset: 0x0010C3EC
		protected virtual bool CanInteractWithAnimal(Pawn pawn, Pawn animal, bool forced)
		{
			if (!pawn.CanReserve(animal, 1, -1, null, forced))
			{
				return false;
			}
			if (animal.Downed)
			{
				JobFailReason.Is(WorkGiver_InteractAnimal.CantInteractAnimalDownedTrans, null);
				return false;
			}
			if (!animal.Awake())
			{
				JobFailReason.Is(WorkGiver_InteractAnimal.CantInteractAnimalAsleepTrans, null);
				return false;
			}
			if (!animal.CanCasuallyInteractNow(false))
			{
				JobFailReason.Is(WorkGiver_InteractAnimal.CantInteractAnimalBusyTrans, null);
				return false;
			}
			int num = TrainableUtility.MinimumHandlingSkill(animal);
			if (num > pawn.skills.GetSkill(SkillDefOf.Animals).Level)
			{
				JobFailReason.Is("AnimalsSkillTooLow".Translate(num), null);
				return false;
			}
			return true;
		}

		// Token: 0x06002FF9 RID: 12281 RVA: 0x0010E28C File Offset: 0x0010C48C
		protected bool HasFoodToInteractAnimal(Pawn pawn, Pawn tamee)
		{
			ThingOwner<Thing> innerContainer = pawn.inventory.innerContainer;
			int num = 0;
			float num2 = JobDriver_InteractAnimal.RequiredNutritionPerFeed(tamee);
			float num3 = 0f;
			for (int i = 0; i < innerContainer.Count; i++)
			{
				Thing thing = innerContainer[i];
				if (tamee.WillEat(thing, pawn, true) && thing.def.ingestible.preferability <= FoodPreferability.RawTasty && !thing.def.IsDrug)
				{
					for (int j = 0; j < thing.stackCount; j++)
					{
						num3 += thing.GetStatValue(StatDefOf.Nutrition, true);
						if (num3 >= num2)
						{
							num++;
							num3 = 0f;
						}
						if (num >= 2)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06002FFA RID: 12282 RVA: 0x0010E340 File Offset: 0x0010C540
		protected Job TakeFoodForAnimalInteractJob(Pawn pawn, Pawn tamee)
		{
			FoodUtility.bestFoodSourceOnMap_minNutrition_NewTemp = new float?(JobDriver_InteractAnimal.RequiredNutritionPerFeed(tamee) * 2f * 4f);
			ThingDef foodDef;
			Thing thing = FoodUtility.BestFoodSourceOnMap(pawn, tamee, false, out foodDef, FoodPreferability.RawTasty, false, false, false, false, false, false, false, false, false, false, FoodPreferability.Undefined);
			FoodUtility.bestFoodSourceOnMap_minNutrition_NewTemp = null;
			if (thing == null)
			{
				return null;
			}
			float num = JobDriver_InteractAnimal.RequiredNutritionPerFeed(tamee) * 2f * 4f;
			float nutrition = FoodUtility.GetNutrition(thing, foodDef);
			int count = Mathf.CeilToInt(num / nutrition);
			Job job = JobMaker.MakeJob(JobDefOf.TakeInventory, thing);
			job.count = count;
			return job;
		}

		// Token: 0x04001AD9 RID: 6873
		protected static string NoUsableFoodTrans;

		// Token: 0x04001ADA RID: 6874
		protected static string AnimalInteractedTooRecentlyTrans;

		// Token: 0x04001ADB RID: 6875
		private static string CantInteractAnimalDownedTrans;

		// Token: 0x04001ADC RID: 6876
		private static string CantInteractAnimalAsleepTrans;

		// Token: 0x04001ADD RID: 6877
		private static string CantInteractAnimalBusyTrans;
	}
}
