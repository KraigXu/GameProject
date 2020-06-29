using System;

namespace Verse
{
	
	public class HediffCompProperties_Immunizable : HediffCompProperties
	{
		
		public HediffCompProperties_Immunizable()
		{
			this.compClass = typeof(HediffComp_Immunizable);
		}

		
		public float immunityPerDayNotSick;

		
		public float immunityPerDaySick;

		
		public float severityPerDayNotImmune;

		
		public float severityPerDayImmune;

		
		public FloatRange severityPerDayNotImmuneRandomFactor = new FloatRange(1f, 1f);
	}
}
