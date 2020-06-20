using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000670 RID: 1648
	public class JobDriver_RefuelAtomic : JobDriver
	{
		// Token: 0x17000874 RID: 2164
		// (get) Token: 0x06002CE9 RID: 11497 RVA: 0x000FEA94 File Offset: 0x000FCC94
		protected Thing Refuelable
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x17000875 RID: 2165
		// (get) Token: 0x06002CEA RID: 11498 RVA: 0x000FEAB5 File Offset: 0x000FCCB5
		protected CompRefuelable RefuelableComp
		{
			get
			{
				return this.Refuelable.TryGetComp<CompRefuelable>();
			}
		}

		// Token: 0x17000876 RID: 2166
		// (get) Token: 0x06002CEB RID: 11499 RVA: 0x000FEAC4 File Offset: 0x000FCCC4
		protected Thing Fuel
		{
			get
			{
				return this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		// Token: 0x06002CEC RID: 11500 RVA: 0x000FEAE8 File Offset: 0x000FCCE8
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			this.pawn.ReserveAsManyAsPossible(this.job.GetTargetQueue(TargetIndex.B), this.job, 1, -1, null);
			return this.pawn.Reserve(this.Refuelable, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x06002CED RID: 11501 RVA: 0x000FEB35 File Offset: 0x000FCD35
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
			base.AddFailCondition(() => (!this.job.playerForced && !this.RefuelableComp.ShouldAutoRefuelNowIgnoringFuelPct) || !this.RefuelableComp.allowAutoRefuel);
			base.AddFailCondition(() => !this.RefuelableComp.allowAutoRefuel && !this.job.playerForced);
			yield return Toils_General.DoAtomic(delegate
			{
				this.job.count = this.RefuelableComp.GetFuelCountToFullyRefuel();
			});
			Toil getNextIngredient = Toils_General.Label();
			yield return getNextIngredient;
			yield return Toils_JobTransforms.ExtractNextTargetFromQueue(TargetIndex.B, true);
			yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.ClosestTouch).FailOnDespawnedNullOrForbidden(TargetIndex.B).FailOnSomeonePhysicallyInteracting(TargetIndex.B);
			yield return Toils_Haul.StartCarryThing(TargetIndex.B, false, true, false).FailOnDestroyedNullOrForbidden(TargetIndex.B);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			Toil findPlaceTarget = Toils_JobTransforms.SetTargetToIngredientPlaceCell(TargetIndex.A, TargetIndex.B, TargetIndex.C);
			yield return findPlaceTarget;
			yield return Toils_Haul.PlaceHauledThingInCell(TargetIndex.C, findPlaceTarget, false, false);
			yield return Toils_Jump.JumpIf(getNextIngredient, () => !this.job.GetTargetQueue(TargetIndex.B).NullOrEmpty<LocalTargetInfo>());
			findPlaceTarget = null;
			yield return Toils_General.Wait(240, TargetIndex.None).FailOnDestroyedNullOrForbidden(TargetIndex.A).FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch).WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
			yield return Toils_Refuel.FinalizeRefueling(TargetIndex.A, TargetIndex.None);
			yield break;
		}

		// Token: 0x04001A02 RID: 6658
		private const TargetIndex RefuelableInd = TargetIndex.A;

		// Token: 0x04001A03 RID: 6659
		private const TargetIndex FuelInd = TargetIndex.B;

		// Token: 0x04001A04 RID: 6660
		private const TargetIndex FuelPlaceCellInd = TargetIndex.C;

		// Token: 0x04001A05 RID: 6661
		private const int RefuelingDuration = 240;
	}
}
