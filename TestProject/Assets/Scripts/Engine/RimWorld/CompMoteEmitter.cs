using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D2E RID: 3374
	public class CompMoteEmitter : ThingComp
	{
		// Token: 0x17000E78 RID: 3704
		// (get) Token: 0x060051FF RID: 20991 RVA: 0x001B6626 File Offset: 0x001B4826
		private CompProperties_MoteEmitter Props
		{
			get
			{
				return (CompProperties_MoteEmitter)this.props;
			}
		}

		// Token: 0x06005200 RID: 20992 RVA: 0x001B6634 File Offset: 0x001B4834
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

		// Token: 0x06005201 RID: 20993 RVA: 0x001B6708 File Offset: 0x001B4908
		protected void Emit()
		{
			this.mote = MoteMaker.MakeStaticMote(this.parent.DrawPos + this.Props.offset, this.parent.Map, this.Props.mote, 1f);
		}

		// Token: 0x06005202 RID: 20994 RVA: 0x001B6758 File Offset: 0x001B4958
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.ticksSinceLastEmitted, ((this.Props.saveKeysPrefix != null) ? (this.Props.saveKeysPrefix + "_") : "") + "ticksSinceLastEmitted", 0, false);
		}

		// Token: 0x04002D31 RID: 11569
		public int ticksSinceLastEmitted;

		// Token: 0x04002D32 RID: 11570
		protected Mote mote;
	}
}
