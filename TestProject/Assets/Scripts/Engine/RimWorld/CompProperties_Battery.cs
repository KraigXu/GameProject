using System;

namespace RimWorld
{
	
	public class CompProperties_Battery : CompProperties_Power
	{
		
		public CompProperties_Battery()
		{
			this.compClass = typeof(CompPowerBattery);
		}

		
		public float storedEnergyMax = 1000f;

		
		public float efficiency = 0.5f;
	}
}
