using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	
	public class JobDriver_HaulToContainer : JobDriver
	{
		
		// (get) Token: 0x0600259A RID: 9626 RVA: 0x000DEE22 File Offset: 0x000DD022
		public Thing ThingToCarry
		{
			get
			{
				return (Thing)this.job.GetTarget(TargetIndex.A);
			}
		}

		
		// (get) Token: 0x0600259B RID: 9627 RVA: 0x000DEE35 File Offset: 0x000DD035
		public Thing Container
		{
			get
			{
				return (Thing)this.job.GetTarget(TargetIndex.B);
			}
		}

		
		// (get) Token: 0x0600259C RID: 9628 RVA: 0x000DEE48 File Offset: 0x000DD048
		private int Duration
		{
			get
			{
				if (this.Container == null || !(this.Container is Building))
				{
					return 0;
				}
				return this.Container.def.building.haulToContainerDuration;
			}
		}

		
		public override string GetReport()
		{
			Thing thing;
			if (this.pawn.CurJob == this.job && this.pawn.carryTracker.CarriedThing != null)
			{
				thing = this.pawn.carryTracker.CarriedThing;
			}
			else
			{
				thing = base.TargetThingA;
			}
			if (thing == null || !this.job.targetB.HasThing)
			{
				return "ReportHaulingUnknown".Translate();
			}
			return ((this.job.GetTarget(TargetIndex.B).Thing is Building_Grave) ? "ReportHaulingToGrave" : "ReportHaulingTo").Translate(thing.Label, this.job.targetB.Thing.LabelShort.Named("DESTINATION"), thing.Named("THING"));
		}

		
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			if (!this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 1, -1, null, errorOnFailed))
			{
				return false;
			}
			if (!this.pawn.Reserve(this.job.GetTarget(TargetIndex.B), this.job, 1, -1, null, errorOnFailed))
			{
				return false;
			}
			this.pawn.ReserveAsManyAsPossible(this.job.GetTargetQueue(TargetIndex.A), this.job, 1, -1, null);
			this.pawn.ReserveAsManyAsPossible(this.job.GetTargetQueue(TargetIndex.B), this.job, 1, -1, null);
			return true;
		}

		
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDestroyedOrNull(TargetIndex.A);
			this.FailOnDestroyedNullOrForbidden(TargetIndex.B);
			this.FailOn(() => TransporterUtility.WasLoadingCanceled(this.Container));
			this.FailOn(delegate
			{
				ThingOwner thingOwner = this.Container.TryGetInnerInteractableThingOwner();
				if (thingOwner != null && !thingOwner.CanAcceptAnyOf(this.ThingToCarry, true))
				{
					return true;
				}
				IHaulDestination haulDestination = this.Container as IHaulDestination;
				return haulDestination != null && !haulDestination.Accepts(this.ThingToCarry);
			});
			Toil getToHaulTarget = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch).FailOnSomeonePhysicallyInteracting(TargetIndex.A);
			yield return getToHaulTarget;
			yield return Toils_Construct.UninstallIfMinifiable(TargetIndex.A).FailOnSomeonePhysicallyInteracting(TargetIndex.A);
			yield return Toils_Haul.StartCarryThing(TargetIndex.A, false, true, false);
			yield return Toils_Haul.JumpIfAlsoCollectingNextTargetInQueue(getToHaulTarget, TargetIndex.A);
			Toil carryToContainer = Toils_Haul.CarryHauledThingToContainer();
			yield return carryToContainer;
			yield return Toils_Goto.MoveOffTargetBlueprint(TargetIndex.B);
			Toil toil = Toils_General.Wait(this.Duration, TargetIndex.B);
			toil.WithProgressBarToilDelay(TargetIndex.B, false, -0.5f);
			yield return toil;
			yield return Toils_Construct.MakeSolidThingFromBlueprintIfNecessary(TargetIndex.B, TargetIndex.C);
			yield return Toils_Haul.DepositHauledThingInContainer(TargetIndex.B, TargetIndex.C);
			yield return Toils_Haul.JumpToCarryToNextContainerIfPossible(carryToContainer, TargetIndex.C);
			yield break;
		}

		
		protected const TargetIndex CarryThingIndex = TargetIndex.A;

		
		public const TargetIndex DestIndex = TargetIndex.B;

		
		protected const TargetIndex PrimaryDestIndex = TargetIndex.C;
	}
}
