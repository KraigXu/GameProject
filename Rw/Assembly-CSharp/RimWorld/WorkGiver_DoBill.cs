using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200073D RID: 1853
	public class WorkGiver_DoBill : WorkGiver_Scanner
	{
		// Token: 0x170008C9 RID: 2249
		// (get) Token: 0x06003091 RID: 12433 RVA: 0x0010FDBF File Offset: 0x0010DFBF
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.InteractionCell;
			}
		}

		// Token: 0x06003092 RID: 12434 RVA: 0x0007C4F4 File Offset: 0x0007A6F4
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Some;
		}

		// Token: 0x170008CA RID: 2250
		// (get) Token: 0x06003093 RID: 12435 RVA: 0x00110378 File Offset: 0x0010E578
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				if (this.def.fixedBillGiverDefs != null && this.def.fixedBillGiverDefs.Count == 1)
				{
					return ThingRequest.ForDef(this.def.fixedBillGiverDefs[0]);
				}
				return ThingRequest.ForGroup(ThingRequestGroup.PotentialBillGiver);
			}
		}

		// Token: 0x06003094 RID: 12436 RVA: 0x001103B8 File Offset: 0x0010E5B8
		public static void ResetStaticData()
		{
			WorkGiver_DoBill.MissingMaterialsTranslated = "MissingMaterials".Translate();
		}

		// Token: 0x06003095 RID: 12437 RVA: 0x001103D0 File Offset: 0x0010E5D0
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			List<Thing> list = pawn.Map.listerThings.ThingsInGroup(ThingRequestGroup.PotentialBillGiver);
			for (int i = 0; i < list.Count; i++)
			{
				IBillGiver billGiver;
				if ((billGiver = (list[i] as IBillGiver)) != null && this.ThingIsUsableBillGiver(list[i]) && billGiver.BillStack.AnyShouldDoNow)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06003096 RID: 12438 RVA: 0x00110430 File Offset: 0x0010E630
		public override Job JobOnThing(Pawn pawn, Thing thing, bool forced = false)
		{
			IBillGiver billGiver = thing as IBillGiver;
			if (billGiver == null || !this.ThingIsUsableBillGiver(thing) || !billGiver.BillStack.AnyShouldDoNow || !billGiver.UsableForBillsAfterFueling() || !pawn.CanReserve(thing, 1, -1, null, forced) || thing.IsBurning() || thing.IsForbidden(pawn))
			{
				return null;
			}
			CompRefuelable compRefuelable = thing.TryGetComp<CompRefuelable>();
			if (compRefuelable == null || compRefuelable.HasFuel)
			{
				billGiver.BillStack.RemoveIncompletableBills();
				return this.StartOrResumeBillJob(pawn, billGiver);
			}
			if (!RefuelWorkGiverUtility.CanRefuel(pawn, thing, forced))
			{
				return null;
			}
			return RefuelWorkGiverUtility.RefuelJob(pawn, thing, forced, null, null);
		}

		// Token: 0x06003097 RID: 12439 RVA: 0x001104C8 File Offset: 0x0010E6C8
		private static UnfinishedThing ClosestUnfinishedThingForBill(Pawn pawn, Bill_ProductionWithUft bill)
		{
			Predicate<Thing> <>9__1;
			Predicate<Thing> validator = delegate(Thing t)
			{
				if (!t.IsForbidden(pawn) && ((UnfinishedThing)t).Recipe == bill.recipe && ((UnfinishedThing)t).Creator == pawn)
				{
					List<Thing> ingredients = ((UnfinishedThing)t).ingredients;
					Predicate<Thing> match;
					if ((match = <>9__1) == null)
					{
						match = (<>9__1 = ((Thing x) => bill.IsFixedOrAllowedIngredient(x.def)));
					}
					if (ingredients.TrueForAll(match))
					{
						return pawn.CanReserve(t, 1, -1, null, false);
					}
				}
				return false;
			};
			return (UnfinishedThing)GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForDef(bill.recipe.unfinishedThingDef), PathEndMode.InteractionCell, TraverseParms.For(pawn, pawn.NormalMaxDanger(), TraverseMode.ByPawn, false), 9999f, validator, null, 0, -1, false, RegionType.Set_Passable, false);
		}

		// Token: 0x06003098 RID: 12440 RVA: 0x00110550 File Offset: 0x0010E750
		private static Job FinishUftJob(Pawn pawn, UnfinishedThing uft, Bill_ProductionWithUft bill)
		{
			if (uft.Creator != pawn)
			{
				Log.Error(string.Concat(new object[]
				{
					"Tried to get FinishUftJob for ",
					pawn,
					" finishing ",
					uft,
					" but its creator is ",
					uft.Creator
				}), false);
				return null;
			}
			Job job = WorkGiverUtility.HaulStuffOffBillGiverJob(pawn, bill.billStack.billGiver, uft);
			if (job != null && job.targetA.Thing != uft)
			{
				return job;
			}
			Job job2 = JobMaker.MakeJob(JobDefOf.DoBill, (Thing)bill.billStack.billGiver);
			job2.bill = bill;
			job2.targetQueueB = new List<LocalTargetInfo>
			{
				uft
			};
			job2.countQueue = new List<int>
			{
				1
			};
			job2.haulMode = HaulMode.ToCellNonStorage;
			return job2;
		}

		// Token: 0x06003099 RID: 12441 RVA: 0x00110620 File Offset: 0x0010E820
		private Job StartOrResumeBillJob(Pawn pawn, IBillGiver giver)
		{
			for (int i = 0; i < giver.BillStack.Count; i++)
			{
				Bill bill = giver.BillStack[i];
				if ((bill.recipe.requiredGiverWorkType == null || bill.recipe.requiredGiverWorkType == this.def.workType) && (Find.TickManager.TicksGame >= bill.lastIngredientSearchFailTicks + WorkGiver_DoBill.ReCheckFailedBillTicksRange.RandomInRange || FloatMenuMakerMap.makingFor == pawn))
				{
					bill.lastIngredientSearchFailTicks = 0;
					if (bill.ShouldDoNow() && bill.PawnAllowedToStartAnew(pawn))
					{
						SkillRequirement skillRequirement = bill.recipe.FirstSkillRequirementPawnDoesntSatisfy(pawn);
						if (skillRequirement != null)
						{
							JobFailReason.Is("UnderRequiredSkill".Translate(skillRequirement.minLevel), bill.Label);
						}
						else
						{
							Bill_ProductionWithUft bill_ProductionWithUft = bill as Bill_ProductionWithUft;
							if (bill_ProductionWithUft != null)
							{
								if (bill_ProductionWithUft.BoundUft != null)
								{
									if (bill_ProductionWithUft.BoundWorker == pawn && pawn.CanReserveAndReach(bill_ProductionWithUft.BoundUft, PathEndMode.Touch, Danger.Deadly, 1, -1, null, false) && !bill_ProductionWithUft.BoundUft.IsForbidden(pawn))
									{
										return WorkGiver_DoBill.FinishUftJob(pawn, bill_ProductionWithUft.BoundUft, bill_ProductionWithUft);
									}
									goto IL_199;
								}
								else
								{
									UnfinishedThing unfinishedThing = WorkGiver_DoBill.ClosestUnfinishedThingForBill(pawn, bill_ProductionWithUft);
									if (unfinishedThing != null)
									{
										return WorkGiver_DoBill.FinishUftJob(pawn, unfinishedThing, bill_ProductionWithUft);
									}
								}
							}
							if (WorkGiver_DoBill.TryFindBestBillIngredients(bill, pawn, (Thing)giver, this.chosenIngThings))
							{
								Job job;
								Job result = WorkGiver_DoBill.TryStartNewDoBillJob(pawn, bill, giver, this.chosenIngThings, out job, true);
								this.chosenIngThings.Clear();
								return result;
							}
							if (FloatMenuMakerMap.makingFor != pawn)
							{
								bill.lastIngredientSearchFailTicks = Find.TickManager.TicksGame;
							}
							else
							{
								JobFailReason.Is(WorkGiver_DoBill.MissingMaterialsTranslated, bill.Label);
							}
							this.chosenIngThings.Clear();
						}
					}
				}
				IL_199:;
			}
			this.chosenIngThings.Clear();
			return null;
		}

		// Token: 0x0600309A RID: 12442 RVA: 0x001107E8 File Offset: 0x0010E9E8
		public static Job TryStartNewDoBillJob(Pawn pawn, Bill bill, IBillGiver giver, List<ThingCount> chosenIngThings, out Job haulOffJob, bool dontCreateJobIfHaulOffRequired = true)
		{
			haulOffJob = WorkGiverUtility.HaulStuffOffBillGiverJob(pawn, giver, null);
			if (haulOffJob != null && dontCreateJobIfHaulOffRequired)
			{
				return haulOffJob;
			}
			Job job = JobMaker.MakeJob(JobDefOf.DoBill, (Thing)giver);
			job.targetQueueB = new List<LocalTargetInfo>(chosenIngThings.Count);
			job.countQueue = new List<int>(chosenIngThings.Count);
			for (int i = 0; i < chosenIngThings.Count; i++)
			{
				job.targetQueueB.Add(chosenIngThings[i].Thing);
				job.countQueue.Add(chosenIngThings[i].Count);
			}
			job.haulMode = HaulMode.ToCellNonStorage;
			job.bill = bill;
			return job;
		}

		// Token: 0x0600309B RID: 12443 RVA: 0x001108A0 File Offset: 0x0010EAA0
		public bool ThingIsUsableBillGiver(Thing thing)
		{
			Pawn pawn = thing as Pawn;
			Corpse corpse = thing as Corpse;
			Pawn pawn2 = null;
			if (corpse != null)
			{
				pawn2 = corpse.InnerPawn;
			}
			if (this.def.fixedBillGiverDefs != null && this.def.fixedBillGiverDefs.Contains(thing.def))
			{
				return true;
			}
			if (pawn != null)
			{
				if (this.def.billGiversAllHumanlikes && pawn.RaceProps.Humanlike)
				{
					return true;
				}
				if (this.def.billGiversAllMechanoids && pawn.RaceProps.IsMechanoid)
				{
					return true;
				}
				if (this.def.billGiversAllAnimals && pawn.RaceProps.Animal)
				{
					return true;
				}
			}
			if (corpse != null && pawn2 != null)
			{
				if (this.def.billGiversAllHumanlikesCorpses && pawn2.RaceProps.Humanlike)
				{
					return true;
				}
				if (this.def.billGiversAllMechanoidsCorpses && pawn2.RaceProps.IsMechanoid)
				{
					return true;
				}
				if (this.def.billGiversAllAnimalsCorpses && pawn2.RaceProps.Animal)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600309C RID: 12444 RVA: 0x001109A0 File Offset: 0x0010EBA0
		private static bool TryFindBestBillIngredients(Bill bill, Pawn pawn, Thing billGiver, List<ThingCount> chosen)
		{
			chosen.Clear();
			WorkGiver_DoBill.newRelevantThings.Clear();
			if (bill.recipe.ingredients.Count == 0)
			{
				return true;
			}
			IntVec3 rootCell = WorkGiver_DoBill.GetBillGiverRootCell(billGiver, pawn);
			Region rootReg = rootCell.GetRegion(pawn.Map, RegionType.Set_Passable);
			if (rootReg == null)
			{
				return false;
			}
			WorkGiver_DoBill.MakeIngredientsListInProcessingOrder(WorkGiver_DoBill.ingredientsOrdered, bill);
			WorkGiver_DoBill.relevantThings.Clear();
			WorkGiver_DoBill.processedThings.Clear();
			bool foundAll = false;
			Predicate<Thing> baseValidator = (Thing t) => t.Spawned && !t.IsForbidden(pawn) && (float)(t.Position - billGiver.Position).LengthHorizontalSquared < bill.ingredientSearchRadius * bill.ingredientSearchRadius && bill.IsFixedOrAllowedIngredient(t) && bill.recipe.ingredients.Any((IngredientCount ingNeed) => ingNeed.filter.Allows(t)) && pawn.CanReserve(t, 1, -1, null, false);
			bool billGiverIsPawn = billGiver is Pawn;
			if (billGiverIsPawn)
			{
				WorkGiver_DoBill.AddEveryMedicineToRelevantThings(pawn, billGiver, WorkGiver_DoBill.relevantThings, baseValidator, pawn.Map);
				if (WorkGiver_DoBill.TryFindBestBillIngredientsInSet(WorkGiver_DoBill.relevantThings, bill, chosen, rootCell, billGiverIsPawn))
				{
					WorkGiver_DoBill.relevantThings.Clear();
					WorkGiver_DoBill.ingredientsOrdered.Clear();
					return true;
				}
			}
			TraverseParms traverseParams = TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false);
			RegionEntryPredicate entryCondition = null;
			if (Math.Abs(999f - bill.ingredientSearchRadius) >= 1f)
			{
				float radiusSq = bill.ingredientSearchRadius * bill.ingredientSearchRadius;
				entryCondition = delegate(Region from, Region r)
				{
					if (!r.Allows(traverseParams, false))
					{
						return false;
					}
					CellRect extentsClose = r.extentsClose;
					int num = Math.Abs(billGiver.Position.x - Math.Max(extentsClose.minX, Math.Min(billGiver.Position.x, extentsClose.maxX)));
					if ((float)num > bill.ingredientSearchRadius)
					{
						return false;
					}
					int num2 = Math.Abs(billGiver.Position.z - Math.Max(extentsClose.minZ, Math.Min(billGiver.Position.z, extentsClose.maxZ)));
					return (float)num2 <= bill.ingredientSearchRadius && (float)(num * num + num2 * num2) <= radiusSq;
				};
			}
			else
			{
				entryCondition = ((Region from, Region r) => r.Allows(traverseParams, false));
			}
			int adjacentRegionsAvailable = rootReg.Neighbors.Count((Region region) => entryCondition(rootReg, region));
			int regionsProcessed = 0;
			WorkGiver_DoBill.processedThings.AddRange(WorkGiver_DoBill.relevantThings);
			RegionProcessor regionProcessor = delegate(Region r)
			{
				List<Thing> list = r.ListerThings.ThingsMatching(ThingRequest.ForGroup(ThingRequestGroup.HaulableEver));
				for (int i = 0; i < list.Count; i++)
				{
					Thing thing = list[i];
					if (!WorkGiver_DoBill.processedThings.Contains(thing) && ReachabilityWithinRegion.ThingFromRegionListerReachable(thing, r, PathEndMode.ClosestTouch, pawn) && baseValidator(thing) && !(thing.def.IsMedicine & billGiverIsPawn))
					{
						WorkGiver_DoBill.newRelevantThings.Add(thing);
						WorkGiver_DoBill.processedThings.Add(thing);
					}
				}
				int regionsProcessed;
				regionsProcessed++;
				regionsProcessed = regionsProcessed;
				if (WorkGiver_DoBill.newRelevantThings.Count > 0 && regionsProcessed > adjacentRegionsAvailable)
				{
					WorkGiver_DoBill.relevantThings.AddRange(WorkGiver_DoBill.newRelevantThings);
					WorkGiver_DoBill.newRelevantThings.Clear();
					if (WorkGiver_DoBill.TryFindBestBillIngredientsInSet(WorkGiver_DoBill.relevantThings, bill, chosen, rootCell, billGiverIsPawn))
					{
						foundAll = true;
						return true;
					}
				}
				return false;
			};
			RegionTraverser.BreadthFirstTraverse(rootReg, entryCondition, regionProcessor, 99999, RegionType.Set_Passable);
			WorkGiver_DoBill.relevantThings.Clear();
			WorkGiver_DoBill.newRelevantThings.Clear();
			WorkGiver_DoBill.processedThings.Clear();
			WorkGiver_DoBill.ingredientsOrdered.Clear();
			return foundAll;
		}

		// Token: 0x0600309D RID: 12445 RVA: 0x00110C18 File Offset: 0x0010EE18
		private static IntVec3 GetBillGiverRootCell(Thing billGiver, Pawn forPawn)
		{
			Building building = billGiver as Building;
			if (building == null)
			{
				return billGiver.Position;
			}
			if (building.def.hasInteractionCell)
			{
				return building.InteractionCell;
			}
			Log.Error("Tried to find bill ingredients for " + billGiver + " which has no interaction cell.", false);
			return forPawn.Position;
		}

		// Token: 0x0600309E RID: 12446 RVA: 0x00110C68 File Offset: 0x0010EE68
		private static void AddEveryMedicineToRelevantThings(Pawn pawn, Thing billGiver, List<Thing> relevantThings, Predicate<Thing> baseValidator, Map map)
		{
			MedicalCareCategory medicalCareCategory = WorkGiver_DoBill.GetMedicalCareCategory(billGiver);
			List<Thing> list = map.listerThings.ThingsInGroup(ThingRequestGroup.Medicine);
			WorkGiver_DoBill.tmpMedicine.Clear();
			for (int i = 0; i < list.Count; i++)
			{
				Thing thing = list[i];
				if (medicalCareCategory.AllowsMedicine(thing.def) && baseValidator(thing) && pawn.CanReach(thing, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn))
				{
					WorkGiver_DoBill.tmpMedicine.Add(thing);
				}
			}
			WorkGiver_DoBill.tmpMedicine.SortBy((Thing x) => -x.GetStatValue(StatDefOf.MedicalPotency, true), (Thing x) => x.Position.DistanceToSquared(billGiver.Position));
			relevantThings.AddRange(WorkGiver_DoBill.tmpMedicine);
			WorkGiver_DoBill.tmpMedicine.Clear();
		}

		// Token: 0x0600309F RID: 12447 RVA: 0x00110D44 File Offset: 0x0010EF44
		private static MedicalCareCategory GetMedicalCareCategory(Thing billGiver)
		{
			Pawn pawn = billGiver as Pawn;
			if (pawn != null && pawn.playerSettings != null)
			{
				return pawn.playerSettings.medCare;
			}
			return MedicalCareCategory.Best;
		}

		// Token: 0x060030A0 RID: 12448 RVA: 0x00110D70 File Offset: 0x0010EF70
		private static void MakeIngredientsListInProcessingOrder(List<IngredientCount> ingredientsOrdered, Bill bill)
		{
			ingredientsOrdered.Clear();
			if (bill.recipe.productHasIngredientStuff)
			{
				ingredientsOrdered.Add(bill.recipe.ingredients[0]);
			}
			for (int i = 0; i < bill.recipe.ingredients.Count; i++)
			{
				if (!bill.recipe.productHasIngredientStuff || i != 0)
				{
					IngredientCount ingredientCount = bill.recipe.ingredients[i];
					if (ingredientCount.IsFixedIngredient)
					{
						ingredientsOrdered.Add(ingredientCount);
					}
				}
			}
			for (int j = 0; j < bill.recipe.ingredients.Count; j++)
			{
				IngredientCount item = bill.recipe.ingredients[j];
				if (!ingredientsOrdered.Contains(item))
				{
					ingredientsOrdered.Add(item);
				}
			}
		}

		// Token: 0x060030A1 RID: 12449 RVA: 0x00110E30 File Offset: 0x0010F030
		private static bool TryFindBestBillIngredientsInSet(List<Thing> availableThings, Bill bill, List<ThingCount> chosen, IntVec3 rootCell, bool alreadySorted)
		{
			if (bill.recipe.allowMixingIngredients)
			{
				return WorkGiver_DoBill.TryFindBestBillIngredientsInSet_AllowMix(availableThings, bill, chosen);
			}
			return WorkGiver_DoBill.TryFindBestBillIngredientsInSet_NoMix(availableThings, bill, chosen, rootCell, alreadySorted);
		}

		// Token: 0x060030A2 RID: 12450 RVA: 0x00110E54 File Offset: 0x0010F054
		private static bool TryFindBestBillIngredientsInSet_NoMix(List<Thing> availableThings, Bill bill, List<ThingCount> chosen, IntVec3 rootCell, bool alreadySorted)
		{
			if (!alreadySorted)
			{
				Comparison<Thing> comparison = delegate(Thing t1, Thing t2)
				{
					float num5 = (float)(t1.Position - rootCell).LengthHorizontalSquared;
					float value = (float)(t2.Position - rootCell).LengthHorizontalSquared;
					return num5.CompareTo(value);
				};
				availableThings.Sort(comparison);
			}
			RecipeDef recipe = bill.recipe;
			chosen.Clear();
			WorkGiver_DoBill.availableCounts.Clear();
			WorkGiver_DoBill.availableCounts.GenerateFrom(availableThings);
			for (int i = 0; i < WorkGiver_DoBill.ingredientsOrdered.Count; i++)
			{
				IngredientCount ingredientCount = recipe.ingredients[i];
				bool flag = false;
				for (int j = 0; j < WorkGiver_DoBill.availableCounts.Count; j++)
				{
					float num = (float)ingredientCount.CountRequiredOfFor(WorkGiver_DoBill.availableCounts.GetDef(j), bill.recipe);
					if ((recipe.ignoreIngredientCountTakeEntireStacks || num <= WorkGiver_DoBill.availableCounts.GetCount(j)) && ingredientCount.filter.Allows(WorkGiver_DoBill.availableCounts.GetDef(j)) && (ingredientCount.IsFixedIngredient || bill.ingredientFilter.Allows(WorkGiver_DoBill.availableCounts.GetDef(j))))
					{
						for (int k = 0; k < availableThings.Count; k++)
						{
							if (availableThings[k].def == WorkGiver_DoBill.availableCounts.GetDef(j))
							{
								int num2 = availableThings[k].stackCount - ThingCountUtility.CountOf(chosen, availableThings[k]);
								if (num2 > 0)
								{
									if (recipe.ignoreIngredientCountTakeEntireStacks)
									{
										ThingCountUtility.AddToList(chosen, availableThings[k], num2);
										return true;
									}
									int num3 = Mathf.Min(Mathf.FloorToInt(num), num2);
									ThingCountUtility.AddToList(chosen, availableThings[k], num3);
									num -= (float)num3;
									if (num < 0.001f)
									{
										flag = true;
										float num4 = WorkGiver_DoBill.availableCounts.GetCount(j);
										num4 -= (float)ingredientCount.CountRequiredOfFor(WorkGiver_DoBill.availableCounts.GetDef(j), bill.recipe);
										WorkGiver_DoBill.availableCounts.SetCount(j, num4);
										break;
									}
								}
							}
						}
						if (flag)
						{
							break;
						}
					}
				}
				if (!flag)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060030A3 RID: 12451 RVA: 0x00111064 File Offset: 0x0010F264
		private static bool TryFindBestBillIngredientsInSet_AllowMix(List<Thing> availableThings, Bill bill, List<ThingCount> chosen)
		{
			chosen.Clear();
			availableThings.Sort((Thing t, Thing t2) => bill.recipe.IngredientValueGetter.ValuePerUnitOf(t2.def).CompareTo(bill.recipe.IngredientValueGetter.ValuePerUnitOf(t.def)));
			for (int i = 0; i < bill.recipe.ingredients.Count; i++)
			{
				IngredientCount ingredientCount = bill.recipe.ingredients[i];
				float num = ingredientCount.GetBaseCount();
				for (int j = 0; j < availableThings.Count; j++)
				{
					Thing thing = availableThings[j];
					if (ingredientCount.filter.Allows(thing) && (ingredientCount.IsFixedIngredient || bill.ingredientFilter.Allows(thing)))
					{
						float num2 = bill.recipe.IngredientValueGetter.ValuePerUnitOf(thing.def);
						int num3 = Mathf.Min(Mathf.CeilToInt(num / num2), thing.stackCount);
						ThingCountUtility.AddToList(chosen, thing, num3);
						num -= (float)num3 * num2;
						if (num <= 0.0001f)
						{
							break;
						}
					}
				}
				if (num > 0.0001f)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x04001AEA RID: 6890
		private List<ThingCount> chosenIngThings = new List<ThingCount>();

		// Token: 0x04001AEB RID: 6891
		private static readonly IntRange ReCheckFailedBillTicksRange = new IntRange(500, 600);

		// Token: 0x04001AEC RID: 6892
		private static string MissingMaterialsTranslated;

		// Token: 0x04001AED RID: 6893
		private static List<Thing> relevantThings = new List<Thing>();

		// Token: 0x04001AEE RID: 6894
		private static HashSet<Thing> processedThings = new HashSet<Thing>();

		// Token: 0x04001AEF RID: 6895
		private static List<Thing> newRelevantThings = new List<Thing>();

		// Token: 0x04001AF0 RID: 6896
		private static List<IngredientCount> ingredientsOrdered = new List<IngredientCount>();

		// Token: 0x04001AF1 RID: 6897
		private static List<Thing> tmpMedicine = new List<Thing>();

		// Token: 0x04001AF2 RID: 6898
		private static WorkGiver_DoBill.DefCountList availableCounts = new WorkGiver_DoBill.DefCountList();

		// Token: 0x0200189D RID: 6301
		private class DefCountList
		{
			// Token: 0x17001645 RID: 5701
			// (get) Token: 0x06008DC5 RID: 36293 RVA: 0x002CFA8C File Offset: 0x002CDC8C
			public int Count
			{
				get
				{
					return this.defs.Count;
				}
			}

			// Token: 0x17001646 RID: 5702
			public float this[ThingDef def]
			{
				get
				{
					int num = this.defs.IndexOf(def);
					if (num < 0)
					{
						return 0f;
					}
					return this.counts[num];
				}
				set
				{
					int num = this.defs.IndexOf(def);
					if (num < 0)
					{
						this.defs.Add(def);
						this.counts.Add(value);
						num = this.defs.Count - 1;
					}
					else
					{
						this.counts[num] = value;
					}
					this.CheckRemove(num);
				}
			}

			// Token: 0x06008DC8 RID: 36296 RVA: 0x002CFB26 File Offset: 0x002CDD26
			public float GetCount(int index)
			{
				return this.counts[index];
			}

			// Token: 0x06008DC9 RID: 36297 RVA: 0x002CFB34 File Offset: 0x002CDD34
			public void SetCount(int index, float val)
			{
				this.counts[index] = val;
				this.CheckRemove(index);
			}

			// Token: 0x06008DCA RID: 36298 RVA: 0x002CFB4A File Offset: 0x002CDD4A
			public ThingDef GetDef(int index)
			{
				return this.defs[index];
			}

			// Token: 0x06008DCB RID: 36299 RVA: 0x002CFB58 File Offset: 0x002CDD58
			private void CheckRemove(int index)
			{
				if (this.counts[index] == 0f)
				{
					this.counts.RemoveAt(index);
					this.defs.RemoveAt(index);
				}
			}

			// Token: 0x06008DCC RID: 36300 RVA: 0x002CFB85 File Offset: 0x002CDD85
			public void Clear()
			{
				this.defs.Clear();
				this.counts.Clear();
			}

			// Token: 0x06008DCD RID: 36301 RVA: 0x002CFBA0 File Offset: 0x002CDDA0
			public void GenerateFrom(List<Thing> things)
			{
				this.Clear();
				for (int i = 0; i < things.Count; i++)
				{
					ThingDef def = things[i].def;
					this[def] += (float)things[i].stackCount;
				}
			}

			// Token: 0x04005D73 RID: 23923
			private List<ThingDef> defs = new List<ThingDef>();

			// Token: 0x04005D74 RID: 23924
			private List<float> counts = new List<float>();
		}
	}
}
