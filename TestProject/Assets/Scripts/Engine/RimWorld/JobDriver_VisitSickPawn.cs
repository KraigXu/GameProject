using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	
	public class JobDriver_VisitSickPawn : JobDriver
	{
		
		
		private Pawn Patient
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		
		
		private Thing Chair
		{
			get
			{
				return this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Patient, this.job, 1, -1, null, errorOnFailed) && (this.Chair == null || this.pawn.Reserve(this.Chair, this.job, 1, -1, null, errorOnFailed));
		}

		
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			this.FailOn(() => !this.Patient.InBed() || !this.Patient.Awake());
			if (this.Chair != null)
			{
				this.FailOnDespawnedNullOrForbidden(TargetIndex.B);
			}
			if (this.Chair != null)
			{
				yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.OnCell);
			}
			else
			{
				yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell);
			}
			yield return Toils_Interpersonal.WaitToBeAbleToInteract(this.pawn);
			yield return new Toil
			{
				tickAction = delegate
				{
					this.Patient.needs.joy.GainJoy(this.job.def.joyGainRate * 0.000144f, this.job.def.joyKind);
					if (this.pawn.IsHashIntervalTick(320))
					{
						InteractionDef intDef = (Rand.Value < 0.8f) ? InteractionDefOf.Chitchat : InteractionDefOf.DeepTalk;
						this.pawn.interactions.TryInteractWith(this.Patient, intDef);
					}
					this.pawn.rotationTracker.FaceCell(this.Patient.Position);
					this.pawn.GainComfortFromCellIfPossible(false);
					JoyUtility.JoyTickCheckEnd(this.pawn, JoyTickFullJoyAction.None, 1f, null);
					if (this.pawn.needs.joy.CurLevelPercentage > 0.9999f && this.Patient.needs.joy.CurLevelPercentage > 0.9999f)
					{
						this.pawn.jobs.EndCurrentJob(JobCondition.Succeeded, true, true);
					}
				},
				handlingFacing = true,
				socialMode = RandomSocialMode.Off,
				defaultCompleteMode = ToilCompleteMode.Delay,
				defaultDuration = this.job.def.joyDuration
			};
			yield break;
		}

		
		private const TargetIndex PatientInd = TargetIndex.A;

		
		private const TargetIndex ChairInd = TargetIndex.B;
	}
}
