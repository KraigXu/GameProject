using System;

namespace Verse
{
	// Token: 0x02000241 RID: 577
	public class HediffComp_ChanceToRemove : HediffComp
	{
		// Token: 0x17000332 RID: 818
		// (get) Token: 0x06001025 RID: 4133 RVA: 0x0005D01C File Offset: 0x0005B21C
		public HediffCompProperties_ChanceToRemove Props
		{
			get
			{
				return (HediffCompProperties_ChanceToRemove)this.props;
			}
		}

		// Token: 0x17000333 RID: 819
		// (get) Token: 0x06001026 RID: 4134 RVA: 0x0005D029 File Offset: 0x0005B229
		public override bool CompShouldRemove
		{
			get
			{
				return base.CompShouldRemove || (this.removeNextInterval && this.currentInterval <= 0);
			}
		}

		// Token: 0x06001027 RID: 4135 RVA: 0x0005D04C File Offset: 0x0005B24C
		public override void CompPostTick(ref float severityAdjustment)
		{
			if (this.CompShouldRemove)
			{
				return;
			}
			if (this.currentInterval > 0)
			{
				this.currentInterval--;
				return;
			}
			if (Rand.Chance(this.Props.chance))
			{
				this.removeNextInterval = true;
				this.currentInterval = Rand.Range(0, this.Props.intervalTicks);
				return;
			}
			this.currentInterval = this.Props.intervalTicks;
		}

		// Token: 0x06001028 RID: 4136 RVA: 0x0005D0BC File Offset: 0x0005B2BC
		public override void CompExposeData()
		{
			Scribe_Values.Look<int>(ref this.currentInterval, "currentInterval", 0, false);
			Scribe_Values.Look<bool>(ref this.removeNextInterval, "removeNextInterval", false, false);
		}

		// Token: 0x06001029 RID: 4137 RVA: 0x0005D0E2 File Offset: 0x0005B2E2
		public override string CompDebugString()
		{
			return string.Format("currentInterval: {0}\nremove: {1}", this.currentInterval, this.removeNextInterval);
		}

		// Token: 0x04000BDA RID: 3034
		public int currentInterval;

		// Token: 0x04000BDB RID: 3035
		public bool removeNextInterval;
	}
}
