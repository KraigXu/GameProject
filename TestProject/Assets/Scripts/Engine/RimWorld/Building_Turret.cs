using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C5C RID: 3164
	public abstract class Building_Turret : Building, IAttackTarget, ILoadReferenceable, IAttackTargetSearcher
	{
		// Token: 0x17000D44 RID: 3396
		// (get) Token: 0x06004B94 RID: 19348
		public abstract LocalTargetInfo CurrentTarget { get; }

		// Token: 0x17000D45 RID: 3397
		// (get) Token: 0x06004B95 RID: 19349
		public abstract Verb AttackVerb { get; }

		// Token: 0x17000D46 RID: 3398
		// (get) Token: 0x06004B96 RID: 19350 RVA: 0x0006461A File Offset: 0x0006281A
		Thing IAttackTarget.Thing
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000D47 RID: 3399
		// (get) Token: 0x06004B97 RID: 19351 RVA: 0x00197878 File Offset: 0x00195A78
		public LocalTargetInfo TargetCurrentlyAimingAt
		{
			get
			{
				return this.CurrentTarget;
			}
		}

		// Token: 0x17000D48 RID: 3400
		// (get) Token: 0x06004B98 RID: 19352 RVA: 0x0006461A File Offset: 0x0006281A
		Thing IAttackTargetSearcher.Thing
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000D49 RID: 3401
		// (get) Token: 0x06004B99 RID: 19353 RVA: 0x00197880 File Offset: 0x00195A80
		public Verb CurrentEffectiveVerb
		{
			get
			{
				return this.AttackVerb;
			}
		}

		// Token: 0x17000D4A RID: 3402
		// (get) Token: 0x06004B9A RID: 19354 RVA: 0x00197888 File Offset: 0x00195A88
		public LocalTargetInfo LastAttackedTarget
		{
			get
			{
				return this.lastAttackedTarget;
			}
		}

		// Token: 0x17000D4B RID: 3403
		// (get) Token: 0x06004B9B RID: 19355 RVA: 0x00197890 File Offset: 0x00195A90
		public int LastAttackTargetTick
		{
			get
			{
				return this.lastAttackTargetTick;
			}
		}

		// Token: 0x17000D4C RID: 3404
		// (get) Token: 0x06004B9C RID: 19356 RVA: 0x0001BFCE File Offset: 0x0001A1CE
		public float TargetPriorityFactor
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x06004B9D RID: 19357 RVA: 0x00197898 File Offset: 0x00195A98
		public Building_Turret()
		{
			this.stunner = new StunHandler(this);
		}

		// Token: 0x06004B9E RID: 19358 RVA: 0x001978B8 File Offset: 0x00195AB8
		public override void Tick()
		{
			base.Tick();
			if (this.forcedTarget.HasThing && (!this.forcedTarget.Thing.Spawned || !base.Spawned || this.forcedTarget.Thing.Map != base.Map))
			{
				this.forcedTarget = LocalTargetInfo.Invalid;
			}
			this.stunner.StunHandlerTick();
		}

		// Token: 0x06004B9F RID: 19359 RVA: 0x00197920 File Offset: 0x00195B20
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_TargetInfo.Look(ref this.forcedTarget, "forcedTarget");
			Scribe_TargetInfo.Look(ref this.lastAttackedTarget, "lastAttackedTarget");
			Scribe_Deep.Look<StunHandler>(ref this.stunner, "stunner", new object[]
			{
				this
			});
			Scribe_Values.Look<int>(ref this.lastAttackTargetTick, "lastAttackTargetTick", 0, false);
		}

		// Token: 0x06004BA0 RID: 19360 RVA: 0x0019797F File Offset: 0x00195B7F
		public override void PreApplyDamage(ref DamageInfo dinfo, out bool absorbed)
		{
			base.PreApplyDamage(ref dinfo, out absorbed);
			if (absorbed)
			{
				return;
			}
			this.stunner.Notify_DamageApplied(dinfo, true);
			absorbed = false;
		}

		// Token: 0x06004BA1 RID: 19361
		public abstract void OrderAttack(LocalTargetInfo targ);

		// Token: 0x06004BA2 RID: 19362 RVA: 0x001979A4 File Offset: 0x00195BA4
		public bool ThreatDisabled(IAttackTargetSearcher disabledFor)
		{
			CompPowerTrader comp = base.GetComp<CompPowerTrader>();
			if (comp != null && !comp.PowerOn)
			{
				return true;
			}
			CompMannable comp2 = base.GetComp<CompMannable>();
			if (comp2 != null && !comp2.MannedNow)
			{
				return true;
			}
			CompCanBeDormant comp3 = base.GetComp<CompCanBeDormant>();
			if (comp3 != null && !comp3.Awake)
			{
				return true;
			}
			CompInitiatable comp4 = base.GetComp<CompInitiatable>();
			return comp4 != null && !comp4.Initiated;
		}

		// Token: 0x06004BA3 RID: 19363 RVA: 0x00197A02 File Offset: 0x00195C02
		protected void OnAttackedTarget(LocalTargetInfo target)
		{
			this.lastAttackTargetTick = Find.TickManager.TicksGame;
			this.lastAttackedTarget = target;
		}

		// Token: 0x04002AB8 RID: 10936
		protected StunHandler stunner;

		// Token: 0x04002AB9 RID: 10937
		protected LocalTargetInfo forcedTarget = LocalTargetInfo.Invalid;

		// Token: 0x04002ABA RID: 10938
		private LocalTargetInfo lastAttackedTarget;

		// Token: 0x04002ABB RID: 10939
		private int lastAttackTargetTick;

		// Token: 0x04002ABC RID: 10940
		private const float SightRadiusTurret = 13.4f;
	}
}
