using System;

namespace Verse
{
	
	public class CompProperties_HeatPusher : CompProperties
	{
		
		public CompProperties_HeatPusher()
		{
			this.compClass = typeof(CompHeatPusher);
		}

		
		public float heatPerSecond;

		
		public float heatPushMaxTemperature = 99999f;

		
		public float heatPushMinTemperature = -99999f;
	}
}
