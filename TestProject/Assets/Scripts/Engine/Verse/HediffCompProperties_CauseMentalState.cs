using System;

namespace Verse
{
	
	public class HediffCompProperties_CauseMentalState : HediffCompProperties
	{
		
		public HediffCompProperties_CauseMentalState()
		{
			this.compClass = typeof(HediffComp_CauseMentalState);
		}

		
		public MentalStateDef animalMentalState;

		
		public MentalStateDef animalMentalStateAlias;

		
		public MentalStateDef humanMentalState;

		
		public LetterDef letterDef;

		
		public float mtbDaysToCauseMentalState;
	}
}
