using System;

namespace RimWorld
{
	
	public class CompProperties_CausesGameCondition_PsychicEmanation : CompProperties_CausesGameCondition
	{
		
		public CompProperties_CausesGameCondition_PsychicEmanation()
		{
			this.compClass = typeof(CompCauseGameCondition_PsychicEmanation);
		}

		
		public PsychicDroneLevel droneLevel = PsychicDroneLevel.BadHigh;

		
		public int droneLevelIncreaseInterval = int.MinValue;
	}
}
