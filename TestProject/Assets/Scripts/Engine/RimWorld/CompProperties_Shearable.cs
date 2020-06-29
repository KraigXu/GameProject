using System;
using Verse;

namespace RimWorld
{
	
	public class CompProperties_Shearable : CompProperties
	{
		
		public CompProperties_Shearable()
		{
			this.compClass = typeof(CompShearable);
		}

		
		public int shearIntervalDays;

		
		public int woolAmount = 1;

		
		public ThingDef woolDef;
	}
}
