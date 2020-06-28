using System;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000D4F RID: 3407
	[StaticConstructorOnStartup]
	public class CompSendSignalOnPawnProximity : ThingComp
	{
		// Token: 0x17000EAB RID: 3755
		// (get) Token: 0x060052D4 RID: 21204 RVA: 0x001BAA28 File Offset: 0x001B8C28
		public CompProperties_SendSignalOnPawnProximity Props
		{
			get
			{
				return (CompProperties_SendSignalOnPawnProximity)this.props;
			}
		}

		// Token: 0x17000EAC RID: 3756
		// (get) Token: 0x060052D5 RID: 21205 RVA: 0x001BAA35 File Offset: 0x001B8C35
		public bool Sent
		{
			get
			{
				return this.sent;
			}
		}

		// Token: 0x17000EAD RID: 3757
		// (get) Token: 0x060052D6 RID: 21206 RVA: 0x001BAA3D File Offset: 0x001B8C3D
		public bool Enabled
		{
			get
			{
				return this.ticksUntilEnabled <= 0;
			}
		}

		// Token: 0x060052D7 RID: 21207 RVA: 0x001BAA4B File Offset: 0x001B8C4B
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.signalTag = this.Props.signalTag;
			this.ticksUntilEnabled = this.Props.enableAfterTicks;
		}

		// Token: 0x060052D8 RID: 21208 RVA: 0x001BAA78 File Offset: 0x001B8C78
		public override void CompTick()
		{
			base.CompTick();
			if (this.sent || !this.parent.Spawned)
			{
				return;
			}
			if (this.Enabled && Find.TickManager.TicksGame % 250 == 0)
			{
				this.CompTickRare();
			}
			if (this.ticksUntilEnabled > 0)
			{
				this.ticksUntilEnabled--;
			}
		}

		// Token: 0x060052D9 RID: 21209 RVA: 0x001BAAD8 File Offset: 0x001B8CD8
		public override void CompTickRare()
		{
			base.CompTickRare();
			Predicate<Thing> predicate = null;
			if (this.Props.onlyHumanlike)
			{
				predicate = delegate(Thing t)
				{
					Pawn pawn = t as Pawn;
					return pawn != null && pawn.RaceProps.Humanlike;
				};
			}
			Thing thing = null;
			if (this.Props.triggerOnPawnInRoom)
			{
				foreach (Thing thing2 in this.parent.GetRoom(RegionType.Set_Passable).ContainedAndAdjacentThings)
				{
					if (predicate(thing2))
					{
						thing = thing2;
						break;
					}
				}
			}
			if (thing == null && this.Props.radius > 0f)
			{
				thing = GenClosest.ClosestThingReachable(this.parent.Position, this.parent.Map, ThingRequest.ForGroup(ThingRequestGroup.Pawn), PathEndMode.OnCell, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false), this.Props.radius, predicate, null, 0, -1, false, RegionType.Set_Passable, false);
			}
			if (thing != null)
			{
				Effecter effecter = new Effecter(EffecterDefOf.ActivatorProximityTriggered);
				effecter.Trigger(this.parent, TargetInfo.Invalid);
				effecter.Cleanup();
				Messages.Message("MessageActivatorProximityTriggered".Translate(thing), this.parent, MessageTypeDefOf.ThreatBig, true);
				Find.SignalManager.SendSignal(new Signal(this.signalTag, this.parent.Named("SUBJECT")));
				SoundDefOf.MechanoidsWakeUp.PlayOneShot(new TargetInfo(this.parent.Position, this.parent.Map, false));
				this.sent = true;
			}
		}

		// Token: 0x060052DA RID: 21210 RVA: 0x001BAC80 File Offset: 0x001B8E80
		public void Expire()
		{
			this.sent = true;
		}

		// Token: 0x060052DB RID: 21211 RVA: 0x001BAC8C File Offset: 0x001B8E8C
		public override void Notify_SignalReceived(Signal signal)
		{
			Thing thing;
			if (signal.tag == "CompCanBeDormant.WakeUp" && signal.args.TryGetArg<Thing>("SUBJECT", out thing) && thing != this.parent && thing != null && thing.Map == this.parent.Map && this.parent.Position.DistanceTo(thing.Position) <= 40f)
			{
				this.sent = true;
			}
		}

		// Token: 0x060052DC RID: 21212 RVA: 0x001BAD04 File Offset: 0x001B8F04
		public override string CompInspectStringExtra()
		{
			if (!this.Enabled)
			{
				return "SendSignalOnCountdownCompTime".Translate(this.ticksUntilEnabled.TicksToSeconds().ToString("0.0"));
			}
			if (!this.sent)
			{
				return "radius".Translate().CapitalizeFirst() + ": " + this.Props.radius.ToString("F0");
			}
			return "expired".Translate().CapitalizeFirst();
		}

		// Token: 0x060052DD RID: 21213 RVA: 0x001BADA1 File Offset: 0x001B8FA1
		public override void PostExposeData()
		{
			Scribe_Values.Look<string>(ref this.signalTag, "signalTag", null, false);
			Scribe_Values.Look<bool>(ref this.sent, "sent", false, false);
			Scribe_Values.Look<int>(ref this.ticksUntilEnabled, "ticksUntilEnabled", 0, false);
		}

		// Token: 0x04002DBB RID: 11707
		public string signalTag;

		// Token: 0x04002DBC RID: 11708
		private bool sent;

		// Token: 0x04002DBD RID: 11709
		private int ticksUntilEnabled;

		// Token: 0x04002DBE RID: 11710
		private const float MaxDistActivationByOther = 40f;
	}
}
