using System;

namespace RimWorld
{
	
	public class StorytellerCompProperties_ThreatsGenerator : StorytellerCompProperties
	{
		
		public StorytellerCompProperties_ThreatsGenerator()
		{
			this.compClass = typeof(StorytellerComp_ThreatsGenerator);
		}

		
		public ThreatsGeneratorParams parms;
	}
}
