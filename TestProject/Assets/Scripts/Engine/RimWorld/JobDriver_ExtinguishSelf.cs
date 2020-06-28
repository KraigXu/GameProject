using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200063C RID: 1596
	public class JobDriver_ExtinguishSelf : JobDriver
	{
		// Token: 0x17000845 RID: 2117
		// (get) Token: 0x06002BAC RID: 11180 RVA: 0x000FB804 File Offset: 0x000F9A04
		protected Fire TargetFire
		{
			get
			{
				return (Fire)this.job.targetA.Thing;
			}
		}

		// Token: 0x06002BAD RID: 11181 RVA: 0x000FB89C File Offset: 0x000F9A9C
		public override string GetReport()
		{
			if (this.TargetFire != null && this.TargetFire.parent != null)
			{
				return "ReportExtinguishingFireOn".Translate(this.TargetFire.parent.LabelCap, this.TargetFire.parent.Named("TARGET"));
			}
			return "ReportExtinguishingFire".Translate();
		}

		// Token: 0x06002BAE RID: 11182 RVA: 0x0001028D File Offset: 0x0000E48D
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06002BAF RID: 11183 RVA: 0x000FB907 File Offset: 0x000F9B07
		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return new Toil
			{
				defaultCompleteMode = ToilCompleteMode.Delay,
				defaultDuration = 150
			};
			Toil toil = new Toil();
			toil.initAction = delegate
			{
				this.TargetFire.Destroy(DestroyMode.Vanish);
				this.pawn.records.Increment(RecordDefOf.FiresExtinguished);
			};
			toil.FailOnDestroyedOrNull(TargetIndex.A);
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			yield return toil;
			yield break;
		}
	}
}
