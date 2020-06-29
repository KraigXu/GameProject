﻿using System;

namespace Verse
{
	
	public class HediffComp_ChanceToRemove : HediffComp
	{
		
		// (get) Token: 0x06001025 RID: 4133 RVA: 0x0005D01C File Offset: 0x0005B21C
		public HediffCompProperties_ChanceToRemove Props
		{
			get
			{
				return (HediffCompProperties_ChanceToRemove)this.props;
			}
		}

		
		// (get) Token: 0x06001026 RID: 4134 RVA: 0x0005D029 File Offset: 0x0005B229
		public override bool CompShouldRemove
		{
			get
			{
				return base.CompShouldRemove || (this.removeNextInterval && this.currentInterval <= 0);
			}
		}

		
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

		
		public override void CompExposeData()
		{
			Scribe_Values.Look<int>(ref this.currentInterval, "currentInterval", 0, false);
			Scribe_Values.Look<bool>(ref this.removeNextInterval, "removeNextInterval", false, false);
		}

		
		public override string CompDebugString()
		{
			return string.Format("currentInterval: {0}\nremove: {1}", this.currentInterval, this.removeNextInterval);
		}

		
		public int currentInterval;

		
		public bool removeNextInterval;
	}
}
