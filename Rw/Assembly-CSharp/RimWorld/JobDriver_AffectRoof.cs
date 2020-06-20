using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200062F RID: 1583
	public abstract class JobDriver_AffectRoof : JobDriver
	{
		// Token: 0x17000833 RID: 2099
		// (get) Token: 0x06002B5B RID: 11099 RVA: 0x000FB03C File Offset: 0x000F923C
		protected IntVec3 Cell
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Cell;
			}
		}

		// Token: 0x17000834 RID: 2100
		// (get) Token: 0x06002B5C RID: 11100
		protected abstract PathEndMode PathEndMode { get; }

		// Token: 0x06002B5D RID: 11101
		protected abstract void DoEffect();

		// Token: 0x06002B5E RID: 11102
		protected abstract bool DoWorkFailOn();

		// Token: 0x06002B5F RID: 11103 RVA: 0x000FB05D File Offset: 0x000F925D
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.workLeft, "workLeft", 0f, false);
		}

		// Token: 0x06002B60 RID: 11104 RVA: 0x000FB07B File Offset: 0x000F927B
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Cell, this.job, 1, -1, ReservationLayerDefOf.Ceiling, errorOnFailed);
		}

		// Token: 0x06002B61 RID: 11105 RVA: 0x000FB0A1 File Offset: 0x000F92A1
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.B);
			yield return Toils_Goto.Goto(TargetIndex.B, this.PathEndMode);
			Toil doWork = new Toil();
			doWork.initAction = delegate
			{
				this.workLeft = 65f;
			};
			doWork.tickAction = delegate
			{
				float num = doWork.actor.GetStatValue(StatDefOf.ConstructionSpeed, true) * 1.7f;
				this.workLeft -= num;
				if (this.workLeft <= 0f)
				{
					this.DoEffect();
					this.ReadyForNextToil();
					return;
				}
			};
			doWork.FailOnCannotTouch(TargetIndex.B, this.PathEndMode);
			doWork.PlaySoundAtStart(SoundDefOf.Roof_Start);
			doWork.PlaySoundAtEnd(SoundDefOf.Roof_Finish);
			doWork.WithEffect(EffecterDefOf.RoofWork, TargetIndex.A);
			doWork.FailOn(new Func<bool>(this.DoWorkFailOn));
			doWork.WithProgressBar(TargetIndex.A, () => 1f - this.workLeft / 65f, false, -0.5f);
			doWork.defaultCompleteMode = ToilCompleteMode.Never;
			yield return doWork;
			yield break;
		}

		// Token: 0x04001998 RID: 6552
		private float workLeft;

		// Token: 0x04001999 RID: 6553
		private const TargetIndex CellInd = TargetIndex.A;

		// Token: 0x0400199A RID: 6554
		private const TargetIndex GotoTargetInd = TargetIndex.B;

		// Token: 0x0400199B RID: 6555
		private const float BaseWorkAmount = 65f;
	}
}
