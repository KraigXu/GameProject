using System;
using Verse;

namespace RimWorld
{
	
	public class CompMoteEmitter : ThingComp
	{
		
		// (get) Token: 0x060051FF RID: 20991 RVA: 0x001B6626 File Offset: 0x001B4826
		private CompProperties_MoteEmitter Props
		{
			get
			{
				return (CompProperties_MoteEmitter)this.props;
			}
		}

		
		public override void CompTick()
		{
			CompPowerTrader comp = this.parent.GetComp<CompPowerTrader>();
			if (comp != null && !comp.PowerOn)
			{
				return;
			}
			CompSendSignalOnCountdown comp2 = this.parent.GetComp<CompSendSignalOnCountdown>();
			if (comp2 != null && comp2.ticksLeft <= 0)
			{
				return;
			}
			CompInitiatable comp3 = this.parent.GetComp<CompInitiatable>();
			if (comp3 != null && !comp3.Initiated)
			{
				return;
			}
			if (this.Props.emissionInterval != -1 && !this.Props.maintain)
			{
				if (this.ticksSinceLastEmitted >= this.Props.emissionInterval)
				{
					this.Emit();
					this.ticksSinceLastEmitted = 0;
				}
				else
				{
					this.ticksSinceLastEmitted++;
				}
			}
			else if (this.mote == null)
			{
				this.Emit();
			}
			if (this.Props.maintain && this.mote != null)
			{
				this.mote.Maintain();
			}
		}

		
		protected void Emit()
		{
			this.mote = MoteMaker.MakeStaticMote(this.parent.DrawPos + this.Props.offset, this.parent.Map, this.Props.mote, 1f);
		}

		
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.ticksSinceLastEmitted, ((this.Props.saveKeysPrefix != null) ? (this.Props.saveKeysPrefix + "_") : "") + "ticksSinceLastEmitted", 0, false);
		}

		
		public int ticksSinceLastEmitted;

		
		protected Mote mote;
	}
}
