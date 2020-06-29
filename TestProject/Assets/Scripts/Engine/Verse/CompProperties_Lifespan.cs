using System;

namespace Verse
{
	
	public class CompProperties_Lifespan : CompProperties
	{
		
		public CompProperties_Lifespan()
		{
			this.compClass = typeof(CompLifespan);
		}

		
		public int lifespanTicks = 100;
	}
}
