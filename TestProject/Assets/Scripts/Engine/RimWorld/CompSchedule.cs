using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A81 RID: 2689
	public class CompSchedule : ThingComp
	{
		// Token: 0x17000B45 RID: 2885
		// (get) Token: 0x06003F8C RID: 16268 RVA: 0x00151C59 File Offset: 0x0014FE59
		public CompProperties_Schedule Props
		{
			get
			{
				return (CompProperties_Schedule)this.props;
			}
		}

		// Token: 0x17000B46 RID: 2886
		// (get) Token: 0x06003F8D RID: 16269 RVA: 0x00151C66 File Offset: 0x0014FE66
		// (set) Token: 0x06003F8E RID: 16270 RVA: 0x00151C6E File Offset: 0x0014FE6E
		public bool Allowed
		{
			get
			{
				return this.intAllowed;
			}
			set
			{
				if (this.intAllowed == value)
				{
					return;
				}
				this.intAllowed = value;
				this.parent.BroadcastCompSignal(this.intAllowed ? "ScheduledOn" : "ScheduledOff");
			}
		}

		// Token: 0x06003F8F RID: 16271 RVA: 0x00151CA0 File Offset: 0x0014FEA0
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.RecalculateAllowed();
		}

		// Token: 0x06003F90 RID: 16272 RVA: 0x00151CAF File Offset: 0x0014FEAF
		public override void CompTickRare()
		{
			base.CompTickRare();
			this.RecalculateAllowed();
		}

		// Token: 0x06003F91 RID: 16273 RVA: 0x00151CC0 File Offset: 0x0014FEC0
		public void RecalculateAllowed()
		{
			float num = GenLocalDate.DayPercent(this.parent);
			if (this.Props.startTime <= this.Props.endTime)
			{
				this.Allowed = (num > this.Props.startTime && num < this.Props.endTime);
				return;
			}
			this.Allowed = (num < this.Props.endTime || num > this.Props.startTime);
		}

		// Token: 0x06003F92 RID: 16274 RVA: 0x00151D3C File Offset: 0x0014FF3C
		public override string CompInspectStringExtra()
		{
			if (!this.Allowed)
			{
				return this.Props.offMessage;
			}
			return null;
		}

		// Token: 0x040024F9 RID: 9465
		public const string ScheduledOnSignal = "ScheduledOn";

		// Token: 0x040024FA RID: 9466
		public const string ScheduledOffSignal = "ScheduledOff";

		// Token: 0x040024FB RID: 9467
		private bool intAllowed;
	}
}
