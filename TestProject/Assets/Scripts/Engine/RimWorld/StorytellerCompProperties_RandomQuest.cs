using System;

namespace RimWorld
{
	
	public class StorytellerCompProperties_RandomQuest : StorytellerCompProperties_OnOffCycle
	{
		
		public StorytellerCompProperties_RandomQuest()
		{
			this.compClass = typeof(StorytellerComp_RandomQuest);
		}
	}
}
