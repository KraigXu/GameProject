using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	
	public abstract class Building_Turret : Building, IAttackTarget, ILoadReferenceable, IAttackTargetSearcher
	{
		
		// (get) Token: 0x06004B94 RID: 19348
		public abstract LocalTargetInfo CurrentTarget { get; }

		
		// (get) Token: 0x06004B95 RID: 19349
		public abstract Verb AttackVerb { get; }

		
		// (get) Token: 0x06004B96 RID: 19350 RVA: 0x0006461A File Offset: 0x0006281A
		Thing IAttackTarget.Thing
		{
			get
			{
				return this;
			}
		}

		
		// (get) Token: 0x06004B97 RID: 19351 RVA: 0x00197878 File Offset: 0x00195A78
		public LocalTargetInfo TargetCurrentlyAimingAt
		{
			get
			{
				return this.CurrentTarget;
			}
		}

		
		// (get) Token: 0x06004B98 RID: 19352 RVA: 0x0006461A File Offset: 0x0006281A
		Thing IAttackTargetSearcher.Thing
		{
			get
			{
				return this;
			}
		}

		
		// (get) Token: 0x06004B99 RID: 19353 RVA: 0x00197880 File Offset: 0x00195A80
		public Verb CurrentEffectiveVerb
		{
			get
			{
				return this.AttackVerb;
			}
		}

		
		// (get) Token: 0x06004B9A RID: 19354 RVA: 0x00197888 File Offset: 0x00195A88
		public LocalTargetInfo LastAttackedTarget
		{
			get
			{
				return this.lastAttackedTarget;
			}
		}

		
		// (get) Token: 0x06004B9B RID: 19355 RVA: 0x00197890 File Offset: 0x00195A90
		public int LastAttackTargetTick
		{
			get
			{
				return this.lastAttackTargetTick;
			}
		}

		
		// (get) Token: 0x06004B9C RID: 19356 RVA: 0x0001BFCE File Offset: 0x0001A1CE
		public float TargetPriorityFactor
		{
			get
			{
				return 1f;
			}
		}

		
		public Building_Turret()
		{
			this.stunner = new StunHandler(this);
		}

		
		public override void Tick()
		{
			base.Tick();
			if (this.forcedTarget.HasThing && (!this.forcedTarget.Thing.Spawned || !base.Spawned || this.forcedTarget.Thing.Map != base.Map))
			{
				this.forcedTarget = LocalTargetInfo.Invalid;
			}
			this.stunner.StunHandlerTick();
		}

		
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

		
		public abstract void OrderAttack(LocalTargetInfo targ);

		
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

		
		protected void OnAttackedTarget(LocalTargetInfo target)
		{
			this.lastAttackTargetTick = Find.TickManager.TicksGame;
			this.lastAttackedTarget = target;
		}

		
		protected StunHandler stunner;

		
		protected LocalTargetInfo forcedTarget = LocalTargetInfo.Invalid;

		
		private LocalTargetInfo lastAttackedTarget;

		
		private int lastAttackTargetTick;

		
		private const float SightRadiusTurret = 13.4f;
	}
}
