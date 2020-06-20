using System;
using System.Collections.Generic;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000658 RID: 1624
	public class JobDriver_ClearSnow : JobDriver
	{
		// Token: 0x1700085C RID: 2140
		// (get) Token: 0x06002C52 RID: 11346 RVA: 0x000FD515 File Offset: 0x000FB715
		private float TotalNeededWork
		{
			get
			{
				return 50f * base.Map.snowGrid.GetDepth(base.TargetLocA);
			}
		}

		// Token: 0x06002C53 RID: 11347 RVA: 0x000DDBC6 File Offset: 0x000DBDC6
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x06002C54 RID: 11348 RVA: 0x000FD533 File Offset: 0x000FB733
		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.Touch);
			Toil clearToil = new Toil();
			clearToil.tickAction = delegate
			{
				float statValue = clearToil.actor.GetStatValue(StatDefOf.GeneralLaborSpeed, true);
				this.workDone += statValue;
				if (this.workDone >= this.TotalNeededWork)
				{
					this.Map.snowGrid.SetDepth(this.TargetLocA, 0f);
					this.ReadyForNextToil();
					return;
				}
			};
			clearToil.defaultCompleteMode = ToilCompleteMode.Never;
			clearToil.WithEffect(EffecterDefOf.ClearSnow, TargetIndex.A);
			clearToil.PlaySustainerOrSound(() => SoundDefOf.Interact_CleanFilth);
			clearToil.WithProgressBar(TargetIndex.A, () => this.workDone / this.TotalNeededWork, true, -0.5f);
			clearToil.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			yield return clearToil;
			yield break;
		}

		// Token: 0x040019D6 RID: 6614
		private float workDone;

		// Token: 0x040019D7 RID: 6615
		private const float ClearWorkPerSnowDepth = 50f;
	}
}
