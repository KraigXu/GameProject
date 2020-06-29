using System;

namespace RimWorld
{
	
	public class StorytellerCompProperties_SingleOnceFixed : StorytellerCompProperties
	{
		
		public StorytellerCompProperties_SingleOnceFixed()
		{
			this.compClass = typeof(StorytellerComp_SingleOnceFixed);
		}

		
		public IncidentDef incident;

		
		public int fireAfterDaysPassed;

		
		public RoyalTitleDef skipIfColonistHasMinTitle;

		
		public bool skipIfOnExtremeBiome;

		
		public int minColonistCount;
	}
}
