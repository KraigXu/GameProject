using System;

namespace Verse
{
	
	public class HediffCompProperties_SelfHeal : HediffCompProperties
	{
		
		public HediffCompProperties_SelfHeal()
		{
			this.compClass = typeof(HediffComp_SelfHeal);
		}

		
		public int healIntervalTicksStanding = 50;

		
		public float healAmount = 1f;
	}
}
