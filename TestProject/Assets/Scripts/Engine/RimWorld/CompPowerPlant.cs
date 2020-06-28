using System;

namespace RimWorld
{
	// Token: 0x02000A79 RID: 2681
	public class CompPowerPlant : CompPowerTrader
	{
		// Token: 0x17000B3B RID: 2875
		// (get) Token: 0x06003F4D RID: 16205 RVA: 0x0015078F File Offset: 0x0014E98F
		protected virtual float DesiredPowerOutput
		{
			get
			{
				return -base.Props.basePowerConsumption;
			}
		}

		// Token: 0x06003F4E RID: 16206 RVA: 0x001507A0 File Offset: 0x0014E9A0
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.refuelableComp = this.parent.GetComp<CompRefuelable>();
			this.breakdownableComp = this.parent.GetComp<CompBreakdownable>();
			if (base.Props.basePowerConsumption < 0f && !this.parent.IsBrokenDown() && FlickUtility.WantsToBeOn(this.parent))
			{
				base.PowerOn = true;
			}
		}

		// Token: 0x06003F4F RID: 16207 RVA: 0x00150809 File Offset: 0x0014EA09
		public override void CompTick()
		{
			base.CompTick();
			this.UpdateDesiredPowerOutput();
		}

		// Token: 0x06003F50 RID: 16208 RVA: 0x00150818 File Offset: 0x0014EA18
		public void UpdateDesiredPowerOutput()
		{
			if ((this.breakdownableComp != null && this.breakdownableComp.BrokenDown) || (this.refuelableComp != null && !this.refuelableComp.HasFuel) || (this.flickableComp != null && !this.flickableComp.SwitchIsOn) || !base.PowerOn)
			{
				base.PowerOutput = 0f;
				return;
			}
			base.PowerOutput = this.DesiredPowerOutput;
		}

		// Token: 0x040024CA RID: 9418
		protected CompRefuelable refuelableComp;

		// Token: 0x040024CB RID: 9419
		protected CompBreakdownable breakdownableComp;
	}
}
