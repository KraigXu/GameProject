using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200066F RID: 1647
	public class JobDriver_Refuel : JobDriver
	{
		// Token: 0x17000871 RID: 2161
		// (get) Token: 0x06002CDF RID: 11487 RVA: 0x000FE974 File Offset: 0x000FCB74
		protected Thing Refuelable
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x17000872 RID: 2162
		// (get) Token: 0x06002CE0 RID: 11488 RVA: 0x000FE995 File Offset: 0x000FCB95
		protected CompRefuelable RefuelableComp
		{
			get
			{
				return this.Refuelable.TryGetComp<CompRefuelable>();
			}
		}

		// Token: 0x17000873 RID: 2163
		// (get) Token: 0x06002CE1 RID: 11489 RVA: 0x000FE9A4 File Offset: 0x000FCBA4
		protected Thing Fuel
		{
			get
			{
				return this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		// Token: 0x06002CE2 RID: 11490 RVA: 0x000FE9C8 File Offset: 0x000FCBC8
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Refuelable, this.job, 1, -1, null, errorOnFailed) && this.pawn.Reserve(this.Fuel, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x06002CE3 RID: 11491 RVA: 0x000FEA19 File Offset: 0x000FCC19
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

		// Token: 0x040019FF RID: 6655
		private const TargetIndex RefuelableInd = TargetIndex.A;

		// Token: 0x04001A00 RID: 6656
		private const TargetIndex FuelInd = TargetIndex.B;

		// Token: 0x04001A01 RID: 6657
		private const int RefuelingDuration = 240;
	}
}
