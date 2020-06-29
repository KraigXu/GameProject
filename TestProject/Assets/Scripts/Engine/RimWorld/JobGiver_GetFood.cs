using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	
	public class JobGiver_GetFood : ThinkNode_JobGiver
	{
		
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_GetFood jobGiver_GetFood = (JobGiver_GetFood)base.DeepCopy(resolve);
			jobGiver_GetFood.minCategory = this.minCategory;
			jobGiver_GetFood.maxLevelPercentage = this.maxLevelPercentage;
			jobGiver_GetFood.forceScanWholeMap = this.forceScanWholeMap;
			return jobGiver_GetFood;
		}

		
		public override float GetPriority(Pawn pawn)
		{
			Need_Food food = pawn.needs.food;
			if (food == null)
			{
				return 0f;
			}
			if (pawn.needs.food.CurCategory < HungerCategory.Starving && FoodUtility.ShouldBeFedBySomeone(pawn))
			{
				return 0f;
			}
			if (food.CurCategory < this.minCategory)
			{
				return 0f;
			}
			if (food.CurLevelPercentage > this.maxLevelPercentage)
			{
				return 0f;
			}
			if (food.CurLevelPercentage < pawn.RaceProps.FoodLevelPercentageWantEat)
			{
				return 9.5f;
			}
			return 0f;
		}

		
		protected override Job TryGiveJob(Pawn pawn)
		{
			Need_Food food = pawn.needs.food;
			if (food == null || food.CurCategory < this.minCategory || food.CurLevelPercentage > this.maxLevelPercentage)
			{
				return null;
			}
			bool allowCorpse;
			if (pawn.AnimalOrWildMan())
			{
				allowCorpse = true;
			}
			else
			{
				Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Malnutrition, false);
				allowCorpse = (firstHediffOfDef != null && firstHediffOfDef.Severity > 0.4f);
			}
			bool desperate = pawn.needs.food.CurCategory == HungerCategory.Starving;
			Thing thing;
			ThingDef thingDef;
			if (!FoodUtility.TryFindBestFoodSourceFor(pawn, pawn, desperate, out thing, out thingDef, true, true, false, allowCorpse, false, pawn.IsWildMan(), this.forceScanWholeMap, false, FoodPreferability.Undefined))
			{
				return null;
			}
			Pawn pawn2 = thing as Pawn;
			if (pawn2 != null)
			{
				Job job = JobMaker.MakeJob(JobDefOf.PredatorHunt, pawn2);
				job.killIncappedTarget = true;
				return job;
			}
			if (thing is Plant && thing.def.plant.harvestedThingDef == thingDef)
			{
				return JobMaker.MakeJob(JobDefOf.Harvest, thing);
			}
			Building_NutrientPasteDispenser building_NutrientPasteDispenser = thing as Building_NutrientPasteDispenser;
			if (building_NutrientPasteDispenser != null && !building_NutrientPasteDispenser.HasEnoughFeedstockInHoppers())
			{
				Building building = building_NutrientPasteDispenser.AdjacentReachableHopper(pawn);
				if (building != null)
				{
					ISlotGroupParent hopperSgp = building as ISlotGroupParent;
					Job job2 = WorkGiver_CookFillHopper.HopperFillFoodJob(pawn, hopperSgp);
					if (job2 != null)
					{
						return job2;
					}
				}
				thing = FoodUtility.BestFoodSourceOnMap(pawn, pawn, desperate, out thingDef, FoodPreferability.MealLavish, false, !pawn.IsTeetotaler(), false, false, false, false, false, false, this.forceScanWholeMap, false, FoodPreferability.Undefined);
				if (thing == null)
				{
					return null;
				}
			}
			float nutrition = FoodUtility.GetNutrition(thing, thingDef);
			Job job3 = JobMaker.MakeJob(JobDefOf.Ingest, thing);
			job3.count = FoodUtility.WillIngestStackCountOf(pawn, thingDef, nutrition);
			return job3;
		}

		
		private HungerCategory minCategory;

		
		private float maxLevelPercentage = 1f;

		
		public bool forceScanWholeMap;
	}
}
