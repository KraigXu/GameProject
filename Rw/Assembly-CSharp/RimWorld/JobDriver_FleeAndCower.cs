using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200065D RID: 1629
	public class JobDriver_FleeAndCower : JobDriver_Flee
	{
		// Token: 0x06002C6B RID: 11371 RVA: 0x000FD73C File Offset: 0x000FB93C
		public override string GetReport()
		{
			if (this.pawn.CurJob != this.job || this.pawn.Position != this.job.GetTarget(TargetIndex.A).Cell)
			{
				return base.GetReport();
			}
			return "ReportCowering".Translate();
		}

		// Token: 0x06002C6C RID: 11372 RVA: 0x000FD798 File Offset: 0x000FB998
		protected override IEnumerable<Toil> MakeNewToils()
		{
			foreach (Toil toil in this.<>n__0())
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

		// Token: 0x040019DE RID: 6622
		private const int CowerTicks = 1200;

		// Token: 0x040019DF RID: 6623
		private const int CheckFleeAgainIntervalTicks = 35;
	}
}
