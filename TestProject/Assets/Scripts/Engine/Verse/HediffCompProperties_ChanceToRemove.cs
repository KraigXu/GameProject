using System;

namespace Verse
{
	
	public class HediffCompProperties_ChanceToRemove : HediffCompProperties
	{
		
		public HediffCompProperties_ChanceToRemove()
		{
			this.compClass = typeof(HediffComp_ChanceToRemove);
		}

		
		public int intervalTicks;

		
		public float chance;
	}
}
