﻿using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	
	public class FoodRestrictionDatabase : IExposable
	{
		
		
		public List<FoodRestriction> AllFoodRestrictions
		{
			get
			{
				return this.foodRestrictions;
			}
		}

		
		public FoodRestrictionDatabase()
		{
			this.GenerateStartingFoodRestrictions();
		}

		
		public void ExposeData()
		{
			Scribe_Collections.Look<FoodRestriction>(ref this.foodRestrictions, "foodRestrictions", LookMode.Deep, Array.Empty<object>());
			BackCompatibility.PostExposeData(this);
		}

		
		public FoodRestriction DefaultFoodRestriction()
		{
			if (this.foodRestrictions.Count == 0)
			{
				this.MakeNewFoodRestriction();
			}
			return this.foodRestrictions[0];
		}

		
		public AcceptanceReport TryDelete(FoodRestriction foodRestriction)
		{
			foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive)
			{
				if (pawn.foodRestriction != null && pawn.foodRestriction.CurrentFoodRestriction == foodRestriction)
				{
					return new AcceptanceReport("FoodRestrictionInUse".Translate(pawn));
				}
			}
			foreach (Pawn pawn2 in PawnsFinder.AllMapsWorldAndTemporary_AliveOrDead)
			{
				if (pawn2.foodRestriction != null && pawn2.foodRestriction.CurrentFoodRestriction == foodRestriction)
				{
					pawn2.foodRestriction.CurrentFoodRestriction = null;
				}
			}
			this.foodRestrictions.Remove(foodRestriction);
			return AcceptanceReport.WasAccepted;
		}

		
		public FoodRestriction MakeNewFoodRestriction()
		{
			int num;
			if (!this.foodRestrictions.Any<FoodRestriction>())
			{
				num = 1;
			}
			else
			{
				num = this.foodRestrictions.Max((FoodRestriction o) => o.id) + 1;
			}
			int id = num;
			FoodRestriction foodRestriction = new FoodRestriction(id, "FoodRestriction".Translate() + " " + id.ToString());
			foodRestriction.filter.SetAllow(ThingCategoryDefOf.Foods, true, null, null);
			foodRestriction.filter.SetAllow(ThingCategoryDefOf.CorpsesHumanlike, true, null, null);
			foodRestriction.filter.SetAllow(ThingCategoryDefOf.CorpsesAnimal, true, null, null);
			this.foodRestrictions.Add(foodRestriction);
			return foodRestriction;
		}

		
		private void GenerateStartingFoodRestrictions()
		{
			this.MakeNewFoodRestriction().label = "FoodRestrictionLavish".Translate();
			FoodRestriction foodRestriction = this.MakeNewFoodRestriction();
			foodRestriction.label = "FoodRestrictionFine".Translate();
			foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
			{
				if (thingDef.ingestible != null && thingDef.ingestible.preferability >= FoodPreferability.MealLavish && thingDef != ThingDefOf.InsectJelly)
				{
					foodRestriction.filter.SetAllow(thingDef, false);
				}
			}
			FoodRestriction foodRestriction2 = this.MakeNewFoodRestriction();
			foodRestriction2.label = "FoodRestrictionSimple".Translate();
			foreach (ThingDef thingDef2 in DefDatabase<ThingDef>.AllDefs)
			{
				if (thingDef2.ingestible != null && thingDef2.ingestible.preferability >= FoodPreferability.MealFine && thingDef2 != ThingDefOf.InsectJelly)
				{
					foodRestriction2.filter.SetAllow(thingDef2, false);
				}
			}
			foodRestriction2.filter.SetAllow(ThingDefOf.MealSurvivalPack, false);
			FoodRestriction foodRestriction3 = this.MakeNewFoodRestriction();
			foodRestriction3.label = "FoodRestrictionPaste".Translate();
			foreach (ThingDef thingDef3 in DefDatabase<ThingDef>.AllDefs)
			{
				if (thingDef3.ingestible != null && thingDef3.ingestible.preferability >= FoodPreferability.MealSimple && thingDef3 != ThingDefOf.MealNutrientPaste && thingDef3 != ThingDefOf.InsectJelly && thingDef3 != ThingDefOf.Pemmican)
				{
					foodRestriction3.filter.SetAllow(thingDef3, false);
				}
			}
			FoodRestriction foodRestriction4 = this.MakeNewFoodRestriction();
			foodRestriction4.label = "FoodRestrictionRaw".Translate();
			foreach (ThingDef thingDef4 in DefDatabase<ThingDef>.AllDefs)
			{
				if (thingDef4.ingestible != null && thingDef4.ingestible.preferability >= FoodPreferability.MealAwful)
				{
					foodRestriction4.filter.SetAllow(thingDef4, false);
				}
			}
			foodRestriction4.filter.SetAllow(ThingDefOf.Chocolate, false);
			FoodRestriction foodRestriction5 = this.MakeNewFoodRestriction();
			foodRestriction5.label = "FoodRestrictionNothing".Translate();
			foodRestriction5.filter.SetDisallowAll(null, null);
		}

		
		private List<FoodRestriction> foodRestrictions = new List<FoodRestriction>();
	}
}
