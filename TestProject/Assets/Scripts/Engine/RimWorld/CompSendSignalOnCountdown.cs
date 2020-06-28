using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000D4D RID: 3405
	public class CompSendSignalOnCountdown : ThingComp
	{
		// Token: 0x17000EAA RID: 3754
		// (get) Token: 0x060052C8 RID: 21192 RVA: 0x001BA796 File Offset: 0x001B8996
		private CompProperties_SendSignalOnCountdown Props
		{
			get
			{
				return (CompProperties_SendSignalOnCountdown)this.props;
			}
		}

		// Token: 0x060052C9 RID: 21193 RVA: 0x001BA7A3 File Offset: 0x001B89A3
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.signalTag = this.Props.signalTag;
			this.ticksLeft = Mathf.CeilToInt(Rand.ByCurve(this.Props.countdownCurveTicks));
		}

		// Token: 0x060052CA RID: 21194 RVA: 0x001BA7D8 File Offset: 0x001B89D8
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

		// Token: 0x060052CB RID: 21195 RVA: 0x001BA7E8 File Offset: 0x001B89E8
		public override void CompTick()
		{
			base.CompTick();
			if (this.parent.IsHashIntervalTick(250))
			{
				this.TickRareWorker();
			}
		}

		// Token: 0x060052CC RID: 21196 RVA: 0x001BA808 File Offset: 0x001B8A08
		public override void CompTickRare()
		{
			base.CompTickRare();
			this.TickRareWorker();
		}

		// Token: 0x060052CD RID: 21197 RVA: 0x001BA818 File Offset: 0x001B8A18
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

		// Token: 0x060052CE RID: 21198 RVA: 0x001BA8A8 File Offset: 0x001B8AA8
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

		// Token: 0x060052CF RID: 21199 RVA: 0x001BA908 File Offset: 0x001B8B08
		public override void Notify_SignalReceived(Signal signal)
		{
			Thing thing;
			if (signal.tag == "CompCanBeDormant.WakeUp" && signal.args.TryGetArg<Thing>("SUBJECT", out thing) && thing != this.parent && thing != null && thing.Map == this.parent.Map && this.parent.Position.DistanceTo(thing.Position) <= 40f)
			{
				this.ticksLeft = 0;
			}
		}

		// Token: 0x060052D0 RID: 21200 RVA: 0x001BA97F File Offset: 0x001B8B7F
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.ticksLeft, "ticksLeft", 0, false);
			Scribe_Values.Look<string>(ref this.signalTag, "signalTag", null, false);
		}

		// Token: 0x04002DB3 RID: 11699
		public string signalTag;

		// Token: 0x04002DB4 RID: 11700
		public int ticksLeft;

		// Token: 0x04002DB5 RID: 11701
		private const float MaxDistActivationByOther = 40f;
	}
}
