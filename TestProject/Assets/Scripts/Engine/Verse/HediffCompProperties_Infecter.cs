using System;

namespace Verse
{
	
	public class HediffCompProperties_Infecter : HediffCompProperties
	{
		
		public HediffCompProperties_Infecter()
		{
			this.compClass = typeof(HediffComp_Infecter);
		}

		
		public float infectionChance = 0.5f;
	}
}
