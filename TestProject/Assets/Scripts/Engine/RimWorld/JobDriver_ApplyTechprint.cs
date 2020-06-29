using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	
	public class JobDriver_ApplyTechprint : JobDriver
	{
		
		
		protected Building_ResearchBench ResearchBench
		{
			get
			{
				return (Building_ResearchBench)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		
		
		protected Thing Techprint
		{
			get
			{
				return this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		
		
		protected CompTechprint TechprintComp
		{
			get
			{
				return this.Techprint.TryGetComp<CompTechprint>();
			}
		}

		
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.ResearchBench, this.job, 1, -1, null, errorOnFailed) && this.pawn.Reserve(this.Techprint, this.job, 1, -1, null, errorOnFailed);
		}

		
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			this.FailOnBurningImmobile(TargetIndex.A);
			yield return Toils_General.DoAtomic(delegate
			{
				this.job.count = 1;
			});
			Toil toil = Toils_Reserve.Reserve(TargetIndex.B, 1, -1, null);
			yield return toil;
			yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.ClosestTouch).FailOnDespawnedNullOrForbidden(TargetIndex.B).FailOnSomeonePhysicallyInteracting(TargetIndex.B);
			yield return Toils_Haul.StartCarryThing(TargetIndex.B, false, true, false).FailOnDestroyedNullOrForbidden(TargetIndex.B);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell);
			yield return Toils_Haul.PlaceHauledThingInCell(TargetIndex.C, null, false, false);
			yield return Toils_General.Wait(600, TargetIndex.None).FailOnDestroyedNullOrForbidden(TargetIndex.B).FailOnDestroyedNullOrForbidden(TargetIndex.A).FailOnCannotTouch(TargetIndex.A, PathEndMode.InteractionCell).WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
			yield return new Toil
			{
				initAction = delegate
				{
					Find.ResearchManager.ApplyTechprint(this.TechprintComp.Props.project, this.pawn);
					this.Techprint.Destroy(DestroyMode.Vanish);
					SoundDefOf.TechprintApplied.PlayOneShotOnCamera(null);
				},
				defaultCompleteMode = ToilCompleteMode.Instant
			};
			yield break;
		}

		
		private const TargetIndex ResearchBenchInd = TargetIndex.A;

		
		private const TargetIndex TechprintInd = TargetIndex.B;

		
		private const TargetIndex HaulCell = TargetIndex.C;

		
		private const int Duration = 600;
	}
}
