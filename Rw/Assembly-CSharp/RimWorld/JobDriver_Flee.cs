using System;
using System.Collections.Generic;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200065C RID: 1628
	public class JobDriver_Flee : JobDriver
	{
		// Token: 0x06002C67 RID: 11367 RVA: 0x000FD6C8 File Offset: 0x000FB8C8
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			this.pawn.Map.pawnDestinationReservationManager.Reserve(this.pawn, this.job, this.job.GetTarget(TargetIndex.A).Cell);
			return true;
		}

		// Token: 0x06002C68 RID: 11368 RVA: 0x000FD70B File Offset: 0x000FB90B
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

		// Token: 0x040019DC RID: 6620
		protected const TargetIndex DestInd = TargetIndex.A;

		// Token: 0x040019DD RID: 6621
		protected const TargetIndex DangerInd = TargetIndex.B;
	}
}
