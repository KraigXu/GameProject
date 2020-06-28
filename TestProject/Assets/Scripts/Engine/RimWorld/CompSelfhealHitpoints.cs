using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D4B RID: 3403
	public class CompSelfhealHitpoints : ThingComp
	{
		// Token: 0x17000EA9 RID: 3753
		// (get) Token: 0x060052C3 RID: 21187 RVA: 0x001BA6F8 File Offset: 0x001B88F8
		public CompProperties_SelfhealHitpoints Props
		{
			get
			{
				return (CompProperties_SelfhealHitpoints)this.props;
			}
		}

		// Token: 0x060052C4 RID: 21188 RVA: 0x001BA705 File Offset: 0x001B8905
		public override void PostExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksPassedSinceLastHeal, "ticksPassedSinceLastHeal", 0, false);
		}

		// Token: 0x060052C5 RID: 21189 RVA: 0x001BA71C File Offset: 0x001B891C
		public override void CompTick()
		{
			this.ticksPassedSinceLastHeal++;
			if (this.ticksPassedSinceLastHeal == this.Props.ticksPerHeal)
			{
				this.ticksPassedSinceLastHeal = 0;
				if (this.parent.HitPoints < this.parent.MaxHitPoints)
				{
					ThingWithComps parent = this.parent;
					int hitPoints = parent.HitPoints;
					parent.HitPoints = hitPoints + 1;
				}
			}
		}

		// Token: 0x04002DB0 RID: 11696
		public int ticksPassedSinceLastHeal;
	}
}
