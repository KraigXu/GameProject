using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200068D RID: 1677
	public class JobDriver_LayDown : JobDriver
	{
		// Token: 0x1700088F RID: 2191
		// (get) Token: 0x06002D9D RID: 11677 RVA: 0x00100DCC File Offset: 0x000FEFCC
		public Building_Bed Bed
		{
			get
			{
				return (Building_Bed)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x06002D9E RID: 11678 RVA: 0x00100DF4 File Offset: 0x000FEFF4
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return !this.job.GetTarget(TargetIndex.A).HasThing || this.pawn.Reserve(this.Bed, this.job, this.Bed.SleepingSlotsCount, 0, null, errorOnFailed);
		}

		// Token: 0x06002D9F RID: 11679 RVA: 0x00100E46 File Offset: 0x000FF046
		public override bool CanBeginNowWhileLyingDown()
		{
			return JobInBedUtility.InBedOrRestSpotNow(this.pawn, this.job.GetTarget(TargetIndex.A));
		}

		// Token: 0x06002DA0 RID: 11680 RVA: 0x00100E5F File Offset: 0x000FF05F
		protected override IEnumerable<Toil> MakeNewToils()
		{
			bool hasBed = this.job.GetTarget(TargetIndex.A).HasThing;
			if (hasBed)
			{
				yield return Toils_Bed.ClaimBedIfNonMedical(TargetIndex.A, TargetIndex.None);
				yield return Toils_Bed.GotoBed(TargetIndex.A);
			}
			else
			{
				yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
			}
			yield return Toils_LayDown.LayDown(TargetIndex.A, hasBed, true, true, true);
			yield break;
		}

		// Token: 0x06002DA1 RID: 11681 RVA: 0x00100E6F File Offset: 0x000FF06F
		public override string GetReport()
		{
			if (this.asleep)
			{
				return "ReportSleeping".Translate();
			}
			return "ReportResting".Translate();
		}

		// Token: 0x04001A2F RID: 6703
		public const TargetIndex BedOrRestSpotIndex = TargetIndex.A;
	}
}
