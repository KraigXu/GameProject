using System;
using System.Collections.Generic;
using Verse.AI;

namespace RimWorld
{
	
	public class JobDriver_Flee : JobDriver
	{
		
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			this.pawn.Map.pawnDestinationReservationManager.Reserve(this.pawn, this.job, this.job.GetTarget(TargetIndex.A).Cell);
			return true;
		}

		
		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return new Toil
			{
				atomicWithPrevious = true,
				defaultCompleteMode = ToilCompleteMode.Instant,
				initAction = delegate
				{
					if (this.pawn.IsColonist)
					{
						MoteMaker.MakeColonistActionOverlay(this.pawn, ThingDefOf.Mote_ColonistFleeing);
					}
				}
			};
			yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
			yield break;
		}

		
		protected const TargetIndex DestInd = TargetIndex.A;

		
		protected const TargetIndex DangerInd = TargetIndex.B;
	}
}
