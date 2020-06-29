using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	
	public class CompCanBeDormant : ThingComp
	{
		
		// (get) Token: 0x060050BB RID: 20667 RVA: 0x001B1D1A File Offset: 0x001AFF1A
		private CompProperties_CanBeDormant Props
		{
			get
			{
				return (CompProperties_CanBeDormant)this.props;
			}
		}

		
		// (get) Token: 0x060050BC RID: 20668 RVA: 0x001B1D27 File Offset: 0x001AFF27
		private bool WaitingToWakeUp
		{
			get
			{
				return this.wakeUpOnTick != int.MinValue;
			}
		}

		
		// (get) Token: 0x060050BD RID: 20669 RVA: 0x001B1D39 File Offset: 0x001AFF39
		public bool Awake
		{
			get
			{
				return this.wokeUpTick != int.MinValue && this.wokeUpTick <= Find.TickManager.TicksGame;
			}
		}

		
		public override void PostPostMake()
		{
			base.PostPostMake();
			this.makeTick = GenTicks.TicksGame;
			if (!this.Props.startsDormant)
			{
				this.WakeUp();
			}
		}

		
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.wakeUpSignalTag = this.Props.wakeUpSignalTag;
		}

		
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

		
		public void WakeUpWithDelay()
		{
			if (!this.Awake)
			{
				this.wakeUpOnTick = Find.TickManager.TicksGame + Rand.Range(60, 300);
			}
		}

		
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

		
		public override void CompTickRare()
		{
			base.CompTickRare();
			if (this.wakeUpOnTick != -2147483648 && Find.TickManager.TicksGame >= this.wakeUpOnTick)
			{
				this.WakeUp();
			}
			this.TickRareWorker();
		}

		
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

		
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.wokeUpTick, "wokeUpTick", int.MinValue, false);
			Scribe_Values.Look<int>(ref this.wakeUpOnTick, "wakeUpOnTick", int.MinValue, false);
			Scribe_Values.Look<string>(ref this.wakeUpSignalTag, "wakeUpSignalTag", null, false);
			Scribe_Collections.Look<string>(ref this.wakeUpSignalTags, "wakeUpSignalTags", LookMode.Value, Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.makeTick, "makeTick", 0, false);
		}

		
		public int makeTick;

		
		public int wokeUpTick = int.MinValue;

		
		public int wakeUpOnTick = int.MinValue;

		
		public string wakeUpSignalTag;

		
		public List<string> wakeUpSignalTags;

		
		public const string DefaultWakeUpSignal = "CompCanBeDormant.WakeUp";
	}
}
