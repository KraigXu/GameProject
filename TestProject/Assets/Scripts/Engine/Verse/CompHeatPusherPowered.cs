using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000320 RID: 800
	public class CompHeatPusherPowered : CompHeatPusher
	{
		// Token: 0x170004D8 RID: 1240
		// (get) Token: 0x0600175E RID: 5982 RVA: 0x00085920 File Offset: 0x00083B20
		protected override bool ShouldPushHeatNow
		{
			get
			{
				return base.ShouldPushHeatNow && FlickUtility.WantsToBeOn(this.parent) && (this.powerComp == null || this.powerComp.PowerOn) && (this.refuelableComp == null || this.refuelableComp.HasFuel) && (this.breakdownableComp == null || !this.breakdownableComp.BrokenDown);
			}
		}

		// Token: 0x0600175F RID: 5983 RVA: 0x00085984 File Offset: 0x00083B84
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.powerComp = this.parent.GetComp<CompPowerTrader>();
			this.refuelableComp = this.parent.GetComp<CompRefuelable>();
			this.breakdownableComp = this.parent.GetComp<CompBreakdownable>();
		}

		// Token: 0x04000EA5 RID: 3749
		protected CompPowerTrader powerComp;

		// Token: 0x04000EA6 RID: 3750
		protected CompRefuelable refuelableComp;

		// Token: 0x04000EA7 RID: 3751
		protected CompBreakdownable breakdownableComp;
	}
}
