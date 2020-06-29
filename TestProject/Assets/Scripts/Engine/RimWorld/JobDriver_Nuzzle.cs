using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	
	public class JobDriver_Nuzzle : JobDriver
	{
		
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			this.FailOnNotCasualInterruptible(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			yield return Toils_Interpersonal.WaitToBeAbleToInteract(this.pawn);
			Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).socialMode = RandomSocialMode.Off;
			Toils_General.WaitWith(TargetIndex.A, 100, false, true).socialMode = RandomSocialMode.Off;
			yield return Toils_General.Do(delegate
			{
				Pawn recipient = (Pawn)this.pawn.CurJob.targetA.Thing;
				this.pawn.interactions.TryInteractWith(recipient, InteractionDefOf.Nuzzle);
			});
			yield break;
		}

		
		private const int NuzzleDuration = 100;
	}
}
