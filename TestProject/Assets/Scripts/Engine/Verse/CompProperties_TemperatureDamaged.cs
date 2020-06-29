using System;

namespace Verse
{
	
	public class CompProperties_TemperatureDamaged : CompProperties
	{
		
		public CompProperties_TemperatureDamaged()
		{
			this.compClass = typeof(CompTemperatureDamaged);
		}

		
		public FloatRange safeTemperatureRange = new FloatRange(-30f, 30f);

		
		public int damagePerTickRare = 1;
	}
}
