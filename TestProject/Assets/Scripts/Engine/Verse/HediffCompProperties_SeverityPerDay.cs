using System;

namespace Verse
{
	
	public class HediffCompProperties_SeverityPerDay : HediffCompProperties
	{
		
		public HediffCompProperties_SeverityPerDay()
		{
			this.compClass = typeof(HediffComp_SeverityPerDay);
		}

		
		public float severityPerDay;

		
		public bool showDaysToRecover;

		
		public bool showHoursToRecover;
	}
}
