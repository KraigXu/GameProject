using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	
	public class JobDriver_FleeAndCower : JobDriver_Flee
	{
		
		public override string GetReport()
		{
			if (this.pawn.CurJob != this.job || this.pawn.Position != this.job.GetTarget(TargetIndex.A).Cell)
			{
				return base.GetReport();
			}
			return "ReportCowering".Translate();
		}

		
		protected override IEnumerable<Toil> MakeNewToils()
		{
			foreach (Toil toil in this.n__0())
			{
				yield return toil;
			}
			IEnumerator<Toil> enumerator = null;
			yield return new Toil
			{
				defaultCompleteMode = ToilCompleteMode.Delay,
				defaultDuration = 1200,
				tickAction = delegate
				{
					if (this.pawn.IsHashIntervalTick(35) && SelfDefenseUtility.ShouldStartFleeing(this.pawn))
					{
						base.EndJobWith(JobCondition.InterruptForced);
					}
				}
			};
			yield break;
			yield break;
		}

		
		private const int CowerTicks = 1200;

		
		private const int CheckFleeAgainIntervalTicks = 35;
	}
}
