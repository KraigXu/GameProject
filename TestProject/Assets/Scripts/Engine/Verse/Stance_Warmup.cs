using System;
using RimWorld;
using Verse.Sound;

namespace Verse
{
	// Token: 0x020002A5 RID: 677
	public class Stance_Warmup : Stance_Busy
	{
		// Token: 0x0600136B RID: 4971 RVA: 0x0006F8BF File Offset: 0x0006DABF
		public Stance_Warmup()
		{
		}

		// Token: 0x0600136C RID: 4972 RVA: 0x0006F8C8 File Offset: 0x0006DAC8
		public Stance_Warmup(int ticks, LocalTargetInfo focusTarg, Verb verb) : base(ticks, focusTarg, verb)
		{
			if (focusTarg.HasThing && focusTarg.Thing is Pawn)
			{
				Pawn pawn = (Pawn)focusTarg.Thing;
				this.targetStartedDowned = pawn.Downed;
				if (pawn.apparel != null)
				{
					for (int i = 0; i < pawn.apparel.WornApparelCount; i++)
					{
						ShieldBelt shieldBelt = pawn.apparel.WornApparel[i] as ShieldBelt;
						if (shieldBelt != null)
						{
							shieldBelt.KeepDisplaying();
						}
					}
				}
			}
			if (verb != null && verb.verbProps.soundAiming != null)
			{
				SoundInfo info = SoundInfo.InMap(verb.caster, MaintenanceType.PerTick);
				if (verb.CasterIsPawn)
				{
					info.pitchFactor = 1f / verb.CasterPawn.GetStatValue(StatDefOf.AimingDelayFactor, true);
				}
				this.sustainer = verb.verbProps.soundAiming.TrySpawnSustainer(info);
			}
		}

		// Token: 0x0600136D RID: 4973 RVA: 0x0006F9AB File Offset: 0x0006DBAB
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.targetStartedDowned, "targetStartDowned", false, false);
		}

		// Token: 0x0600136E RID: 4974 RVA: 0x0006F9C8 File Offset: 0x0006DBC8
		public override void StanceDraw()
		{
			if (Find.Selector.IsSelected(this.stanceTracker.pawn))
			{
				GenDraw.DrawAimPie(this.stanceTracker.pawn, this.focusTarg, (int)((float)this.ticksLeft * this.pieSizeFactor), 0.2f);
			}
		}

		// Token: 0x0600136F RID: 4975 RVA: 0x0006FA18 File Offset: 0x0006DC18
		public override void StanceTick()
		{
			if (this.sustainer != null && !this.sustainer.Ended)
			{
				this.sustainer.Maintain();
			}
			if (!this.targetStartedDowned && this.focusTarg.HasThing && this.focusTarg.Thing is Pawn && ((Pawn)this.focusTarg.Thing).Downed)
			{
				this.stanceTracker.SetStance(new Stance_Mobile());
				return;
			}
			if (this.focusTarg.HasThing && (!this.focusTarg.Thing.Spawned || this.verb == null || !this.verb.CanHitTargetFrom(base.Pawn.Position, this.focusTarg)))
			{
				this.stanceTracker.SetStance(new Stance_Mobile());
				return;
			}
			if (this.focusTarg == base.Pawn.mindState.enemyTarget)
			{
				base.Pawn.mindState.Notify_EngagedTarget();
			}
			base.StanceTick();
		}

		// Token: 0x06001370 RID: 4976 RVA: 0x0006FB22 File Offset: 0x0006DD22
		public void Interrupt()
		{
			base.Expire();
		}

		// Token: 0x06001371 RID: 4977 RVA: 0x0006FB2A File Offset: 0x0006DD2A
		protected override void Expire()
		{
			this.verb.WarmupComplete();
			base.Expire();
		}

		// Token: 0x04000D1D RID: 3357
		private Sustainer sustainer;

		// Token: 0x04000D1E RID: 3358
		private bool targetStartedDowned;
	}
}
