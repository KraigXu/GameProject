using System;
using Verse;

namespace RimWorld
{
	
	public class CompProperties_CausesGameCondition_ClimateAdjuster : CompProperties_CausesGameCondition
	{
		
		public CompProperties_CausesGameCondition_ClimateAdjuster()
		{
			this.compClass = typeof(CompCauseGameCondition_TemperatureOffset);
		}

		
		public FloatRange temperatureOffsetRange = new FloatRange(-10f, 10f);
	}
}
