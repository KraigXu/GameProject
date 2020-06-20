using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000517 RID: 1303
	public class JobDriver_Follow : JobDriver
	{
		// Token: 0x06002542 RID: 9538 RVA: 0x0001028D File Offset: 0x0000E48D
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06002543 RID: 9539 RVA: 0x000DD554 File Offset: 0x000DB754
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

		// Token: 0x06002544 RID: 9540 RVA: 0x000DD564 File Offset: 0x000DB764
		public override bool IsContinuation(Job j)
		{
			return this.job.GetTarget(TargetIndex.A) == j.GetTarget(TargetIndex.A);
		}

		// Token: 0x040016D8 RID: 5848
		private const TargetIndex FolloweeInd = TargetIndex.A;

		// Token: 0x040016D9 RID: 5849
		private const int Distance = 4;
	}
}
