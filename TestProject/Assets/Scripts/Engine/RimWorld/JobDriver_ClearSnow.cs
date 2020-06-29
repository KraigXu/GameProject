using System;
using System.Collections.Generic;
using Verse.AI;

namespace RimWorld
{
	
	public class JobDriver_ClearSnow : JobDriver
	{
		
		
		private float TotalNeededWork
		{
			get
			{
				return 50f * base.Map.snowGrid.GetDepth(base.TargetLocA);
			}
		}

		
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed);
		}

		
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

		
		private float workDone;

		
		private const float ClearWorkPerSnowDepth = 50f;
	}
}
