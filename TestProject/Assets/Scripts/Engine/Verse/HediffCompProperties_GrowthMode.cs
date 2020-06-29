using System;

namespace Verse
{
	
	public class HediffCompProperties_GrowthMode : HediffCompProperties
	{
		
		public HediffCompProperties_GrowthMode()
		{
			this.compClass = typeof(HediffComp_GrowthMode);
		}

		
		public float severityPerDayGrowing;

		
		public float severityPerDayRemission;

		
		public FloatRange severityPerDayGrowingRandomFactor = new FloatRange(1f, 1f);

		
		public FloatRange severityPerDayRemissionRandomFactor = new FloatRange(1f, 1f);
	}
}
