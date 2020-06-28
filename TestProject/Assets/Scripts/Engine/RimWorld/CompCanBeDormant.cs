using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000CFB RID: 3323
	public class CompCanBeDormant : ThingComp
	{
		// Token: 0x17000E29 RID: 3625
		// (get) Token: 0x060050BB RID: 20667 RVA: 0x001B1D1A File Offset: 0x001AFF1A
		private CompProperties_CanBeDormant Props
		{
			get
			{
				return (CompProperties_CanBeDormant)this.props;
			}
		}

		// Token: 0x17000E2A RID: 3626
		// (get) Token: 0x060050BC RID: 20668 RVA: 0x001B1D27 File Offset: 0x001AFF27
		private bool WaitingToWakeUp
		{
			get
			{
				return this.wakeUpOnTick != int.MinValue;
			}
		}

		// Token: 0x17000E2B RID: 3627
		// (get) Token: 0x060050BD RID: 20669 RVA: 0x001B1D39 File Offset: 0x001AFF39
		public bool Awake
		{
			get
			{
				return this.wokeUpTick != int.MinValue && this.wokeUpTick <= Find.TickManager.TicksGame;
			}
		}

		// Token: 0x060050BE RID: 20670 RVA: 0x001B1D5F File Offset: 0x001AFF5F
		public override void PostPostMake()
		{
			base.PostPostMake();
			this.makeTick = GenTicks.TicksGame;
			if (!this.Props.startsDormant)
			{
				this.WakeUp();
			}
		}

		// Token: 0x060050BF RID: 20671 RVA: 0x001B1D85 File Offset: 0x001AFF85
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.wakeUpSignalTag = this.Props.wakeUpSignalTag;
		}

		// Token: 0x060050C0 RID: 20672 RVA: 0x001B1D9F File Offset: 0x001AFF9F
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (!Prefs.DevMode)
			{
				yield break;
			}
			yield return new Command_Action
			{
				defaultLabel = "DEV: Wake Up",
				action = delegate
				{
					this.WakeUp();
				}
			};
			yield break;
		}

		// Token: 0x060050C1 RID: 20673 RVA: 0x001B1DB0 File Offset: 0x001AFFB0
		public override string CompInspectStringExtra()
		{
			if (!this.Awake)
			{
				return this.Props.dormantStateLabelKey.Translate();
			}
			if (this.makeTick != this.wokeUpTick)
			{
				return this.Props.awakeStateLabelKey.Translate((GenTicks.TicksGame - this.wokeUpTick).TicksToDays().ToString("0.#"));
			}
			return null;
		}

		// Token: 0x060050C2 RID: 20674 RVA: 0x001B1E23 File Offset: 0x001B0023
		public void WakeUpWithDelay()
		{
			if (!this.Awake)
			{
				this.wakeUpOnTick = Find.TickManager.TicksGame + Rand.Range(60, 300);
			}
		}

		// Token: 0x060050C3 RID: 20675 RVA: 0x001B1E4C File Offset: 0x001B004C
		public void WakeUp()
		{
			if (this.Awake)
			{
				return;
			}
			this.wokeUpTick = GenTicks.TicksGame;
			this.wakeUpOnTick = int.MinValue;
			Pawn pawn = this.parent as Pawn;
			Building building = this.parent as Building;
			Lord lord = ((pawn != null) ? pawn.GetLord() : null) ?? ((building != null) ? building.GetLord() : null);
			if (lord != null)
			{
				lord.Notify_DormancyWakeup();
			}
			if (this.parent.Spawned)
			{
				IAttackTarget attackTarget = this.parent as IAttackTarget;
				if (attackTarget != null)
				{
					this.parent.Map.attackTargetsCache.UpdateTarget(attackTarget);
				}
			}
		}

		// Token: 0x060050C4 RID: 20676 RVA: 0x001B1EE8 File Offset: 0x001B00E8
		public void ToSleep()
		{
			if (!this.Awake)
			{
				return;
			}
			this.wokeUpTick = int.MinValue;
			if (this.parent.Spawned)
			{
				IAttackTarget attackTarget = this.parent as IAttackTarget;
				if (attackTarget != null)
				{
					this.parent.Map.attackTargetsCache.UpdateTarget(attackTarget);
				}
			}
		}

		// Token: 0x060050C5 RID: 20677 RVA: 0x001B1F3B File Offset: 0x001B013B
		public override void CompTickRare()
		{
			base.CompTickRare();
			if (this.wakeUpOnTick != -2147483648 && Find.TickManager.TicksGame >= this.wakeUpOnTick)
			{
				this.WakeUp();
			}
			this.TickRareWorker();
		}

		// Token: 0x060050C6 RID: 20678 RVA: 0x001B1F70 File Offset: 0x001B0170
		public override void CompTick()
		{
			base.CompTick();
			if (this.wakeUpOnTick != -2147483648 && Find.TickManager.TicksGame >= this.wakeUpOnTick)
			{
				this.WakeUp();
			}
			if (this.parent.IsHashIntervalTick(250))
			{
				this.TickRareWorker();
			}
		}

		// Token: 0x060050C7 RID: 20679 RVA: 0x001B1FC0 File Offset: 0x001B01C0
		public void TickRareWorker()
		{
			if (!this.parent.Spawned || this.Awake)
			{
				return;
			}
			if (!(this.parent is Pawn) && !this.parent.Position.Fogged(this.parent.Map))
			{
				MoteMaker.ThrowMetaIcon(this.parent.Position, this.parent.Map, ThingDefOf.Mote_SleepZ);
			}
		}

		// Token: 0x060050C8 RID: 20680 RVA: 0x001B2030 File Offset: 0x001B0230
		public override void Notify_SignalReceived(Signal signal)
		{
			if (string.IsNullOrEmpty(this.wakeUpSignalTag) || this.Awake)
			{
				return;
			}
			Thing thing;
			Faction faction;
			if ((signal.tag == this.wakeUpSignalTag || (this.wakeUpSignalTags != null && this.wakeUpSignalTags.Contains(signal.tag))) && signal.args.TryGetArg<Thing>("SUBJECT", out thing) && thing != this.parent && thing != null && thing.Map == this.parent.Map && this.parent.Position.DistanceTo(thing.Position) <= this.Props.maxDistAwakenByOther && (!signal.args.TryGetArg<Faction>("FACTION", out faction) || faction == null || faction == this.parent.Faction) && (this.Props.canWakeUpFogged || !this.parent.Fogged()) && !this.WaitingToWakeUp)
			{
				this.WakeUpWithDelay();
			}
		}

		// Token: 0x060050C9 RID: 20681 RVA: 0x001B2138 File Offset: 0x001B0338
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.wokeUpTick, "wokeUpTick", int.MinValue, false);
			Scribe_Values.Look<int>(ref this.wakeUpOnTick, "wakeUpOnTick", int.MinValue, false);
			Scribe_Values.Look<string>(ref this.wakeUpSignalTag, "wakeUpSignalTag", null, false);
			Scribe_Collections.Look<string>(ref this.wakeUpSignalTags, "wakeUpSignalTags", LookMode.Value, Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.makeTick, "makeTick", 0, false);
		}

		// Token: 0x04002CDA RID: 11482
		public int makeTick;

		// Token: 0x04002CDB RID: 11483
		public int wokeUpTick = int.MinValue;

		// Token: 0x04002CDC RID: 11484
		public int wakeUpOnTick = int.MinValue;

		// Token: 0x04002CDD RID: 11485
		public string wakeUpSignalTag;

		// Token: 0x04002CDE RID: 11486
		public List<string> wakeUpSignalTags;

		// Token: 0x04002CDF RID: 11487
		public const string DefaultWakeUpSignal = "CompCanBeDormant.WakeUp";
	}
}
