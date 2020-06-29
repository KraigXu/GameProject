using System;

namespace Verse
{
	
	public class HediffCompProperties_Disappears : HediffCompProperties
	{
		
		public HediffCompProperties_Disappears()
		{
			this.compClass = typeof(HediffComp_Disappears);
		}

		
		public IntRange disappearsAfterTicks;

		
		public bool showRemainingTime;
	}
}
