using System;

namespace RimWorld
{
	
	public class StorytellerCompProperties_Triggered : StorytellerCompProperties
	{
		
		public StorytellerCompProperties_Triggered()
		{
			this.compClass = typeof(StorytellerComp_Triggered);
		}

		
		public IncidentDef incident;

		
		public int delayTicks = 60;
	}
}
