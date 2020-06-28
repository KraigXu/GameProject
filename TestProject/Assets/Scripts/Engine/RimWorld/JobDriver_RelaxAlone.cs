using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200064B RID: 1611
	public class JobDriver_RelaxAlone : JobDriver
	{
		// Token: 0x1700084C RID: 2124
		// (get) Token: 0x06002BFB RID: 11259 RVA: 0x000FC750 File Offset: 0x000FA950
		private bool FromBed
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing is Building_Bed;
			}
		}

		// Token: 0x1700084D RID: 2125
		// (get) Token: 0x06002BFC RID: 11260 RVA: 0x0001028D File Offset: 0x0000E48D
		protected virtual bool CanSleep
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002BFD RID: 11261 RVA: 0x000FC779 File Offset: 0x000FA979
		public override bool CanBeginNowWhileLyingDown()
		{
			return this.FromBed && JobInBedUtility.InBedOrRestSpotNow(this.pawn, this.job.GetTarget(TargetIndex.A));
		}

		// Token: 0x06002BFE RID: 11262 RVA: 0x000FC79C File Offset: 0x000FA99C
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			if (this.FromBed)
			{
				if (!this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, ((Building_Bed)this.job.GetTarget(TargetIndex.A).Thing).SleepingSlotsCount, 0, null, errorOnFailed))
				{
					return false;
				}
			}
			else if (!this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 1, -1, null, errorOnFailed))
			{
				return false;
			}
			return true;
		}

		// Token: 0x06002BFF RID: 11263 RVA: 0x000FC819 File Offset: 0x000FAA19
		protected override IEnumerable<Toil> MakeNewToils()
		{
			Toil toil;
			if (this.FromBed)
			{
				this.KeepLyingDown(TargetIndex.A);
				yield return Toils_Bed.ClaimBedIfNonMedical(TargetIndex.A, TargetIndex.None);
				yield return Toils_Bed.GotoBed(TargetIndex.A);
				toil = Toils_LayDown.LayDown(TargetIndex.A, true, false, this.CanSleep, true);
				toil.AddFailCondition(() => !this.pawn.Awake());
			}
			else
			{
				yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
				toil = new Toil();
				toil.initAction = delegate
				{
					this.faceDir = (this.job.def.faceDir.IsValid ? this.job.def.faceDir : Rot4.Random);
				};
				toil.handlingFacing = true;
			}
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			toil.defaultDuration = this.job.def.joyDuration;
			toil.AddPreTickAction(delegate
			{
				if (this.faceDir.IsValid)
				{
					this.pawn.rotationTracker.FaceCell(this.pawn.Position + this.faceDir.FacingCell);
				}
				this.pawn.GainComfortFromCellIfPossible(false);
				JoyUtility.JoyTickCheckEnd(this.pawn, JoyTickFullJoyAction.EndJob, 1f, null);
			});
			yield return toil;
			yield break;
		}

		// Token: 0x06002C00 RID: 11264 RVA: 0x000FC82C File Offset: 0x000FAA2C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<Rot4>(ref this.faceDir, "faceDir", default(Rot4), false);
		}

		// Token: 0x040019C4 RID: 6596
		protected Rot4 faceDir = Rot4.Invalid;

		// Token: 0x040019C5 RID: 6597
		protected const TargetIndex SpotOrBedInd = TargetIndex.A;
	}
}
