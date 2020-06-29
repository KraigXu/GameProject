using System;
using Verse;

namespace RimWorld
{
	
	public class CompMelter : ThingComp
	{
		
		public override void CompTickRare()
		{
			float ambientTemperature = this.parent.AmbientTemperature;
			if (ambientTemperature < 0f)
			{
				return;
			}
			int num = GenMath.RoundRandom(0.15f * (ambientTemperature / 10f));
			if (num > 0)
			{
				this.parent.TakeDamage(new DamageInfo(DamageDefOf.Rotting, (float)num, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
			}
		}

		
		private const float MeltPerIntervalPer10Degrees = 0.15f;
	}
}
