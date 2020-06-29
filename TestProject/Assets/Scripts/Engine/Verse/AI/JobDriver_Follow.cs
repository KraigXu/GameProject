using System;
using System.Collections.Generic;

namespace Verse.AI
{
	
	public class JobDriver_Follow : JobDriver
	{
		
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			yield return new Toil
			{
				tickAction = delegate
				{
					Pawn pawn = (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
					if (this.pawn.Position.InHorDistOf(pawn.Position, 4f) && this.pawn.Position.WithinRegions(pawn.Position, base.Map, 2, TraverseParms.For(this.pawn, Danger.Deadly, TraverseMode.ByPawn, false), RegionType.Set_Passable))
					{
						return;
					}
					if (!this.pawn.CanReach(pawn, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn))
					{
						base.EndJobWith(JobCondition.Incompletable);
						return;
					}
					if (!this.pawn.pather.Moving || this.pawn.pather.Destination != pawn)
					{
						this.pawn.pather.StartPath(pawn, PathEndMode.Touch);
					}
				},
				defaultCompleteMode = ToilCompleteMode.Never
			};
			yield break;
		}

		
		public override bool IsContinuation(Job j)
		{
			return this.job.GetTarget(TargetIndex.A) == j.GetTarget(TargetIndex.A);
		}

		
		private const TargetIndex FolloweeInd = TargetIndex.A;

		
		private const int Distance = 4;
	}
}
