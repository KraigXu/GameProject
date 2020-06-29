using System;

namespace Verse
{
	
	public class HediffCompProperties_KillAfterDays : HediffCompProperties
	{
		
		public HediffCompProperties_KillAfterDays()
		{
			this.compClass = typeof(HediffComp_KillAfterDays);
		}

		
		public int days;
	}
}
