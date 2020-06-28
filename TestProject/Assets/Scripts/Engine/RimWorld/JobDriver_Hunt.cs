using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000661 RID: 1633
	public class JobDriver_Hunt : JobDriver
	{
		// Token: 0x17000865 RID: 2149
		// (get) Token: 0x06002C88 RID: 11400 RVA: 0x000FDC68 File Offset: 0x000FBE68
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

		// Token: 0x17000866 RID: 2150
		// (get) Token: 0x06002C89 RID: 11401 RVA: 0x000FDCA0 File Offset: 0x000FBEA0
		private Corpse Corpse
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing as Corpse;
			}
		}

		// Token: 0x06002C8A RID: 11402 RVA: 0x000FDCC6 File Offset: 0x000FBEC6
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.jobStartTick, "jobStartTick", 0, false);
		}

		// Token: 0x06002C8B RID: 11403 RVA: 0x000FDCE0 File Offset: 0x000FBEE0
		public override string GetReport()
		{
			if (this.Victim != null)
			{
				return JobUtility.GetResolvedJobReport(this.job.def.reportString, this.Victim);
			}
			return base.GetReport();
		}

		// Token: 0x06002C8C RID: 11404 RVA: 0x000FDD11 File Offset: 0x000FBF11
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Victim, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x06002C8D RID: 11405 RVA: 0x000FDD33 File Offset: 0x000FBF33
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

		// Token: 0x06002C8E RID: 11406 RVA: 0x000FDD44 File Offset: 0x000FBF44
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

		// Token: 0x040019E5 RID: 6629
		private int jobStartTick = -1;

		// Token: 0x040019E6 RID: 6630
		private const TargetIndex VictimInd = TargetIndex.A;

		// Token: 0x040019E7 RID: 6631
		private const TargetIndex CorpseInd = TargetIndex.A;

		// Token: 0x040019E8 RID: 6632
		private const TargetIndex StoreCellInd = TargetIndex.B;

		// Token: 0x040019E9 RID: 6633
		private const int MaxHuntTicks = 5000;
	}
}
