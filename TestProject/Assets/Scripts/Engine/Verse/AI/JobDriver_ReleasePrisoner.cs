using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	
	public class JobDriver_ReleasePrisoner : JobDriver
	{
		
		
		private Pawn Prisoner
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Prisoner, this.job, 1, -1, null, errorOnFailed);
		}

		
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDestroyedOrNull(TargetIndex.A);
			this.FailOnBurningImmobile(TargetIndex.B);
			this.FailOn(() => ((Pawn)((Thing)this.GetActor().CurJob.GetTarget(TargetIndex.A))).guest.interactionMode != PrisonerInteractionModeDefOf.Release);
			this.FailOnDowned(TargetIndex.A);
			this.FailOnAggroMentalState(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch).FailOn(() => !this.Prisoner.IsPrisonerOfColony || !this.Prisoner.guest.PrisonerIsSecure).FailOnSomeonePhysicallyInteracting(TargetIndex.A);
			yield return Toils_Haul.StartCarryThing(TargetIndex.A, false, false, false);
			Toil carryToCell = Toils_Haul.CarryHauledThingToCell(TargetIndex.B);
			yield return carryToCell;
			yield return Toils_Haul.PlaceHauledThingInCell(TargetIndex.B, carryToCell, false, false);
			Toil setReleased = new Toil();
			setReleased.initAction = delegate
			{
				Pawn pawn = setReleased.actor.jobs.curJob.targetA.Thing as Pawn;
				GenGuest.PrisonerRelease(pawn);
				pawn.guest.ClearLastRecruiterData();
				if (!PawnBanishUtility.WouldBeLeftToDie(pawn, pawn.Map.Tile))
				{
					GenGuest.AddHealthyPrisonerReleasedThoughts(pawn);
				}
			};
			yield return setReleased;
			yield break;
		}

		
		private const TargetIndex PrisonerInd = TargetIndex.A;

		
		private const TargetIndex ReleaseCellInd = TargetIndex.B;
	}
}
