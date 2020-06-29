using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class CompProperties_Power : CompProperties
	{
		
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
		{


			IEnumerator<StatDrawEntry> enumerator = null;
			if (this.basePowerConsumption > 0f)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Building, "PowerConsumption".Translate(), this.basePowerConsumption.ToString("F0") + " W", "Stat_Thing_PowerConsumption_Desc".Translate(), 5000, null, null, false);
			}
			yield break;
			yield break;
		}

		
		public bool transmitsPower;

		
		public float basePowerConsumption;

		
		public bool shortCircuitInRain;

		
		public SoundDef soundPowerOn;

		
		public SoundDef soundPowerOff;

		
		public SoundDef soundAmbientPowered;
	}
}
