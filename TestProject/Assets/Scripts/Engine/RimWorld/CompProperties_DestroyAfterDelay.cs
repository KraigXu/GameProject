using System;
using Verse;

namespace RimWorld
{
	
	public class CompProperties_DestroyAfterDelay : CompProperties
	{
		
		public CompProperties_DestroyAfterDelay()
		{
			this.compClass = typeof(CompDestroyAfterDelay);
		}

		
		public int delayTicks;
	}
}
