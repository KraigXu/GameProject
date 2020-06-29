using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	
	public class JobDriver_ExtinguishSelf : JobDriver
	{
		
		// (get) Token: 0x06002BAC RID: 11180 RVA: 0x000FB804 File Offset: 0x000F9A04
		protected Fire TargetFire
		{
			get
			{
				return (Fire)this.job.targetA.Thing;
			}
		}

		
		public override string GetReport()
		{
			if (this.TargetFire != null && this.TargetFire.parent != null)
			{
				return "ReportExtinguishingFireOn".Translate(this.TargetFire.parent.LabelCap, this.TargetFire.parent.Named("TARGET"));
			}
			return "ReportExtinguishingFire".Translate();
		}

		
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		
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
