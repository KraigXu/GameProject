using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x0200054C RID: 1356
	public class MentalState_MurderousRage : MentalState
	{
		// Token: 0x060026C8 RID: 9928 RVA: 0x000E41BD File Offset: 0x000E23BD
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.target, "target", false);
		}

		// Token: 0x060026C9 RID: 9929 RVA: 0x00010306 File Offset: 0x0000E506
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}

		// Token: 0x060026CA RID: 9930 RVA: 0x000E41D6 File Offset: 0x000E23D6
		public override void PreStart()
		{
			base.PreStart();
			this.TryFindNewTarget();
		}

		// Token: 0x060026CB RID: 9931 RVA: 0x000E41E8 File Offset: 0x000E23E8
		public override void MentalStateTick()
		{
			base.MentalStateTick();
			if (this.target != null && this.target.Dead)
			{
				base.RecoverFromState();
			}
			if (this.pawn.IsHashIntervalTick(120) && !this.IsTargetStillValidAndReachable())
			{
				if (!this.TryFindNewTarget())
				{
					base.RecoverFromState();
					return;
				}
				Messages.Message("MessageMurderousRageChangedTarget".Translate(this.pawn.LabelShort, this.target.Label, this.pawn.Named("PAWN"), this.target.Named("TARGET")).AdjustedFor(this.pawn, "PAWN", true), this.pawn, MessageTypeDefOf.NegativeEvent, true);
				base.MentalStateTick();
			}
		}

		// Token: 0x060026CC RID: 9932 RVA: 0x000E42C4 File Offset: 0x000E24C4
		public override string GetBeginLetterText()
		{
			if (this.target == null)
			{
				Log.Error("No target. This should have been checked in this mental state's worker.", false);
				return "";
			}
			return this.def.beginLetter.Formatted(this.pawn.NameShortColored.Resolve(), this.target.NameShortColored.Resolve(), this.pawn.Named("PAWN"), this.target.Named("TARGET")).AdjustedFor(this.pawn, "PAWN", true).CapitalizeFirst();
		}

		// Token: 0x060026CD RID: 9933 RVA: 0x000E436B File Offset: 0x000E256B
		private bool TryFindNewTarget()
		{
			this.target = MurderousRageMentalStateUtility.FindPawnToKill(this.pawn);
			return this.target != null;
		}

		// Token: 0x060026CE RID: 9934 RVA: 0x000E4388 File Offset: 0x000E2588
		public bool IsTargetStillValidAndReachable()
		{
			return this.target != null && this.target.SpawnedParentOrMe != null && (!(this.target.SpawnedParentOrMe is Pawn) || this.target.SpawnedParentOrMe == this.target) && this.pawn.CanReach(this.target.SpawnedParentOrMe, PathEndMode.Touch, Danger.Deadly, true, TraverseMode.ByPawn);
		}

		// Token: 0x04001744 RID: 5956
		public Pawn target;

		// Token: 0x04001745 RID: 5957
		private const int NoLongerValidTargetCheckInterval = 120;
	}
}
