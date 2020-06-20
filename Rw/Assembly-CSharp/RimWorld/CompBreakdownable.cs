using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CF6 RID: 3318
	public class CompBreakdownable : ThingComp
	{
		// Token: 0x17000E27 RID: 3623
		// (get) Token: 0x060050A4 RID: 20644 RVA: 0x001B19EE File Offset: 0x001AFBEE
		public bool BrokenDown
		{
			get
			{
				return this.brokenDownInt;
			}
		}

		// Token: 0x060050A5 RID: 20645 RVA: 0x001B19F6 File Offset: 0x001AFBF6
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<bool>(ref this.brokenDownInt, "brokenDown", false, false);
		}

		// Token: 0x060050A6 RID: 20646 RVA: 0x001B1A10 File Offset: 0x001AFC10
		public override void PostDraw()
		{
			if (this.brokenDownInt)
			{
				this.parent.Map.overlayDrawer.DrawOverlay(this.parent, OverlayTypes.BrokenDown);
			}
		}

		// Token: 0x060050A7 RID: 20647 RVA: 0x001B1A37 File Offset: 0x001AFC37
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.powerComp = this.parent.GetComp<CompPowerTrader>();
			this.parent.Map.GetComponent<BreakdownManager>().Register(this);
		}

		// Token: 0x060050A8 RID: 20648 RVA: 0x001B1A67 File Offset: 0x001AFC67
		public override void PostDeSpawn(Map map)
		{
			base.PostDeSpawn(map);
			map.GetComponent<BreakdownManager>().Deregister(this);
		}

		// Token: 0x060050A9 RID: 20649 RVA: 0x001B1A7C File Offset: 0x001AFC7C
		public void CheckForBreakdown()
		{
			if (this.CanBreakdownNow() && Rand.MTBEventOccurs(1.368E+07f, 1f, 1041f))
			{
				this.DoBreakdown();
			}
		}

		// Token: 0x060050AA RID: 20650 RVA: 0x001B1AA2 File Offset: 0x001AFCA2
		protected bool CanBreakdownNow()
		{
			return !this.BrokenDown && (this.powerComp == null || this.powerComp.PowerOn);
		}

		// Token: 0x060050AB RID: 20651 RVA: 0x001B1AC4 File Offset: 0x001AFCC4
		public void Notify_Repaired()
		{
			this.brokenDownInt = false;
			this.parent.Map.GetComponent<BreakdownManager>().Notify_Repaired(this.parent);
			if (this.parent is Building_PowerSwitch)
			{
				this.parent.Map.powerNetManager.Notfiy_TransmitterTransmitsPowerNowChanged(this.parent.GetComp<CompPower>());
			}
		}

		// Token: 0x060050AC RID: 20652 RVA: 0x001B1B20 File Offset: 0x001AFD20
		public void DoBreakdown()
		{
			this.brokenDownInt = true;
			this.parent.BroadcastCompSignal("Breakdown");
			this.parent.Map.GetComponent<BreakdownManager>().Notify_BrokenDown(this.parent);
		}

		// Token: 0x060050AD RID: 20653 RVA: 0x001B1B54 File Offset: 0x001AFD54
		public override string CompInspectStringExtra()
		{
			if (this.BrokenDown)
			{
				return "BrokenDown".Translate();
			}
			return null;
		}

		// Token: 0x04002CCD RID: 11469
		private bool brokenDownInt;

		// Token: 0x04002CCE RID: 11470
		private CompPowerTrader powerComp;

		// Token: 0x04002CCF RID: 11471
		private const int BreakdownMTBTicks = 13680000;

		// Token: 0x04002CD0 RID: 11472
		public const string BreakdownSignal = "Breakdown";
	}
}
