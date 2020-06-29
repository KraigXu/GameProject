using System;

namespace RimWorld
{
	
	public class StorytellerCompProperties_RefiringUniqueQuest : StorytellerCompProperties
	{
		
		public StorytellerCompProperties_RefiringUniqueQuest()
		{
			this.compClass = typeof(StorytellerComp_RefiringUniqueQuest);
		}

		
		public IncidentDef incident;

		
		public float refireEveryDays = -1f;
	}
}
