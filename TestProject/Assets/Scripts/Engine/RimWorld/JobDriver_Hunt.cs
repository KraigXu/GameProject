﻿using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	
	public class JobDriver_Hunt : JobDriver
	{
		
		
		public Pawn Victim
		{
			get
			{
				Corpse corpse = this.Corpse;
				if (corpse != null)
				{
					return corpse.InnerPawn;
				}
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		
		
		private Corpse Corpse
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing as Corpse;
			}
		}

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.jobStartTick, "jobStartTick", 0, false);
		}

		
		public override string GetReport()
		{
			if (this.Victim != null)
			{
				return JobUtility.GetResolvedJobReport(this.job.def.reportString, this.Victim);
			}
			return base.GetReport();
		}

		
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Victim, this.job, 1, -1, null, errorOnFailed);
		}

		
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOn(delegate
			{
				if (!this.job.ignoreDesignations)
				{
					Pawn victim = this.Victim;
					if (victim != null && !victim.Dead && base.Map.designationManager.DesignationOn(victim, DesignationDefOf.Hunt) == null)
					{
						return true;
					}
				}
				return false;
			});
			yield return new Toil
			{
				initAction = delegate
				{
					this.jobStartTick = Find.TickManager.TicksGame;
				}
			};
			yield return Toils_Combat.TrySetJobToUseAttackVerb(TargetIndex.A);
			Toil startCollectCorpseLabel = Toils_General.Label();
			Toil slaughterLabel = Toils_General.Label();
			Toil gotoCastPos = Toils_Combat.GotoCastPosition(TargetIndex.A, true, 0.95f).JumpIfDespawnedOrNull(TargetIndex.A, startCollectCorpseLabel).FailOn(() => Find.TickManager.TicksGame > this.jobStartTick + 5000);
			yield return gotoCastPos;
			Toil slaughterIfPossible = Toils_Jump.JumpIf(slaughterLabel, delegate
			{
				Pawn victim = this.Victim;
				return (victim.RaceProps.DeathActionWorker == null || !victim.RaceProps.DeathActionWorker.DangerousInMelee) && victim.Downed;
			});
			yield return slaughterIfPossible;
			yield return Toils_Jump.JumpIfTargetNotHittable(TargetIndex.A, gotoCastPos);
			yield return Toils_Combat.CastVerb(TargetIndex.A, false).JumpIfDespawnedOrNull(TargetIndex.A, startCollectCorpseLabel).FailOn(() => Find.TickManager.TicksGame > this.jobStartTick + 5000);
			yield return Toils_Jump.JumpIfTargetDespawnedOrNull(TargetIndex.A, startCollectCorpseLabel);
			yield return Toils_Jump.Jump(slaughterIfPossible);
			yield return slaughterLabel;
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).FailOnMobile(TargetIndex.A);
			yield return Toils_General.WaitWith(TargetIndex.A, 180, true, false).FailOnMobile(TargetIndex.A);
			yield return Toils_General.Do(delegate
			{
				if (this.Victim.Dead)
				{
					return;
				}
				ExecutionUtility.DoExecutionByCut(this.pawn, this.Victim);
				this.pawn.records.Increment(RecordDefOf.AnimalsSlaughtered);
				if (this.pawn.InMentalState)
				{
					this.pawn.MentalState.Notify_SlaughteredAnimal();
				}
			});
			yield return Toils_Jump.Jump(startCollectCorpseLabel);
			yield return startCollectCorpseLabel;
			yield return this.StartCollectCorpseToil();
			yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.ClosestTouch).FailOnDespawnedNullOrForbidden(TargetIndex.A).FailOnSomeonePhysicallyInteracting(TargetIndex.A);
			yield return Toils_Haul.StartCarryThing(TargetIndex.A, false, false, false);
			Toil carryToCell = Toils_Haul.CarryHauledThingToCell(TargetIndex.B);
			yield return carryToCell;
			yield return Toils_Haul.PlaceHauledThingInCell(TargetIndex.B, carryToCell, true, false);
			yield break;
		}

		
		private Toil StartCollectCorpseToil()
		{
			Toil toil = new Toil();
			toil.initAction = delegate
			{
				if (this.Victim == null)
				{
					toil.actor.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
					return;
				}
				TaleRecorder.RecordTale(TaleDefOf.Hunted, new object[]
				{
					this.pawn,
					this.Victim
				});
				Corpse corpse = this.Victim.Corpse;
				if (corpse == null || !this.pawn.CanReserveAndReach(corpse, PathEndMode.ClosestTouch, Danger.Deadly, 1, -1, null, false))
				{
					this.pawn.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
					return;
				}
				corpse.SetForbidden(false, true);
				IntVec3 c;
				if (StoreUtility.TryFindBestBetterStoreCellFor(corpse, this.pawn, this.Map, StoragePriority.Unstored, this.pawn.Faction, out c, true))
				{
					this.pawn.Reserve(corpse, this.job, 1, -1, null, true);
					this.pawn.Reserve(c, this.job, 1, -1, null, true);
					this.job.SetTarget(TargetIndex.B, c);
					this.job.SetTarget(TargetIndex.A, corpse);
					this.job.count = 1;
					this.job.haulMode = HaulMode.ToCellStorage;
					return;
				}
				this.pawn.jobs.EndCurrentJob(JobCondition.Succeeded, true, true);
			};
			return toil;
		}

		
		private int jobStartTick = -1;

		
		private const TargetIndex VictimInd = TargetIndex.A;

		
		private const TargetIndex CorpseInd = TargetIndex.A;

		
		private const TargetIndex StoreCellInd = TargetIndex.B;

		
		private const int MaxHuntTicks = 5000;
	}
}
