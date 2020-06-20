using System;

namespace Verse
{
	// Token: 0x0200026C RID: 620
	public class HediffComp_SelfHeal : HediffComp
	{
		// Token: 0x17000350 RID: 848
		// (get) Token: 0x060010B2 RID: 4274 RVA: 0x0005EFA0 File Offset: 0x0005D1A0
		public HediffCompProperties_SelfHeal Props
		{
			get
			{
				return (HediffCompProperties_SelfHeal)this.props;
			}
		}

		// Token: 0x060010B3 RID: 4275 RVA: 0x0005EFAD File Offset: 0x0005D1AD
		public override void CompExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksSinceHeal, "ticksSinceHeal", 0, false);
		}

		// Token: 0x060010B4 RID: 4276 RVA: 0x0005EFC1 File Offset: 0x0005D1C1
		public override void CompPostTick(ref float severityAdjustment)
		{
			this.ticksSinceHeal++;
			if (this.ticksSinceHeal > this.Props.healIntervalTicksStanding)
			{
				severityAdjustment -= this.Props.healAmount;
				this.ticksSinceHeal = 0;
			}
		}

		// Token: 0x04000C2A RID: 3114
		public int ticksSinceHeal;
	}
}
