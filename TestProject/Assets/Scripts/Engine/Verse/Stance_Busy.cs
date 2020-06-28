using System;

namespace Verse
{
	// Token: 0x020002A4 RID: 676
	public abstract class Stance_Busy : Stance
	{
		// Token: 0x1700041B RID: 1051
		// (get) Token: 0x06001363 RID: 4963 RVA: 0x0001028D File Offset: 0x0000E48D
		public override bool StanceBusy
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001364 RID: 4964 RVA: 0x0006F750 File Offset: 0x0006D950
		public Stance_Busy()
		{
			this.SetPieSizeFactor();
		}

		// Token: 0x06001365 RID: 4965 RVA: 0x0006F769 File Offset: 0x0006D969
		public Stance_Busy(int ticks, LocalTargetInfo focusTarg, Verb verb)
		{
			this.ticksLeft = ticks;
			this.focusTarg = focusTarg;
			this.verb = verb;
		}

		// Token: 0x06001366 RID: 4966 RVA: 0x0006F791 File Offset: 0x0006D991
		public Stance_Busy(int ticks) : this(ticks, null, null)
		{
		}

		// Token: 0x06001367 RID: 4967 RVA: 0x0006F7A1 File Offset: 0x0006D9A1
		private void SetPieSizeFactor()
		{
			if (this.ticksLeft < 300)
			{
				this.pieSizeFactor = 1f;
				return;
			}
			if (this.ticksLeft < 450)
			{
				this.pieSizeFactor = 0.75f;
				return;
			}
			this.pieSizeFactor = 0.5f;
		}

		// Token: 0x06001368 RID: 4968 RVA: 0x0006F7E0 File Offset: 0x0006D9E0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksLeft, "ticksLeft", 0, false);
			Scribe_TargetInfo.Look(ref this.focusTarg, "focusTarg");
			Scribe_Values.Look<bool>(ref this.neverAimWeapon, "neverAimWeapon", false, false);
			Scribe_References.Look<Verb>(ref this.verb, "verb", false);
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.SetPieSizeFactor();
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.verb != null && this.verb.BuggedAfterLoading)
			{
				this.verb = null;
				Log.Warning(base.GetType() + " had a bugged verb after loading.", false);
			}
		}

		// Token: 0x06001369 RID: 4969 RVA: 0x0006F880 File Offset: 0x0006DA80
		public override void StanceTick()
		{
			this.ticksLeft--;
			if (this.ticksLeft <= 0)
			{
				this.Expire();
			}
		}

		// Token: 0x0600136A RID: 4970 RVA: 0x0006F89F File Offset: 0x0006DA9F
		protected virtual void Expire()
		{
			if (this.stanceTracker.curStance == this)
			{
				this.stanceTracker.SetStance(new Stance_Mobile());
			}
		}

		// Token: 0x04000D18 RID: 3352
		public int ticksLeft;

		// Token: 0x04000D19 RID: 3353
		public Verb verb;

		// Token: 0x04000D1A RID: 3354
		public LocalTargetInfo focusTarg;

		// Token: 0x04000D1B RID: 3355
		public bool neverAimWeapon;

		// Token: 0x04000D1C RID: 3356
		protected float pieSizeFactor = 1f;
	}
}
