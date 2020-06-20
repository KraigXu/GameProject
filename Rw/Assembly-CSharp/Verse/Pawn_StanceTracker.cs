using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020002A1 RID: 673
	public class Pawn_StanceTracker : IExposable
	{
		// Token: 0x17000417 RID: 1047
		// (get) Token: 0x06001351 RID: 4945 RVA: 0x0006F55F File Offset: 0x0006D75F
		public bool FullBodyBusy
		{
			get
			{
				return this.stunner.Stunned || this.curStance.StanceBusy;
			}
		}

		// Token: 0x17000418 RID: 1048
		// (get) Token: 0x06001352 RID: 4946 RVA: 0x0006F57B File Offset: 0x0006D77B
		public bool Staggered
		{
			get
			{
				return Find.TickManager.TicksGame < this.staggerUntilTick;
			}
		}

		// Token: 0x06001353 RID: 4947 RVA: 0x0006F58F File Offset: 0x0006D78F
		public Pawn_StanceTracker(Pawn newPawn)
		{
			this.pawn = newPawn;
			this.stunner = new StunHandler(this.pawn);
		}

		// Token: 0x06001354 RID: 4948 RVA: 0x0006F5C1 File Offset: 0x0006D7C1
		public void StanceTrackerTick()
		{
			this.stunner.StunHandlerTick();
			if (!this.stunner.Stunned)
			{
				this.curStance.StanceTick();
			}
		}

		// Token: 0x06001355 RID: 4949 RVA: 0x0006F5E6 File Offset: 0x0006D7E6
		public void StanceTrackerDraw()
		{
			this.curStance.StanceDraw();
		}

		// Token: 0x06001356 RID: 4950 RVA: 0x0006F5F4 File Offset: 0x0006D7F4
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.staggerUntilTick, "staggerUntilTick", 0, false);
			Scribe_Deep.Look<StunHandler>(ref this.stunner, "stunner", new object[]
			{
				this.pawn
			});
			Scribe_Deep.Look<Stance>(ref this.curStance, "curStance", Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.LoadingVars && this.curStance != null)
			{
				this.curStance.stanceTracker = this;
			}
		}

		// Token: 0x06001357 RID: 4951 RVA: 0x0006F663 File Offset: 0x0006D863
		public void StaggerFor(int ticks)
		{
			this.staggerUntilTick = Find.TickManager.TicksGame + ticks;
		}

		// Token: 0x06001358 RID: 4952 RVA: 0x0006F677 File Offset: 0x0006D877
		public void CancelBusyStanceSoft()
		{
			if (this.curStance is Stance_Warmup)
			{
				this.SetStance(new Stance_Mobile());
			}
		}

		// Token: 0x06001359 RID: 4953 RVA: 0x0006F691 File Offset: 0x0006D891
		public void CancelBusyStanceHard()
		{
			this.SetStance(new Stance_Mobile());
		}

		// Token: 0x0600135A RID: 4954 RVA: 0x0006F6A0 File Offset: 0x0006D8A0
		public void SetStance(Stance newStance)
		{
			if (this.debugLog)
			{
				Log.Message(string.Concat(new object[]
				{
					Find.TickManager.TicksGame,
					" ",
					this.pawn,
					" SetStance ",
					this.curStance,
					" -> ",
					newStance
				}), false);
			}
			newStance.stanceTracker = this;
			this.curStance = newStance;
			if (this.pawn.jobs.curDriver != null)
			{
				this.pawn.jobs.curDriver.Notify_StanceChanged();
			}
		}

		// Token: 0x0600135B RID: 4955 RVA: 0x00002681 File Offset: 0x00000881
		public void Notify_DamageTaken(DamageInfo dinfo)
		{
		}

		// Token: 0x04000D0F RID: 3343
		public Pawn pawn;

		// Token: 0x04000D10 RID: 3344
		public Stance curStance = new Stance_Mobile();

		// Token: 0x04000D11 RID: 3345
		private int staggerUntilTick = -1;

		// Token: 0x04000D12 RID: 3346
		public StunHandler stunner;

		// Token: 0x04000D13 RID: 3347
		public const int StaggerMeleeAttackTicks = 95;

		// Token: 0x04000D14 RID: 3348
		public const int StaggerBulletImpactTicks = 95;

		// Token: 0x04000D15 RID: 3349
		public const int StaggerExplosionImpactTicks = 95;

		// Token: 0x04000D16 RID: 3350
		public bool debugLog;
	}
}
