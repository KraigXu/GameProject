using System;
using Verse;

namespace RimWorld
{
	
	public class CompProperties_Initiatable : CompProperties
	{
		
		public CompProperties_Initiatable()
		{
			this.compClass = typeof(CompInitiatable);
		}

		
		public int initiationDelayTicks;
	}
}
