using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200068C RID: 1676
	public class JobDriver_Ingest : JobDriver
	{
		// Token: 0x1700088D RID: 2189
		// (get) Token: 0x06002D87 RID: 11655 RVA: 0x00100734 File Offset: 0x000FE934
		private Thing IngestibleSource
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x1700088E RID: 2190
		// (get) Token: 0x06002D88 RID: 11656 RVA: 0x00100758 File Offset: 0x000FE958
		private float ChewDurationMultiplier
		{
			get
			{
				Thing ingestibleSource = this.IngestibleSource;
				if (ingestibleSource.def.ingestible != null && !ingestibleSource.def.ingestible.useEatingSpeedStat)
				{
					return 1f;
				}
				return 1f / this.pawn.GetStatValue(StatDefOf.EatingSpeed, true);
			}
		}

		// Token: 0x06002D89 RID: 11657 RVA: 0x001007A8 File Offset: 0x000FE9A8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.usingNutrientPasteDispenser, "usingNutrientPasteDispenser", false, false);
			Scribe_Values.Look<bool>(ref this.eatingFromInventory, "eatingFromInventory", false, false);
		}

		// Token: 0x06002D8A RID: 11658 RVA: 0x001007D4 File Offset: 0x000FE9D4
		public override string GetReport()
		{
			if (this.usingNutrientPasteDispenser)
			{
				return JobUtility.GetResolvedJobReportRaw(this.job.def.reportString, ThingDefOf.MealNutrientPaste.label, ThingDefOf.MealNutrientPaste, "", "", "", "");
			}
			Thing thing = this.job.targetA.Thing;
			if (thing != null && thing.def.ingestible != null)
			{
				if (!thing.def.ingestible.ingestReportStringEat.NullOrEmpty() && (thing.def.ingestible.ingestReportString.NullOrEmpty() || this.pawn.RaceProps.intelligence < Intelligence.ToolUser))
				{
					return thing.def.ingestible.ingestReportStringEat.Formatted(this.job.targetA.Thing.LabelShort, this.job.targetA.Thing);
				}
				if (!thing.def.ingestible.ingestReportString.NullOrEmpty())
				{
					return thing.def.ingestible.ingestReportString.Formatted(this.job.targetA.Thing.LabelShort, this.job.targetA.Thing);
				}
			}
			return base.GetReport();
		}

		// Token: 0x06002D8B RID: 11659 RVA: 0x0010093C File Offset: 0x000FEB3C
		public override void Notify_Starting()
		{
			base.Notify_Starting();
			this.usingNutrientPasteDispenser = (this.IngestibleSource is Building_NutrientPasteDispenser);
			this.eatingFromInventory = (this.pawn.inventory != null && this.pawn.inventory.Contains(this.IngestibleSource));
		}

		// Token: 0x06002D8C RID: 11660 RVA: 0x00100990 File Offset: 0x000FEB90
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			if (this.pawn.Faction != null && !(this.IngestibleSource is Building_NutrientPasteDispenser))
			{
				Thing ingestibleSource = this.IngestibleSource;
				if (!this.pawn.Reserve(ingestibleSource, this.job, 10, FoodUtility.GetMaxAmountToPickup(ingestibleSource, this.pawn, this.job.count), null, errorOnFailed))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06002D8D RID: 11661 RVA: 0x001009F5 File Offset: 0x000FEBF5
		protected override IEnumerable<Toil> MakeNewToils()
		{
			if (!this.usingNutrientPasteDispenser)
			{
				this.FailOn(() => !this.IngestibleSource.Destroyed && !this.IngestibleSource.IngestibleNow);
			}
			Toil chew = Toils_Ingest.ChewIngestible(this.pawn, this.ChewDurationMultiplier, TargetIndex.A, TargetIndex.B).FailOn((Toil x) => !this.IngestibleSource.Spawned && (this.pawn.carryTracker == null || this.pawn.carryTracker.CarriedThing != this.IngestibleSource)).FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			foreach (Toil toil in this.PrepareToIngestToils(chew))
			{
				yield return toil;
			}
			IEnumerator<Toil> enumerator = null;
			yield return chew;
			yield return Toils_Ingest.FinalizeIngest(this.pawn, TargetIndex.A);
			yield return Toils_Jump.JumpIf(chew, () => this.job.GetTarget(TargetIndex.A).Thing is Corpse && this.pawn.needs.food.CurLevelPercentage < 0.9f);
			yield break;
			yield break;
		}

		// Token: 0x06002D8E RID: 11662 RVA: 0x00100A05 File Offset: 0x000FEC05
		private IEnumerable<Toil> PrepareToIngestToils(Toil chewToil)
		{
			if (this.usingNutrientPasteDispenser)
			{
				return this.PrepareToIngestToils_Dispenser();
			}
			if (this.pawn.RaceProps.ToolUser)
			{
				return this.PrepareToIngestToils_ToolUser(chewToil);
			}
			return this.PrepareToIngestToils_NonToolUser();
		}

		// Token: 0x06002D8F RID: 11663 RVA: 0x00100A36 File Offset: 0x000FEC36
		private IEnumerable<Toil> PrepareToIngestToils_Dispenser()
		{
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell).FailOnDespawnedNullOrForbidden(TargetIndex.A);
			yield return Toils_Ingest.TakeMealFromDispenser(TargetIndex.A, this.pawn);
			yield return Toils_Ingest.CarryIngestibleToChewSpot(this.pawn, TargetIndex.A).FailOnDestroyedNullOrForbidden(TargetIndex.A);
			yield return Toils_Ingest.FindAdjacentEatSurface(TargetIndex.B, TargetIndex.A);
			yield break;
		}

		// Token: 0x06002D90 RID: 11664 RVA: 0x00100A46 File Offset: 0x000FEC46
		private IEnumerable<Toil> PrepareToIngestToils_ToolUser(Toil chewToil)
		{
			if (this.eatingFromInventory)
			{
				yield return Toils_Misc.TakeItemFromInventoryToCarrier(this.pawn, TargetIndex.A);
			}
			else
			{
				yield return this.ReserveFood();
				Toil gotoToPickup = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch).FailOnDespawnedNullOrForbidden(TargetIndex.A);
				yield return Toils_Jump.JumpIf(gotoToPickup, () => this.pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation));
				yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).FailOnDespawnedNullOrForbidden(TargetIndex.A);
				yield return Toils_Jump.Jump(chewToil);
				yield return gotoToPickup;
				yield return Toils_Ingest.PickupIngestible(TargetIndex.A, this.pawn);
				JobDriver_Ingest.<>c__DisplayClass17_0 <>c__DisplayClass17_ = new JobDriver_Ingest.<>c__DisplayClass17_0();
				<>c__DisplayClass17_.<>4__this = this;
				<>c__DisplayClass17_.reserveExtraFoodToCollect = Toils_Ingest.ReserveFoodFromStackForIngesting(TargetIndex.C, null);
				Toil findExtraFoodToCollect = new Toil();
				findExtraFoodToCollect.initAction = delegate
				{
					if (<>c__DisplayClass17_.<>4__this.pawn.inventory.innerContainer.TotalStackCountOfDef(<>c__DisplayClass17_.<>4__this.IngestibleSource.def) < <>c__DisplayClass17_.<>4__this.job.takeExtraIngestibles)
					{
						Thing thing = GenClosest.ClosestThingReachable(<>c__DisplayClass17_.<>4__this.pawn.Position, <>c__DisplayClass17_.<>4__this.pawn.Map, ThingRequest.ForDef(<>c__DisplayClass17_.<>4__this.IngestibleSource.def), PathEndMode.Touch, TraverseParms.For(<>c__DisplayClass17_.<>4__this.pawn, Danger.Deadly, TraverseMode.ByPawn, false), 12f, (Thing x) => <>c__DisplayClass17_.<>4__this.pawn.CanReserve(x, 10, 1, null, false) && !x.IsForbidden(<>c__DisplayClass17_.<>4__this.pawn) && x.IsSociallyProper(<>c__DisplayClass17_.<>4__this.pawn), null, 0, -1, false, RegionType.Set_Passable, false);
						if (thing != null)
						{
							<>c__DisplayClass17_.<>4__this.job.SetTarget(TargetIndex.C, thing);
							<>c__DisplayClass17_.<>4__this.JumpToToil(<>c__DisplayClass17_.reserveExtraFoodToCollect);
						}
					}
				};
				findExtraFoodToCollect.defaultCompleteMode = ToilCompleteMode.Instant;
				yield return Toils_Jump.Jump(findExtraFoodToCollect);
				yield return <>c__DisplayClass17_.reserveExtraFoodToCollect;
				yield return Toils_Goto.GotoThing(TargetIndex.C, PathEndMode.Touch);
				yield return Toils_Haul.TakeToInventory(TargetIndex.C, () => this.job.takeExtraIngestibles - this.pawn.inventory.innerContainer.TotalStackCountOfDef(this.IngestibleSource.def));
				yield return findExtraFoodToCollect;
				<>c__DisplayClass17_ = null;
				findExtraFoodToCollect = null;
				gotoToPickup = null;
			}
			if (!this.IngestibleSource.def.IsDrug)
			{
				yield return Toils_Ingest.CarryIngestibleToChewSpot(this.pawn, TargetIndex.A).FailOnDestroyedOrNull(TargetIndex.A);
			}
			yield return Toils_Ingest.FindAdjacentEatSurface(TargetIndex.B, TargetIndex.A);
			yield break;
		}

		// Token: 0x06002D91 RID: 11665 RVA: 0x00100A5D File Offset: 0x000FEC5D
		private IEnumerable<Toil> PrepareToIngestToils_NonToolUser()
		{
			yield return this.ReserveFood();
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			yield break;
		}

		// Token: 0x06002D92 RID: 11666 RVA: 0x00100A6D File Offset: 0x000FEC6D
		private Toil ReserveFood()
		{
			return new Toil
			{
				initAction = delegate
				{
					if (this.pawn.Faction == null)
					{
						return;
					}
					Thing thing = this.job.GetTarget(TargetIndex.A).Thing;
					if (this.pawn.carryTracker.CarriedThing == thing)
					{
						return;
					}
					int maxAmountToPickup = FoodUtility.GetMaxAmountToPickup(thing, this.pawn, this.job.count);
					if (maxAmountToPickup == 0)
					{
						return;
					}
					if (!this.pawn.Reserve(thing, this.job, 10, maxAmountToPickup, null, true))
					{
						Log.Error(string.Concat(new object[]
						{
							"Pawn food reservation for ",
							this.pawn,
							" on job ",
							this,
							" failed, because it could not register food from ",
							thing,
							" - amount: ",
							maxAmountToPickup
						}), false);
						this.pawn.jobs.EndCurrentJob(JobCondition.Errored, true, true);
					}
					this.job.count = maxAmountToPickup;
				},
				defaultCompleteMode = ToilCompleteMode.Instant,
				atomicWithPrevious = true
			};
		}

		// Token: 0x06002D93 RID: 11667 RVA: 0x00100A94 File Offset: 0x000FEC94
		public override bool ModifyCarriedThingDrawPos(ref Vector3 drawPos, ref bool behind, ref bool flip)
		{
			IntVec3 cell = this.job.GetTarget(TargetIndex.B).Cell;
			return JobDriver_Ingest.ModifyCarriedThingDrawPosWorker(ref drawPos, ref behind, ref flip, cell, this.pawn);
		}

		// Token: 0x06002D94 RID: 11668 RVA: 0x00100AC8 File Offset: 0x000FECC8
		public static bool ModifyCarriedThingDrawPosWorker(ref Vector3 drawPos, ref bool behind, ref bool flip, IntVec3 placeCell, Pawn pawn)
		{
			if (pawn.pather.Moving)
			{
				return false;
			}
			Thing carriedThing = pawn.carryTracker.CarriedThing;
			if (carriedThing == null || !carriedThing.IngestibleNow)
			{
				return false;
			}
			if (placeCell.IsValid && placeCell.AdjacentToCardinal(pawn.Position) && placeCell.HasEatSurface(pawn.Map) && carriedThing.def.ingestible.ingestHoldUsesTable)
			{
				drawPos = new Vector3((float)placeCell.x + 0.5f, drawPos.y, (float)placeCell.z + 0.5f);
				return true;
			}
			if (carriedThing.def.ingestible.ingestHoldOffsetStanding != null)
			{
				HoldOffset holdOffset = carriedThing.def.ingestible.ingestHoldOffsetStanding.Pick(pawn.Rotation);
				if (holdOffset != null)
				{
					drawPos += holdOffset.offset;
					behind = holdOffset.behind;
					flip = holdOffset.flip;
					return true;
				}
			}
			return false;
		}

		// Token: 0x04001A29 RID: 6697
		private bool usingNutrientPasteDispenser;

		// Token: 0x04001A2A RID: 6698
		private bool eatingFromInventory;

		// Token: 0x04001A2B RID: 6699
		public const float EatCorpseBodyPartsUntilFoodLevelPct = 0.9f;

		// Token: 0x04001A2C RID: 6700
		public const TargetIndex IngestibleSourceInd = TargetIndex.A;

		// Token: 0x04001A2D RID: 6701
		private const TargetIndex TableCellInd = TargetIndex.B;

		// Token: 0x04001A2E RID: 6702
		private const TargetIndex ExtraIngestiblesToCollectInd = TargetIndex.C;
	}
}
