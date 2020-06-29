using System;
using Verse;

namespace RimWorld
{
	
	public class CompProperties_Milkable : CompProperties
	{
		
		public CompProperties_Milkable()
		{
			this.compClass = typeof(CompMilkable);
		}

		
		public int milkIntervalDays;

		
		public int milkAmount = 1;

		
		public ThingDef milkDef;

		
		public bool milkFemaleOnly = true;
	}
}
