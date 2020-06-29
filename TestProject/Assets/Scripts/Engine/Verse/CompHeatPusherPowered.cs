using System;
using RimWorld;

namespace Verse
{
	
	public class CompHeatPusherPowered : CompHeatPusher
	{
		
		// (get) Token: 0x0600175E RID: 5982 RVA: 0x00085920 File Offset: 0x00083B20
		protected override bool ShouldPushHeatNow
		{
			get
			{
				return base.ShouldPushHeatNow && FlickUtility.WantsToBeOn(this.parent) && (this.powerComp == null || this.powerComp.PowerOn) && (this.refuelableComp == null || this.refuelableComp.HasFuel) && (this.breakdownableComp == null || !this.breakdownableComp.BrokenDown);
			}
		}

		
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.powerComp = this.parent.GetComp<CompPowerTrader>();
			this.refuelableComp = this.parent.GetComp<CompRefuelable>();
			this.breakdownableComp = this.parent.GetComp<CompBreakdownable>();
		}

		
		protected CompPowerTrader powerComp;

		
		protected CompRefuelable refuelableComp;

		
		protected CompBreakdownable breakdownableComp;
	}
}
