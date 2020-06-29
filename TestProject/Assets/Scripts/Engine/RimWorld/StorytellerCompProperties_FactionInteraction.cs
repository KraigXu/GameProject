using System;

namespace RimWorld
{
	
	public class StorytellerCompProperties_FactionInteraction : StorytellerCompProperties
	{
		
		public StorytellerCompProperties_FactionInteraction()
		{
			this.compClass = typeof(StorytellerComp_FactionInteraction);
		}

		
		public IncidentDef incident;

		
		public float baseIncidentsPerYear;

		
		public float minSpacingDays;

		
		public StoryDanger minDanger;

		
		public bool fullAlliesOnly;
	}
}
