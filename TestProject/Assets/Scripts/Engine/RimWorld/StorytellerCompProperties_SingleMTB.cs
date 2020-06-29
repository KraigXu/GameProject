using System;

namespace RimWorld
{
	
	public class StorytellerCompProperties_SingleMTB : StorytellerCompProperties
	{
		
		public StorytellerCompProperties_SingleMTB()
		{
			this.compClass = typeof(StorytellerComp_SingleMTB);
		}

		
		public IncidentDef incident;

		
		public float mtbDays = 13f;
	}
}
