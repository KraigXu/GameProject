using System;

namespace RimWorld.Planet
{
	
	public class WorldObjectCompProperties_EnterCooldown : WorldObjectCompProperties
	{
		
		public WorldObjectCompProperties_EnterCooldown()
		{
			this.compClass = typeof(EnterCooldownComp);
		}

		
		public bool autoStartOnMapRemoved = true;

		
		public float durationDays = 4f;
	}
}
