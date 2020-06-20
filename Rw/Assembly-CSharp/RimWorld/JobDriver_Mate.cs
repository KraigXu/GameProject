using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200061E RID: 1566
	public class JobDriver_Mate : JobDriver
	{
		// Token: 0x17000818 RID: 2072
		// (get) Token: 0x06002ADC RID: 10972 RVA: 0x000F9EB8 File Offset: 0x000F80B8
		private Pawn Female
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x06002ADD RID: 10973 RVA: 0x0001028D File Offset: 0x0000E48D
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06002ADE RID: 10974 RVA: 0x000F9EDE File Offset: 0x000F80DE
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			this.FailOnDowned(TargetIndex.A);
			this.FailOnNotCasualInterruptible(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			Toil toil = Toils_General.WaitWith(TargetIndex.A, 500, false, false);
			toil.tickAction = delegate
			{
				if (this.pawn.IsHashIntervalTick(100))
				{
					MoteMaker.ThrowMetaIcon(this.pawn.Position, this.pawn.Map, ThingDefOf.Mote_Heart);
				}
				if (this.Female.IsHashIntervalTick(100))
				{
					MoteMaker.ThrowMetaIcon(this.Female.Position, this.pawn.Map, ThingDefOf.Mote_Heart);
				}
			};
			yield return toil;
			yield return Toils_General.Do(delegate
			{
				PawnUtility.Mated(this.pawn, this.Female);
			});
			yield break;
		}

		// Token: 0x0400197A RID: 6522
		private const int MateDuration = 500;

		// Token: 0x0400197B RID: 6523
		private const TargetIndex FemInd = TargetIndex.A;

		// Token: 0x0400197C RID: 6524
		private const int TicksBetweenHeartMotes = 100;
	}
}
