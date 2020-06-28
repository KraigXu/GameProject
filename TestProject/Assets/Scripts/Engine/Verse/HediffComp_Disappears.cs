using System;

namespace Verse
{
	// Token: 0x0200024A RID: 586
	public class HediffComp_Disappears : HediffComp
	{
		// Token: 0x17000338 RID: 824
		// (get) Token: 0x0600103F RID: 4159 RVA: 0x0005D4BC File Offset: 0x0005B6BC
		public HediffCompProperties_Disappears Props
		{
			get
			{
				return (HediffCompProperties_Disappears)this.props;
			}
		}

		// Token: 0x17000339 RID: 825
		// (get) Token: 0x06001040 RID: 4160 RVA: 0x0005D4C9 File Offset: 0x0005B6C9
		public override bool CompShouldRemove
		{
			get
			{
				return base.CompShouldRemove || this.ticksToDisappear <= 0;
			}
		}

		// Token: 0x1700033A RID: 826
		// (get) Token: 0x06001041 RID: 4161 RVA: 0x0005D4E4 File Offset: 0x0005B6E4
		public override string CompLabelInBracketsExtra
		{
			get
			{
				if (!this.Props.showRemainingTime)
				{
					return base.CompLabelInBracketsExtra;
				}
				return this.ticksToDisappear.TicksToSeconds().ToString("0.0");
			}
		}

		// Token: 0x06001042 RID: 4162 RVA: 0x0005D51D File Offset: 0x0005B71D
		public override void CompPostMake()
		{
			base.CompPostMake();
			this.ticksToDisappear = this.Props.disappearsAfterTicks.RandomInRange;
		}

		// Token: 0x06001043 RID: 4163 RVA: 0x0005D53B File Offset: 0x0005B73B
		public override void CompPostTick(ref float severityAdjustment)
		{
			this.ticksToDisappear--;
		}

		// Token: 0x06001044 RID: 4164 RVA: 0x0005D54C File Offset: 0x0005B74C
		public override void CompPostMerged(Hediff other)
		{
			base.CompPostMerged(other);
			HediffComp_Disappears hediffComp_Disappears = other.TryGetComp<HediffComp_Disappears>();
			if (hediffComp_Disappears != null && hediffComp_Disappears.ticksToDisappear > this.ticksToDisappear)
			{
				this.ticksToDisappear = hediffComp_Disappears.ticksToDisappear;
			}
		}

		// Token: 0x06001045 RID: 4165 RVA: 0x0005D584 File Offset: 0x0005B784
		public override void CompExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksToDisappear, "ticksToDisappear", 0, false);
		}

		// Token: 0x06001046 RID: 4166 RVA: 0x0005D598 File Offset: 0x0005B798
		public override string CompDebugString()
		{
			return "ticksToDisappear: " + this.ticksToDisappear;
		}

		// Token: 0x04000BE9 RID: 3049
		public int ticksToDisappear;
	}
}
