using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000622 RID: 1570
	public abstract class JobDriver_InteractAnimal : JobDriver
	{
		// Token: 0x1700081C RID: 2076
		// (get) Token: 0x06002AF5 RID: 10997 RVA: 0x000FA2DB File Offset: 0x000F84DB
		protected Pawn Animal
		{
			get
			{
				return (Pawn)this.job.targetA.Thing;
			}
		}

		// Token: 0x1700081D RID: 2077
		// (get) Token: 0x06002AF6 RID: 10998 RVA: 0x0001028D File Offset: 0x0000E48D
		protected virtual bool CanInteractNow
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002AF7 RID: 10999 RVA: 0x000FA2F2 File Offset: 0x000F84F2
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.feedNutritionLeft, "feedNutritionLeft", 0f, false);
		}

		// Token: 0x06002AF8 RID: 11000
		protected abstract Toil FinalInteractToil();

		// Token: 0x06002AF9 RID: 11001 RVA: 0x000FA310 File Offset: 0x000F8510
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Animal, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x06002AFA RID: 11002 RVA: 0x000FA332 File Offset: 0x000F8532
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			this.FailOnDowned(TargetIndex.A);
			this.FailOnNotCasualInterruptible(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			yield return Toils_Interpersonal.WaitToBeAbleToInteract(this.pawn);
			yield return Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A);
			yield return this.TalkToAnimal(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			yield return Toils_Interpersonal.WaitToBeAbleToInteract(this.pawn);
			yield return Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A);
			yield return this.TalkToAnimal(TargetIndex.A);
			foreach (Toil toil in this.FeedToils())
			{
				yield return toil;
			}
			IEnumerator<Toil> enumerator = null;
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			yield return Toils_Interpersonal.WaitToBeAbleToInteract(this.pawn);
			yield return Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A);
			yield return this.TalkToAnimal(TargetIndex.A);
			foreach (Toil toil2 in this.FeedToils())
			{
				yield return toil2;
			}
			enumerator = null;
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).FailOn(() => !this.CanInteractNow);
			yield return Toils_Interpersonal.SetLastInteractTime(TargetIndex.A);
			yield return Toils_Interpersonal.WaitToBeAbleToInteract(this.pawn);
			yield return Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A);
			yield return this.FinalInteractToil();
			yield break;
			yield break;
		}

		// Token: 0x06002AFB RID: 11003 RVA: 0x000FA342 File Offset: 0x000F8542
		public static float RequiredNutritionPerFeed(Pawn animal)
		{
			return Mathf.Min(animal.needs.food.MaxLevel * 0.15f, 0.3f);
		}

		// Token: 0x06002AFC RID: 11004 RVA: 0x000FA364 File Offset: 0x000F8564
		private IEnumerable<Toil> FeedToils()
		{
			yield return new Toil
			{
				initAction = delegate
				{
					this.feedNutritionLeft = JobDriver_InteractAnimal.RequiredNutritionPerFeed(this.Animal);
				},
				defaultCompleteMode = ToilCompleteMode.Instant
			};
			Toil gotoAnimal = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			yield return gotoAnimal;
			yield return this.StartFeedAnimal(TargetIndex.A);
			yield return Toils_Ingest.FinalizeIngest(this.Animal, TargetIndex.B);
			yield return Toils_General.PutCarriedThingInInventory();
			yield return Toils_General.ClearTarget(TargetIndex.B);
			yield return Toils_Jump.JumpIf(gotoAnimal, () => this.feedNutritionLeft > 0f);
			yield break;
		}

		// Token: 0x06002AFD RID: 11005 RVA: 0x000FA374 File Offset: 0x000F8574
		private Toil TalkToAnimal(TargetIndex tameeInd)
		{
			Toil toil = new Toil();
			toil.initAction = delegate
			{
				Pawn actor = toil.GetActor();
				Pawn recipient = (Pawn)((Thing)actor.CurJob.GetTarget(tameeInd));
				actor.interactions.TryInteractWith(recipient, InteractionDefOf.AnimalChat);
			};
			toil.FailOn(() => !this.CanInteractNow);
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			toil.defaultDuration = 270;
			return toil;
		}

		// Token: 0x06002AFE RID: 11006 RVA: 0x000FA3F4 File Offset: 0x000F85F4
		private Toil StartFeedAnimal(TargetIndex tameeInd)
		{
			Toil toil = new Toil();
			toil.initAction = delegate
			{
				Pawn actor = toil.GetActor();
				Pawn pawn = (Pawn)((Thing)actor.CurJob.GetTarget(tameeInd));
				PawnUtility.ForceWait(pawn, 270, actor, false);
				Thing thing = FoodUtility.BestFoodInInventory(actor, pawn, FoodPreferability.NeverForNutrition, FoodPreferability.RawTasty, 0f, false);
				if (thing == null)
				{
					actor.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
					return;
				}
				actor.mindState.lastInventoryRawFoodUseTick = Find.TickManager.TicksGame;
				int num = FoodUtility.StackCountForNutrition(this.feedNutritionLeft, thing.GetStatValue(StatDefOf.Nutrition, true));
				int stackCount = thing.stackCount;
				Thing thing2 = actor.inventory.innerContainer.Take(thing, Mathf.Min(num, stackCount));
				actor.carryTracker.TryStartCarry(thing2);
				actor.CurJob.SetTarget(TargetIndex.B, thing2);
				float num2 = (float)thing2.stackCount * thing2.GetStatValue(StatDefOf.Nutrition, true);
				this.ticksLeftThisToil = Mathf.CeilToInt(270f * (num2 / JobDriver_InteractAnimal.RequiredNutritionPerFeed(pawn)));
				if (num <= stackCount)
				{
					this.feedNutritionLeft = 0f;
					return;
				}
				this.feedNutritionLeft -= num2;
				if (this.feedNutritionLeft < 0.001f)
				{
					this.feedNutritionLeft = 0f;
				}
			};
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			return toil;
		}

		// Token: 0x04001986 RID: 6534
		protected const TargetIndex AnimalInd = TargetIndex.A;

		// Token: 0x04001987 RID: 6535
		private const TargetIndex FoodHandInd = TargetIndex.B;

		// Token: 0x04001988 RID: 6536
		private const int FeedDuration = 270;

		// Token: 0x04001989 RID: 6537
		private const int TalkDuration = 270;

		// Token: 0x0400198A RID: 6538
		private const float NutritionPercentagePerFeed = 0.15f;

		// Token: 0x0400198B RID: 6539
		private const float MaxMinNutritionPerFeed = 0.3f;

		// Token: 0x0400198C RID: 6540
		public const int FeedCount = 2;

		// Token: 0x0400198D RID: 6541
		public const FoodPreferability MaxFoodPreferability = FoodPreferability.RawTasty;

		// Token: 0x0400198E RID: 6542
		private float feedNutritionLeft;
	}
}
