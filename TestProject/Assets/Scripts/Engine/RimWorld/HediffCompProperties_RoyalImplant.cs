using System;
using Verse;

namespace RimWorld
{
	
	public class HediffCompProperties_RoyalImplant : HediffCompProperties
	{
		
		public HediffCompProperties_RoyalImplant()
		{
			this.compClass = typeof(HediffComp_RoyalImplant);
		}

		
		public string violationTriggerDescriptionKey;
	}
}
