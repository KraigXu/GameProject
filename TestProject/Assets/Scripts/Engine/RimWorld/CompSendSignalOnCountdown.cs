using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	
	public class CompSendSignalOnCountdown : ThingComp
	{
		
		
		private CompProperties_SendSignalOnCountdown Props
		{
			get
			{
				return (CompProperties_SendSignalOnCountdown)this.props;
			}
		}

		
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.signalTag = this.Props.signalTag;
			this.ticksLeft = Mathf.CeilToInt(Rand.ByCurve(this.Props.countdownCurveTicks));
		}

		
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (!Prefs.DevMode)
			{
				yield break;
			}
			yield return new Command_Action
			{
				defaultLabel = "DEV: Activate",
				action = delegate
				{
					Find.SignalManager.SendSignal(new Signal(this.signalTag, this.parent.Named("SUBJECT")));
					SoundDefOf.MechanoidsWakeUp.PlayOneShot(new TargetInfo(this.parent.Position, this.parent.Map, false));
					this.ticksLeft = 0;
				}
			};
			yield break;
		}

		
		public override void CompTick()
		{
			base.CompTick();
			if (this.parent.IsHashIntervalTick(250))
			{
				this.TickRareWorker();
			}
		}

		
		public override void CompTickRare()
		{
			base.CompTickRare();
			this.TickRareWorker();
		}

		
		public void TickRareWorker()
		{
			if (this.ticksLeft <= 0 || !this.parent.Spawned)
			{
				return;
			}
			this.ticksLeft -= 250;
			if (this.ticksLeft <= 0)
			{
				Find.SignalManager.SendSignal(new Signal(this.signalTag, this.parent.Named("SUBJECT")));
				SoundDefOf.MechanoidsWakeUp.PlayOneShot(new TargetInfo(this.parent.Position, this.parent.Map, false));
			}
		}

		
		public override string CompInspectStringExtra()
		{
			if (!this.parent.Spawned)
			{
				return null;
			}
			if (this.ticksLeft <= 0)
			{
				return "expired".Translate().CapitalizeFirst();
			}
			return "SendSignalOnCountdownCompTime".Translate(this.ticksLeft.ToStringTicksToPeriod(true, false, true, true));
		}

		
		//public override void Notify_SignalReceived(Signal signal)
		//{
		//	Thing thing;
		//	if (signal.tag == "CompCanBeDormant.WakeUp" && signal.args.TryGetArg<Thing>("SUBJECT", out thing) && thing != this.parent && thing != null && thing.Map == this.parent.Map && this.parent.Position.DistanceTo(thing.Position) <= 40f)
		//	{
		//		this.ticksLeft = 0;
		//	}
		//}

		
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.ticksLeft, "ticksLeft", 0, false);
			Scribe_Values.Look<string>(ref this.signalTag, "signalTag", null, false);
		}

		
		public string signalTag;

		
		public int ticksLeft;

		
		private const float MaxDistActivationByOther = 40f;
	}
}
