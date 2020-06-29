﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	
	public static class FoodUtility
	{
		
		public static bool WillEat(this Pawn p, Thing food, Pawn getter = null, bool careIfNotAcceptableForTitle = true)
		{
			if (!p.RaceProps.CanEverEat(food))
			{
				return false;
			}
			if (p.foodRestriction != null)
			{
				FoodRestriction currentRespectedRestriction = p.foodRestriction.GetCurrentRespectedRestriction(getter);
				if (currentRespectedRestriction != null && !currentRespectedRestriction.Allows(food) && (food.def.IsWithinCategory(ThingCategoryDefOf.Foods) || food.def.IsWithinCategory(ThingCategoryDefOf.Corpses)))
				{
					return false;
				}
			}
			return !careIfNotAcceptableForTitle || !FoodUtility.InappropriateForTitle(food.def, p, true);
		}

		
		public static bool WillEat(this Pawn p, ThingDef food, Pawn getter = null, bool careIfNotAcceptableForTitle = true)
		{
			if (!p.RaceProps.CanEverEat(food))
			{
				return false;
			}
			if (p.foodRestriction != null)
			{
				FoodRestriction currentRespectedRestriction = p.foodRestriction.GetCurrentRespectedRestriction(getter);
				if (currentRespectedRestriction != null && !currentRespectedRestriction.Allows(food) && food.IsWithinCategory(currentRespectedRestriction.filter.DisplayRootCategory.catDef))
				{
					return false;
				}
			}
			return !careIfNotAcceptableForTitle || !FoodUtility.InappropriateForTitle(food, p, true);
		}

		
		public static bool InappropriateForTitle(ThingDef food, Pawn p, bool allowIfStarving)
		{
			if ((allowIfStarving && p.needs.food.Starving) || (p.story != null && p.story.traits.HasTrait(TraitDefOf.Ascetic)) || p.IsPrisoner || (food.ingestible.joyKind != null && food.ingestible.joy > 0f))
			{
				return false;
			}
			Pawn_RoyaltyTracker royalty = p.royalty;
			RoyalTitle royalTitle = (royalty != null) ? royalty.MostSeniorTitle : null;
			return royalTitle != null && royalTitle.conceited && royalTitle.def.foodRequirement.Defined && !royalTitle.def.foodRequirement.Acceptable(food);
		}

		
		public static bool TryFindBestFoodSourceFor(Pawn getter, Pawn eater, bool desperate, out Thing foodSource, out ThingDef foodDef, bool canRefillDispenser = true, bool canUseInventory = true, bool allowForbidden = false, bool allowCorpse = true, bool allowSociallyImproper = false, bool allowHarvest = false, bool forceScanWholeMap = false, bool ignoreReservations = false, FoodPreferability minPrefOverride = FoodPreferability.Undefined)
		{
			bool flag = getter.RaceProps.ToolUser && getter.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation);
			bool allowDrug = !eater.IsTeetotaler();
			Thing thing = null;
			if (canUseInventory)
			{
				if (flag)
				{
					thing = FoodUtility.BestFoodInInventory(getter, eater, (minPrefOverride == FoodPreferability.Undefined) ? FoodPreferability.MealAwful : minPrefOverride, FoodPreferability.MealLavish, 0f, false);
				}
				if (thing != null)
				{
					if (getter.Faction != Faction.OfPlayer)
					{
						foodSource = thing;
						foodDef = FoodUtility.GetFinalIngestibleDef(foodSource, false);
						return true;
					}
					CompRottable compRottable = thing.TryGetComp<CompRottable>();
					if (compRottable != null && compRottable.Stage == RotStage.Fresh && compRottable.TicksUntilRotAtCurrentTemp < 30000)
					{
						foodSource = thing;
						foodDef = FoodUtility.GetFinalIngestibleDef(foodSource, false);
						return true;
					}
				}
			}
			ThingDef thingDef;
			Thing thing2 = FoodUtility.BestFoodSourceOnMap(getter, eater, desperate, out thingDef, FoodPreferability.MealLavish, getter == eater, allowDrug, allowCorpse, true, canRefillDispenser, allowForbidden, allowSociallyImproper, allowHarvest, forceScanWholeMap, ignoreReservations, minPrefOverride);
			if (thing == null && thing2 == null)
			{
				if (canUseInventory && flag)
				{
					thing = FoodUtility.BestFoodInInventory(getter, eater, FoodPreferability.DesperateOnly, FoodPreferability.MealLavish, 0f, allowDrug);
					if (thing != null)
					{
						foodSource = thing;
						foodDef = FoodUtility.GetFinalIngestibleDef(foodSource, false);
						return true;
					}
				}
				if (thing2 == null && getter == eater && (getter.RaceProps.predator || (getter.IsWildMan() && !getter.IsPrisoner)))
				{
					Pawn pawn = FoodUtility.BestPawnToHuntForPredator(getter, forceScanWholeMap);
					if (pawn != null)
					{
						foodSource = pawn;
						foodDef = FoodUtility.GetFinalIngestibleDef(foodSource, false);
						return true;
					}
				}
				foodSource = null;
				foodDef = null;
				return false;
			}
			if (thing == null && thing2 != null)
			{
				foodSource = thing2;
				foodDef = thingDef;
				return true;
			}
			ThingDef finalIngestibleDef = FoodUtility.GetFinalIngestibleDef(thing, false);
			if (thing2 == null)
			{
				foodSource = thing;
				foodDef = finalIngestibleDef;
				return true;
			}
			float num = FoodUtility.FoodOptimality(eater, thing2, thingDef, (float)(getter.Position - thing2.Position).LengthManhattan, false);
			float num2 = FoodUtility.FoodOptimality(eater, thing, finalIngestibleDef, 0f, false);
			num2 -= 32f;
			if (num > num2)
			{
				foodSource = thing2;
				foodDef = thingDef;
				return true;
			}
			foodSource = thing;
			foodDef = FoodUtility.GetFinalIngestibleDef(foodSource, false);
			return true;
		}

		
		public static ThingDef GetFinalIngestibleDef(Thing foodSource, bool harvest = false)
		{
			Building_NutrientPasteDispenser building_NutrientPasteDispenser = foodSource as Building_NutrientPasteDispenser;
			if (building_NutrientPasteDispenser != null)
			{
				return building_NutrientPasteDispenser.DispensableDef;
			}
			Pawn pawn = foodSource as Pawn;
			if (pawn != null)
			{
				return pawn.RaceProps.corpseDef;
			}
			if (harvest)
			{
				Plant plant = foodSource as Plant;
				if (plant != null && plant.HarvestableNow && plant.def.plant.harvestedThingDef.IsIngestible)
				{
					return plant.def.plant.harvestedThingDef;
				}
			}
			return foodSource.def;
		}

		
		public static Thing BestFoodInInventory(Pawn holder, Pawn eater = null, FoodPreferability minFoodPref = FoodPreferability.NeverForNutrition, FoodPreferability maxFoodPref = FoodPreferability.MealLavish, float minStackNutrition = 0f, bool allowDrug = false)
		{
			if (holder.inventory == null)
			{
				return null;
			}
			if (eater == null)
			{
				eater = holder;
			}
			ThingOwner<Thing> innerContainer = holder.inventory.innerContainer;
			for (int i = 0; i < innerContainer.Count; i++)
			{
				Thing thing = innerContainer[i];
				if (thing.def.IsNutritionGivingIngestible && thing.IngestibleNow && eater.WillEat(thing, holder, true) && thing.def.ingestible.preferability >= minFoodPref && thing.def.ingestible.preferability <= maxFoodPref && (allowDrug || !thing.def.IsDrug) && thing.GetStatValue(StatDefOf.Nutrition, true) * (float)thing.stackCount >= minStackNutrition)
				{
					return thing;
				}
			}
			return null;
		}

		
		public static int GetMaxAmountToPickup(Thing food, Pawn pawn, int wantedCount)
		{
			if (food is Building_NutrientPasteDispenser)
			{
				if (!pawn.CanReserve(food, 1, -1, null, false))
				{
					return 0;
				}
				return -1;
			}
			else if (food is Corpse)
			{
				if (!pawn.CanReserve(food, 1, -1, null, false))
				{
					return 0;
				}
				return 1;
			}
			else
			{
				int num = Math.Min(wantedCount, food.stackCount);
				if (food.Spawned && food.Map != null)
				{
					return Math.Min(num, food.Map.reservationManager.CanReserveStack(pawn, food, 10, null, false));
				}
				return num;
			}
		}

		
		public static Thing BestFoodSourceOnMap(Pawn getter, Pawn eater, bool desperate, out ThingDef foodDef, FoodPreferability maxPref = FoodPreferability.MealLavish, bool allowPlant = true, bool allowDrug = true, bool allowCorpse = true, bool allowDispenserFull = true, bool allowDispenserEmpty = true, bool allowForbidden = false, bool allowSociallyImproper = false, bool allowHarvest = false, bool forceScanWholeMap = false, bool ignoreReservations = false, FoodPreferability minPrefOverride = FoodPreferability.Undefined)
		{
			foodDef = null;
			bool getterCanManipulate = getter.RaceProps.ToolUser && getter.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation);
			if (!getterCanManipulate && getter != eater)
			{
				Log.Error(string.Concat(new object[]
				{
					getter,
					" tried to find food to bring to ",
					eater,
					" but ",
					getter,
					" is incapable of Manipulation."
				}), false);
				return null;
			}
			FoodPreferability minPref;
			if (minPrefOverride == FoodPreferability.Undefined)
			{
				if (eater.NonHumanlikeOrWildMan())
				{
					minPref = FoodPreferability.NeverForNutrition;
				}
				else if (desperate)
				{
					minPref = FoodPreferability.DesperateOnly;
				}
				else
				{
					minPref = ((eater.needs.food.CurCategory >= HungerCategory.UrgentlyHungry) ? FoodPreferability.RawBad : FoodPreferability.MealAwful);
				}
			}
			else
			{
				minPref = minPrefOverride;
			}
			Predicate<Thing> foodValidator = delegate(Thing t)
			{
				Building_NutrientPasteDispenser building_NutrientPasteDispenser = t as Building_NutrientPasteDispenser;
				if (building_NutrientPasteDispenser != null)
				{
					if (!allowDispenserFull || !getterCanManipulate || ThingDefOf.MealNutrientPaste.ingestible.preferability < minPref || ThingDefOf.MealNutrientPaste.ingestible.preferability > maxPref || !eater.WillEat(ThingDefOf.MealNutrientPaste, getter, true) || (t.Faction != getter.Faction && t.Faction != getter.HostFaction) || (!allowForbidden && t.IsForbidden(getter)) || (!building_NutrientPasteDispenser.powerComp.PowerOn || (!allowDispenserEmpty && !building_NutrientPasteDispenser.HasEnoughFeedstockInHoppers())) || !t.InteractionCell.Standable(t.Map) || !FoodUtility.IsFoodSourceOnMapSociallyProper(t, getter, eater, allowSociallyImproper) || !getter.Map.reachability.CanReachNonLocal(getter.Position, new TargetInfo(t.InteractionCell, t.Map, false), PathEndMode.OnCell, TraverseParms.For(getter, Danger.Some, TraverseMode.ByPawn, false)))
					{
						return false;
					}
				}
				else
				{
					int stackCount = 1;
					if (FoodUtility.bestFoodSourceOnMap_minNutrition_NewTemp != null)
					{
						float statValue = t.GetStatValue(StatDefOf.Nutrition, true);
						stackCount = FoodUtility.StackCountForNutrition(FoodUtility.bestFoodSourceOnMap_minNutrition_NewTemp.Value, statValue);
					}
					if (t.def.ingestible.preferability < minPref || t.def.ingestible.preferability > maxPref || !eater.WillEat(t, getter, true) || !t.def.IsNutritionGivingIngestible || !t.IngestibleNow || (!allowCorpse && t is Corpse) || (!allowDrug && t.def.IsDrug) || (!allowForbidden && t.IsForbidden(getter)) || (!desperate && t.IsNotFresh()) || (t.IsDessicated() || !FoodUtility.IsFoodSourceOnMapSociallyProper(t, getter, eater, allowSociallyImproper) || (!getter.AnimalAwareOf(t) && !forceScanWholeMap)) || (!ignoreReservations && !getter.CanReserve(t, 10, stackCount, null, false)))
					{
						return false;
					}
				}
				return true;
			};
			ThingRequest thingRequest;
			if ((eater.RaceProps.foodType & (FoodTypeFlags.Plant | FoodTypeFlags.Tree)) > FoodTypeFlags.None && allowPlant)
			{
				thingRequest = ThingRequest.ForGroup(ThingRequestGroup.FoodSource);
			}
			else
			{
				thingRequest = ThingRequest.ForGroup(ThingRequestGroup.FoodSourceNotPlantOrTree);
			}
			Thing bestThing;
			if (getter.RaceProps.Humanlike)
			{
				bestThing = FoodUtility.SpawnedFoodSearchInnerScan(eater, getter.Position, getter.Map.listerThings.ThingsMatching(thingRequest), PathEndMode.ClosestTouch, TraverseParms.For(getter, Danger.Deadly, TraverseMode.ByPawn, false), 9999f, foodValidator);
				if (allowHarvest & getterCanManipulate)
				{
					int searchRegionsMax;
					if (forceScanWholeMap && bestThing == null)
					{
						searchRegionsMax = -1;
					}
					else
					{
						searchRegionsMax = 30;
					}
					Thing thing = GenClosest.ClosestThingReachable(getter.Position, getter.Map, ThingRequest.ForGroup(ThingRequestGroup.HarvestablePlant), PathEndMode.Touch, TraverseParms.For(getter, Danger.Deadly, TraverseMode.ByPawn, false), 9999f, delegate(Thing x)
					{
						Plant plant = (Plant)x;
						if (!plant.HarvestableNow)
						{
							return false;
						}
						ThingDef harvestedThingDef = plant.def.plant.harvestedThingDef;
						return harvestedThingDef.IsNutritionGivingIngestible && eater.WillEat(harvestedThingDef, getter, true) && getter.CanReserve(plant, 1, -1, null, false) && (allowForbidden || !plant.IsForbidden(getter)) && (bestThing == null || FoodUtility.GetFinalIngestibleDef(bestThing, false).ingestible.preferability < harvestedThingDef.ingestible.preferability);
					}, null, 0, searchRegionsMax, false, RegionType.Set_Passable, false);
					if (thing != null)
					{
						bestThing = thing;
						foodDef = FoodUtility.GetFinalIngestibleDef(thing, true);
					}
				}
				if (foodDef == null && bestThing != null)
				{
					foodDef = FoodUtility.GetFinalIngestibleDef(bestThing, false);
				}
			}
			else
			{
				int maxRegionsToScan = FoodUtility.GetMaxRegionsToScan(getter, forceScanWholeMap);
				FoodUtility.filtered.Clear();
				foreach (Thing thing2 in GenRadial.RadialDistinctThingsAround(getter.Position, getter.Map, 2f, true))
				{
					Pawn pawn = thing2 as Pawn;
					if (pawn != null && pawn != getter && pawn.RaceProps.Animal && pawn.CurJob != null && pawn.CurJob.def == JobDefOf.Ingest && pawn.CurJob.GetTarget(TargetIndex.A).HasThing)
					{
						FoodUtility.filtered.Add(pawn.CurJob.GetTarget(TargetIndex.A).Thing);
					}
				}
				bool ignoreEntirelyForbiddenRegions = !allowForbidden && ForbidUtility.CaresAboutForbidden(getter, true) && getter.playerSettings != null && getter.playerSettings.EffectiveAreaRestrictionInPawnCurrentMap != null;
				Predicate<Thing> validator = (Thing t) => foodValidator(t) && !FoodUtility.filtered.Contains(t) && (t is Building_NutrientPasteDispenser || t.def.ingestible.preferability > FoodPreferability.DesperateOnly) && !t.IsNotFresh();
				bestThing = GenClosest.ClosestThingReachable(getter.Position, getter.Map, thingRequest, PathEndMode.ClosestTouch, TraverseParms.For(getter, Danger.Deadly, TraverseMode.ByPawn, false), 9999f, validator, null, 0, maxRegionsToScan, false, RegionType.Set_Passable, ignoreEntirelyForbiddenRegions);
				FoodUtility.filtered.Clear();
				if (bestThing == null)
				{
					desperate = true;
					bestThing = GenClosest.ClosestThingReachable(getter.Position, getter.Map, thingRequest, PathEndMode.ClosestTouch, TraverseParms.For(getter, Danger.Deadly, TraverseMode.ByPawn, false), 9999f, foodValidator, null, 0, maxRegionsToScan, false, RegionType.Set_Passable, ignoreEntirelyForbiddenRegions);
				}
				if (bestThing != null)
				{
					foodDef = FoodUtility.GetFinalIngestibleDef(bestThing, false);
				}
			}
			return bestThing;
		}

		
		private static int GetMaxRegionsToScan(Pawn getter, bool forceScanWholeMap)
		{
			if (getter.RaceProps.Humanlike)
			{
				return -1;
			}
			if (forceScanWholeMap)
			{
				return -1;
			}
			if (getter.Faction == Faction.OfPlayer)
			{
				return 100;
			}
			return 30;
		}

		
		private static bool IsFoodSourceOnMapSociallyProper(Thing t, Pawn getter, Pawn eater, bool allowSociallyImproper)
		{
			if (!allowSociallyImproper)
			{
				bool animalsCare = !getter.RaceProps.Animal;
				if (!t.IsSociallyProper(getter) && !t.IsSociallyProper(eater, eater.IsPrisonerOfColony, animalsCare))
				{
					return false;
				}
			}
			return true;
		}

		
		public static float FoodOptimality(Pawn eater, Thing foodSource, ThingDef foodDef, float dist, bool takingToInventory = false)
		{
			float num = 300f;
			num -= dist;
			switch (foodDef.ingestible.preferability)
			{
			case FoodPreferability.NeverForNutrition:
				return -9999999f;
			case FoodPreferability.DesperateOnly:
				num -= 150f;
				break;
			case FoodPreferability.DesperateOnlyForHumanlikes:
				if (eater.RaceProps.Humanlike)
				{
					num -= 150f;
				}
				break;
			}
			CompRottable compRottable = foodSource.TryGetComp<CompRottable>();
			if (compRottable != null)
			{
				if (compRottable.Stage == RotStage.Dessicated)
				{
					return -9999999f;
				}
				if (!takingToInventory && compRottable.Stage == RotStage.Fresh && compRottable.TicksUntilRotAtCurrentTemp < 30000)
				{
					num += 12f;
				}
			}
			if (eater.needs != null && eater.needs.mood != null)
			{
				List<ThoughtDef> list = FoodUtility.ThoughtsFromIngesting(eater, foodSource, foodDef);
				for (int i = 0; i < list.Count; i++)
				{
					num += FoodUtility.FoodOptimalityEffectFromMoodCurve.Evaluate(list[i].stages[0].baseMoodEffect);
				}
			}
			if (foodDef.ingestible != null)
			{
				if (eater.RaceProps.Humanlike)
				{
					num += foodDef.ingestible.optimalityOffsetHumanlikes;
				}
				else if (eater.RaceProps.Animal)
				{
					num += foodDef.ingestible.optimalityOffsetFeedingAnimals;
				}
			}
			return num;
		}

		
		private static Thing SpawnedFoodSearchInnerScan(Pawn eater, IntVec3 root, List<Thing> searchSet, PathEndMode peMode, TraverseParms traverseParams, float maxDistance = 9999f, Predicate<Thing> validator = null)
		{
			if (searchSet == null)
			{
				return null;
			}
			Pawn pawn = traverseParams.pawn ?? eater;
			int num = 0;
			int num2 = 0;
			Thing result = null;
			float num3 = float.MinValue;
			for (int i = 0; i < searchSet.Count; i++)
			{
				Thing thing = searchSet[i];
				num2++;
				float num4 = (float)(root - thing.Position).LengthManhattan;
				if (num4 <= maxDistance)
				{
					float num5 = FoodUtility.FoodOptimality(eater, thing, FoodUtility.GetFinalIngestibleDef(thing, false), num4, false);
					if (num5 >= num3 && pawn.Map.reachability.CanReach(root, thing, peMode, traverseParams) && thing.Spawned && (validator == null || validator(thing)))
					{
						result = thing;
						num3 = num5;
						num++;
					}
				}
			}
			return result;
		}

		
		public static void DebugFoodSearchFromMouse_Update()
		{
			IntVec3 root = UI.MouseCell();
			Pawn pawn = Find.Selector.SingleSelectedThing as Pawn;
			if (pawn == null)
			{
				return;
			}
			if (pawn.Map != Find.CurrentMap)
			{
				return;
			}
			Thing thing = FoodUtility.SpawnedFoodSearchInnerScan(pawn, root, Find.CurrentMap.listerThings.ThingsInGroup(ThingRequestGroup.FoodSourceNotPlantOrTree), PathEndMode.ClosestTouch, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false), 9999f, null);
			if (thing != null)
			{
				GenDraw.DrawLineBetween(root.ToVector3Shifted(), thing.Position.ToVector3Shifted());
			}
		}

		
		public static void DebugFoodSearchFromMouse_OnGUI()
		{
			IntVec3 a = UI.MouseCell();
			Pawn pawn = Find.Selector.SingleSelectedThing as Pawn;
			if (pawn == null)
			{
				return;
			}
			if (pawn.Map != Find.CurrentMap)
			{
				return;
			}
			Text.Anchor = TextAnchor.MiddleCenter;
			Text.Font = GameFont.Tiny;
			foreach (Thing thing in Find.CurrentMap.listerThings.ThingsInGroup(ThingRequestGroup.FoodSourceNotPlantOrTree))
			{
				ThingDef finalIngestibleDef = FoodUtility.GetFinalIngestibleDef(thing, false);
				float num = FoodUtility.FoodOptimality(pawn, thing, finalIngestibleDef, (a - thing.Position).LengthHorizontal, false);
				Vector2 vector = thing.DrawPos.MapToUIPosition();
				Rect rect = new Rect(vector.x - 100f, vector.y - 100f, 200f, 200f);
				string text = num.ToString("F0");
				List<ThoughtDef> list = FoodUtility.ThoughtsFromIngesting(pawn, thing, finalIngestibleDef);
				for (int i = 0; i < list.Count; i++)
				{
					text = string.Concat(new string[]
					{
						text,
						"\n",
						list[i].defName,
						"(",
						FoodUtility.FoodOptimalityEffectFromMoodCurve.Evaluate(list[i].stages[0].baseMoodEffect).ToString("F0"),
						")"
					});
				}
				Widgets.Label(rect, text);
			}
			Text.Anchor = TextAnchor.UpperLeft;
		}

		
		private static Pawn BestPawnToHuntForPredator(Pawn predator, bool forceScanWholeMap)
		{
			if (predator.meleeVerbs.TryGetMeleeVerb(null) == null)
			{
				return null;
			}
			bool flag = false;
			if (predator.health.summaryHealth.SummaryHealthPercent < 0.25f)
			{
				flag = true;
			}
			FoodUtility.tmpPredatorCandidates.Clear();
			if (FoodUtility.GetMaxRegionsToScan(predator, forceScanWholeMap) < 0)
			{
				FoodUtility.tmpPredatorCandidates.AddRange(predator.Map.mapPawns.AllPawnsSpawned);
			}
			else
			{
				TraverseParms traverseParms = TraverseParms.For(predator, Danger.Deadly, TraverseMode.ByPawn, false);
				RegionTraverser.BreadthFirstTraverse(predator.Position, predator.Map, (Region from, Region to) => to.Allows(traverseParms, true), delegate(Region x)
				{
					List<Thing> list = x.ListerThings.ThingsInGroup(ThingRequestGroup.Pawn);
					for (int j = 0; j < list.Count; j++)
					{
						FoodUtility.tmpPredatorCandidates.Add((Pawn)list[j]);
					}
					return false;
				}, 999999, RegionType.Set_Passable);
			}
			Pawn pawn = null;
			float num = 0f;
			bool tutorialMode = TutorSystem.TutorialMode;
			for (int i = 0; i < FoodUtility.tmpPredatorCandidates.Count; i++)
			{
				Pawn pawn2 = FoodUtility.tmpPredatorCandidates[i];
				if (predator.GetRoom(RegionType.Set_Passable) == pawn2.GetRoom(RegionType.Set_Passable) && predator != pawn2 && (!flag || pawn2.Downed) && FoodUtility.IsAcceptablePreyFor(predator, pawn2) && predator.CanReach(pawn2, PathEndMode.ClosestTouch, Danger.Deadly, false, TraverseMode.ByPawn) && !pawn2.IsForbidden(predator) && (!tutorialMode || pawn2.Faction != Faction.OfPlayer))
				{
					float preyScoreFor = FoodUtility.GetPreyScoreFor(predator, pawn2);
					if (preyScoreFor > num || pawn == null)
					{
						num = preyScoreFor;
						pawn = pawn2;
					}
				}
			}
			FoodUtility.tmpPredatorCandidates.Clear();
			return pawn;
		}

		
		public static bool IsAcceptablePreyFor(Pawn predator, Pawn prey)
		{
			if (!prey.RaceProps.canBePredatorPrey)
			{
				return false;
			}
			if (!prey.RaceProps.IsFlesh)
			{
				return false;
			}
			if (!Find.Storyteller.difficulty.predatorsHuntHumanlikes && prey.RaceProps.Humanlike)
			{
				return false;
			}
			if (prey.BodySize > predator.RaceProps.maxPreyBodySize)
			{
				return false;
			}
			if (!prey.Downed)
			{
				if (prey.kindDef.combatPower > 2f * predator.kindDef.combatPower)
				{
					return false;
				}
				float num = prey.kindDef.combatPower * prey.health.summaryHealth.SummaryHealthPercent * prey.ageTracker.CurLifeStage.bodySizeFactor;
				float num2 = predator.kindDef.combatPower * predator.health.summaryHealth.SummaryHealthPercent * predator.ageTracker.CurLifeStage.bodySizeFactor;
				if (num >= num2)
				{
					return false;
				}
			}
			return (predator.Faction == null || prey.Faction == null || predator.HostileTo(prey)) && (predator.Faction == null || prey.HostFaction == null || predator.HostileTo(prey)) && (predator.Faction != Faction.OfPlayer || prey.Faction != Faction.OfPlayer) && (!predator.RaceProps.herdAnimal || predator.def != prey.def);
		}

		
		public static float GetPreyScoreFor(Pawn predator, Pawn prey)
		{
			float num = prey.kindDef.combatPower / predator.kindDef.combatPower;
			float num2 = prey.health.summaryHealth.SummaryHealthPercent;
			float bodySizeFactor = prey.ageTracker.CurLifeStage.bodySizeFactor;
			float lengthHorizontal = (predator.Position - prey.Position).LengthHorizontal;
			if (prey.Downed)
			{
				num2 = Mathf.Min(num2, 0.2f);
			}
			float num3 = -lengthHorizontal - 56f * num2 * num2 * num * bodySizeFactor;
			if (prey.RaceProps.Humanlike)
			{
				num3 -= 35f;
			}
			return num3;
		}

		
		public static void DebugDrawPredatorFoodSource()
		{
			Pawn pawn = Find.Selector.SingleSelectedThing as Pawn;
			if (pawn == null)
			{
				return;
			}
			Thing thing;
			ThingDef thingDef;
			if (FoodUtility.TryFindBestFoodSourceFor(pawn, pawn, true, out thing, out thingDef, false, false, false, true, false, false, false, false, FoodPreferability.Undefined))
			{
				GenDraw.DrawLineBetween(pawn.Position.ToVector3Shifted(), thing.Position.ToVector3Shifted());
				if (!(thing is Pawn))
				{
					Pawn pawn2 = FoodUtility.BestPawnToHuntForPredator(pawn, true);
					if (pawn2 != null)
					{
						GenDraw.DrawLineBetween(pawn.Position.ToVector3Shifted(), pawn2.Position.ToVector3Shifted());
					}
				}
			}
		}

		
		public static List<ThoughtDef> ThoughtsFromIngesting(Pawn ingester, Thing foodSource, ThingDef foodDef)
		{
			FoodUtility.ingestThoughts.Clear();
			if (ingester.needs == null || ingester.needs.mood == null)
			{
				return FoodUtility.ingestThoughts;
			}
			if (!ingester.story.traits.HasTrait(TraitDefOf.Ascetic) && foodDef.ingestible.tasteThought != null)
			{
				FoodUtility.ingestThoughts.Add(foodDef.ingestible.tasteThought);
			}
			CompIngredients compIngredients = foodSource.TryGetComp<CompIngredients>();
			Building_NutrientPasteDispenser building_NutrientPasteDispenser = foodSource as Building_NutrientPasteDispenser;
			if (FoodUtility.IsHumanlikeMeat(foodDef) && ingester.RaceProps.Humanlike)
			{
				FoodUtility.ingestThoughts.Add(ingester.story.traits.HasTrait(TraitDefOf.Cannibal) ? ThoughtDefOf.AteHumanlikeMeatDirectCannibal : ThoughtDefOf.AteHumanlikeMeatDirect);
			}
			else if (compIngredients != null)
			{
				for (int i = 0; i < compIngredients.ingredients.Count; i++)
				{
					FoodUtility.AddIngestThoughtsFromIngredient(compIngredients.ingredients[i], ingester, FoodUtility.ingestThoughts);
				}
			}
			else if (building_NutrientPasteDispenser != null)
			{
				Thing thing = building_NutrientPasteDispenser.FindFeedInAnyHopper();
				if (thing != null)
				{
					FoodUtility.AddIngestThoughtsFromIngredient(thing.def, ingester, FoodUtility.ingestThoughts);
				}
			}
			if (foodDef.ingestible.specialThoughtDirect != null)
			{
				FoodUtility.ingestThoughts.Add(foodDef.ingestible.specialThoughtDirect);
			}
			if (foodSource.IsNotFresh())
			{
				FoodUtility.ingestThoughts.Add(ThoughtDefOf.AteRottenFood);
			}
			if (ModsConfig.RoyaltyActive && FoodUtility.InappropriateForTitle(foodDef, ingester, false))
			{
				FoodUtility.ingestThoughts.Add(ThoughtDefOf.AteFoodInappropriateForTitle);
			}
			return FoodUtility.ingestThoughts;
		}

		
		private static void AddIngestThoughtsFromIngredient(ThingDef ingredient, Pawn ingester, List<ThoughtDef> ingestThoughts)
		{
			if (ingredient.ingestible == null)
			{
				return;
			}
			if (ingester.RaceProps.Humanlike && FoodUtility.IsHumanlikeMeat(ingredient))
			{
				ingestThoughts.Add(ingester.story.traits.HasTrait(TraitDefOf.Cannibal) ? ThoughtDefOf.AteHumanlikeMeatAsIngredientCannibal : ThoughtDefOf.AteHumanlikeMeatAsIngredient);
				return;
			}
			if (ingredient.ingestible.specialThoughtAsIngredient != null)
			{
				ingestThoughts.Add(ingredient.ingestible.specialThoughtAsIngredient);
			}
		}

		
		public static bool IsHumanlikeMeat(ThingDef def)
		{
			return def.ingestible.sourceDef != null && def.ingestible.sourceDef.race != null && def.ingestible.sourceDef.race.Humanlike;
		}

		
		public static bool IsHumanlikeMeatOrHumanlikeCorpse(Thing thing)
		{
			if (FoodUtility.IsHumanlikeMeat(thing.def))
			{
				return true;
			}
			Corpse corpse = thing as Corpse;
			return corpse != null && corpse.InnerPawn.RaceProps.Humanlike;
		}

		
		public static int WillIngestStackCountOf(Pawn ingester, ThingDef def, float singleFoodNutrition)
		{
			int num = Mathf.Min(def.ingestible.maxNumToIngestAtOnce, FoodUtility.StackCountForNutrition(ingester.needs.food.NutritionWanted, singleFoodNutrition));
			if (num < 1)
			{
				num = 1;
			}
			return num;
		}

		
		public static float GetBodyPartNutrition(Corpse corpse, BodyPartRecord part)
		{
			return FoodUtility.GetBodyPartNutrition(corpse.GetStatValue(StatDefOf.Nutrition, true), corpse.InnerPawn, part);
		}

		
		public static float GetBodyPartNutrition(float currentCorpseNutrition, Pawn pawn, BodyPartRecord part)
		{
			HediffSet hediffSet = pawn.health.hediffSet;
			float coverageOfNotMissingNaturalParts = hediffSet.GetCoverageOfNotMissingNaturalParts(pawn.RaceProps.body.corePart);
			if (coverageOfNotMissingNaturalParts <= 0f)
			{
				return 0f;
			}
			float num = hediffSet.GetCoverageOfNotMissingNaturalParts(part) / coverageOfNotMissingNaturalParts;
			return currentCorpseNutrition * num;
		}

		
		public static int StackCountForNutrition(float wantedNutrition, float singleFoodNutrition)
		{
			if (wantedNutrition <= 0.0001f)
			{
				return 0;
			}
			return Mathf.Max(Mathf.RoundToInt(wantedNutrition / singleFoodNutrition), 1);
		}

		
		public static bool ShouldBeFedBySomeone(Pawn pawn)
		{
			return FeedPatientUtility.ShouldBeFed(pawn) || WardenFeedUtility.ShouldBeFed(pawn);
		}

		
		public static void AddFoodPoisoningHediff(Pawn pawn, Thing ingestible, FoodPoisonCause cause)
		{
			Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.FoodPoisoning, false);
			if (firstHediffOfDef != null)
			{
				if (firstHediffOfDef.CurStageIndex != 2)
				{
					firstHediffOfDef.Severity = HediffDefOf.FoodPoisoning.stages[2].minSeverity - 0.001f;
				}
			}
			else
			{
				pawn.health.AddHediff(HediffMaker.MakeHediff(HediffDefOf.FoodPoisoning, pawn, null), null, null, null);
			}
			if (PawnUtility.ShouldSendNotificationAbout(pawn) && MessagesRepeatAvoider.MessageShowAllowed("MessageFoodPoisoning-" + pawn.thingIDNumber, 0.1f))
			{
				Messages.Message("MessageFoodPoisoning".Translate(pawn.LabelShort, ingestible.LabelCapNoCount, cause.ToStringHuman().CapitalizeFirst(), pawn.Named("PAWN"), ingestible.Named("FOOD")).CapitalizeFirst(), pawn, MessageTypeDefOf.NegativeEvent, true);
			}
		}

		
		public static float GetFoodPoisonChanceFactor(Pawn ingester)
		{
			float num = Find.Storyteller.difficulty.foodPoisonChanceFactor;
			if (ingester.health != null && ingester.health.hediffSet != null)
			{
				foreach (Hediff hediff in ingester.health.hediffSet.hediffs)
				{
					HediffStage curStage = hediff.CurStage;
					if (curStage != null)
					{
						num *= curStage.foodPoisoningChanceFactor;
					}
				}
			}
			return num;
		}

		
		public static bool Starving(this Pawn p)
		{
			return p.needs != null && p.needs.food != null && p.needs.food.Starving;
		}

		
		public static float GetNutrition(Thing foodSource, ThingDef foodDef)
		{
			if (foodSource == null || foodDef == null)
			{
				return 0f;
			}
			if (foodSource.def == foodDef)
			{
				return foodSource.GetStatValue(StatDefOf.Nutrition, true);
			}
			return foodDef.GetStatValueAbstract(StatDefOf.Nutrition, null);
		}

		
		public const int FoodPoisoningStageInitial = 2;

		
		public const int FoodPoisoningStageMajor = 1;

		
		public const int FoodPoisoningStageRecovering = 0;

		
		public static float? bestFoodSourceOnMap_minNutrition_NewTemp = null;

		
		private static HashSet<Thing> filtered = new HashSet<Thing>();

		
		private static readonly SimpleCurve FoodOptimalityEffectFromMoodCurve = new SimpleCurve
		{
			{
				new CurvePoint(-100f, -600f),
				true
			},
			{
				new CurvePoint(-10f, -100f),
				true
			},
			{
				new CurvePoint(-5f, -70f),
				true
			},
			{
				new CurvePoint(-1f, -50f),
				true
			},
			{
				new CurvePoint(0f, 0f),
				true
			},
			{
				new CurvePoint(100f, 800f),
				true
			}
		};

		
		private static List<Pawn> tmpPredatorCandidates = new List<Pawn>();

		
		private static List<ThoughtDef> ingestThoughts = new List<ThoughtDef>();
	}
}
