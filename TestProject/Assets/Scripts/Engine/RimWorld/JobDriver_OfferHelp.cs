using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200066B RID: 1643
	public class JobDriver_OfferHelp : JobDriver
	{
		// Token: 0x1700086F RID: 2159
		// (get) Token: 0x06002CCE RID: 11470 RVA: 0x000FE878 File Offset: 0x000FCA78
		public Pawn OtherPawn
		{
			get
			{
				return (Pawn)this.pawn.CurJob.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x06002CCF RID: 11471 RVA: 0x0001028D File Offset: 0x0000E48D
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06002CD0 RID: 11472 RVA: 0x000FE8A3 File Offset: 0x000FCAA3
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			this.FailOn(() => !this.OtherPawn.mindState.WillJoinColonyIfRescued);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			yield return Toils_General.DoAtomic(delegate
			{
				this.OtherPawn.mindState.JoinColonyBecauseRescuedBy(this.pawn);
			});
			yield break;
		}

		// Token: 0x040019FD RID: 6653
		private const TargetIndex OtherPawnInd = TargetIndex.A;
	}
}
