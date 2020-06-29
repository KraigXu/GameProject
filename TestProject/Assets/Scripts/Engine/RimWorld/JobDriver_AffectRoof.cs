using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	
	public abstract class JobDriver_AffectRoof : JobDriver
	{
		
		// (get) Token: 0x06002B5B RID: 11099 RVA: 0x000FB03C File Offset: 0x000F923C
		protected IntVec3 Cell
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Cell;
			}
		}

		
		// (get) Token: 0x06002B5C RID: 11100
		protected abstract PathEndMode PathEndMode { get; }

		
		protected abstract void DoEffect();

		
		protected abstract bool DoWorkFailOn();

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.workLeft, "workLeft", 0f, false);
		}

		
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Cell, this.job, 1, -1, ReservationLayerDefOf.Ceiling, errorOnFailed);
		}

		
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

		
		private float workLeft;

		
		private const TargetIndex CellInd = TargetIndex.A;

		
		private const TargetIndex GotoTargetInd = TargetIndex.B;

		
		private const float BaseWorkAmount = 65f;
	}
}
