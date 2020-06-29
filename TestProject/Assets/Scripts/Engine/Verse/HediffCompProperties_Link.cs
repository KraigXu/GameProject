using System;

namespace Verse
{
	
	public class HediffCompProperties_Link : HediffCompProperties
	{
		
		public HediffCompProperties_Link()
		{
			this.compClass = typeof(HediffComp_Link);
		}

		
		public bool showName = true;

		
		public float maxDistance = -1f;
	}
}
