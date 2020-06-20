using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000874 RID: 2164
	public class CompProperties_Power : CompProperties
	{
		// Token: 0x0600352D RID: 13613 RVA: 0x00122F32 File Offset: 0x00121132
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
		{
			foreach (StatDrawEntry statDrawEntry in this.<>n__0(req))
			{
				yield return statDrawEntry;
			}
			IEnumerator<StatDrawEntry> enumerator = null;
			if (this.basePowerConsumption > 0f)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Building, "PowerConsumption".Translate(), this.basePowerConsumption.ToString("F0") + " W", "Stat_Thing_PowerConsumption_Desc".Translate(), 5000, null, null, false);
			}
			yield break;
			yield break;
		}

		// Token: 0x04001C7E RID: 7294
		public bool transmitsPower;

		// Token: 0x04001C7F RID: 7295
		public float basePowerConsumption;

		// Token: 0x04001C80 RID: 7296
		public bool shortCircuitInRain;

		// Token: 0x04001C81 RID: 7297
		public SoundDef soundPowerOn;

		// Token: 0x04001C82 RID: 7298
		public SoundDef soundPowerOff;

		// Token: 0x04001C83 RID: 7299
		public SoundDef soundAmbientPowered;
	}
}
