using System;
using Verse;

namespace RimWorld
{
	
	public class CompProperties_SelfhealHitpoints : CompProperties
	{
		
		public CompProperties_SelfhealHitpoints()
		{
			this.compClass = typeof(CompSelfhealHitpoints);
		}

		
		public int ticksPerHeal;
	}
}
