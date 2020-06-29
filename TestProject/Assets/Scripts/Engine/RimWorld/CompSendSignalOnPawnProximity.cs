using System;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	
	[StaticConstructorOnStartup]
	public class CompSendSignalOnPawnProximity : ThingComp
	{
		
		
		public CompProperties_SendSignalOnPawnProximity Props
		{
			get
			{
				return (CompProperties_SendSignalOnPawnProximity)this.props;
			}
		}

		
		
		public bool Sent
		{
			get
			{
				return this.sent;
			}
		}

		
		
		public bool Enabled
		{
			get
			{
				return this.ticksUntilEnabled <= 0;
			}
		}

		
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.signalTag = this.Props.signalTag;
			this.ticksUntilEnabled = this.Props.enableAfterTicks;
		}

		
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

		
		public void Expire()
		{
			this.sent = true;
		}

		
		//public override void Notify_SignalReceived(Signal signal)
		//{
		//	Thing thing;
		//	if (signal.tag == "CompCanBeDormant.WakeUp" && signal.args.TryGetArg<Thing>("SUBJECT", out thing) && thing != this.parent && thing != null && thing.Map == this.parent.Map && this.parent.Position.DistanceTo(thing.Position) <= 40f)
		//	{
		//		this.sent = true;
		//	}
		//}

		
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

		
		public override void PostExposeData()
		{
			Scribe_Values.Look<string>(ref this.signalTag, "signalTag", null, false);
			Scribe_Values.Look<bool>(ref this.sent, "sent", false, false);
			Scribe_Values.Look<int>(ref this.ticksUntilEnabled, "ticksUntilEnabled", 0, false);
		}

		
		public string signalTag;

		
		private bool sent;

		
		private int ticksUntilEnabled;

		
		private const float MaxDistActivationByOther = 40f;
	}
}
