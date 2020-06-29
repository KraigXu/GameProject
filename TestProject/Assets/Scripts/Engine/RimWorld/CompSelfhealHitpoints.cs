using System;
using Verse;

namespace RimWorld
{
	
	public class CompSelfhealHitpoints : ThingComp
	{
		
		
		public CompProperties_SelfhealHitpoints Props
		{
			get
			{
				return (CompProperties_SelfhealHitpoints)this.props;
			}
		}

		
		public override void PostExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksPassedSinceLastHeal, "ticksPassedSinceLastHeal", 0, false);
		}

		
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

		
		public int ticksPassedSinceLastHeal;
	}
}
