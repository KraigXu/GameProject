using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000867 RID: 2151
	public class CompProperties_CausesGameCondition_ClimateAdjuster : CompProperties_CausesGameCondition
	{
		// Token: 0x06003508 RID: 13576 RVA: 0x00122787 File Offset: 0x00120987
		public CompProperties_CausesGameCondition_ClimateAdjuster()
		{
			this.compClass = typeof(CompCauseGameCondition_TemperatureOffset);
		}

		// Token: 0x04001C3E RID: 7230
		public FloatRange temperatureOffsetRange = new FloatRange(-10f, 10f);
	}
}
