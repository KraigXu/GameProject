﻿using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	
	public class JobDriver_Refuel : JobDriver
	{
		
		
		protected Thing Refuelable
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		
		
		protected CompRefuelable RefuelableComp
		{
			get
			{
				return this.Refuelable.TryGetComp<CompRefuelable>();
			}
		}

		
		
		protected Thing Fuel
		{
			get
			{
				return this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Refuelable, this.job, 1, -1, null, errorOnFailed) && this.pawn.Reserve(this.Fuel, this.job, 1, -1, null, errorOnFailed);
		}

		
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			base.AddEndCondition(delegate
			{
				if (!this.RefuelableComp.IsFull)
				{
					return JobCondition.Ongoing;
				}
				return JobCondition.Succeeded;
			});
			base.AddFailCondition(() => !this.job.playerForced && !this.RefuelableComp.ShouldAutoRefuelNowIgnoringFuelPct);
			base.AddFailCondition(() => !this.RefuelableComp.allowAutoRefuel && !this.job.playerForced);
			yield return Toils_General.DoAtomic(delegate
			{
				this.job.count = this.RefuelableComp.GetFuelCountToFullyRefuel();
			});
			Toil reserveFuel = Toils_Reserve.Reserve(TargetIndex.B, 1, -1, null);
			yield return reserveFuel;
			yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.ClosestTouch).FailOnDespawnedNullOrForbidden(TargetIndex.B).FailOnSomeonePhysicallyInteracting(TargetIndex.B);
			yield return Toils_Haul.StartCarryThing(TargetIndex.B, false, true, false).FailOnDestroyedNullOrForbidden(TargetIndex.B);
			yield return Toils_Haul.CheckForGetOpportunityDuplicate(reserveFuel, TargetIndex.B, TargetIndex.None, true, null);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			yield return Toils_General.Wait(240, TargetIndex.None).FailOnDestroyedNullOrForbidden(TargetIndex.B).FailOnDestroyedNullOrForbidden(TargetIndex.A).FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch).WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
			yield return Toils_Refuel.FinalizeRefueling(TargetIndex.A, TargetIndex.B);
			yield break;
		}

		
		private const TargetIndex RefuelableInd = TargetIndex.A;

		
		private const TargetIndex FuelInd = TargetIndex.B;

		
		private const int RefuelingDuration = 240;
	}
}
