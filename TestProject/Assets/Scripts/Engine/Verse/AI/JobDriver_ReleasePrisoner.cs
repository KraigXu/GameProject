using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000510 RID: 1296
	public class JobDriver_ReleasePrisoner : JobDriver
	{
		// Token: 0x17000765 RID: 1893
		// (get) Token: 0x06002515 RID: 9493 RVA: 0x000DBEF4 File Offset: 0x000DA0F4
		private Pawn Prisoner
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x06002516 RID: 9494 RVA: 0x000DBF1A File Offset: 0x000DA11A
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Prisoner, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x06002517 RID: 9495 RVA: 0x000DBF3C File Offset: 0x000DA13C
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

		// Token: 0x04001686 RID: 5766
		private const TargetIndex PrisonerInd = TargetIndex.A;

		// Token: 0x04001687 RID: 5767
		private const TargetIndex ReleaseCellInd = TargetIndex.B;
	}
}
