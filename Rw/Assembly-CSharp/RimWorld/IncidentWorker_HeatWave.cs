using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009E1 RID: 2529
	public class IncidentWorker_HeatWave : IncidentWorker_MakeGameCondition
	{
		// Token: 0x06003C4D RID: 15437 RVA: 0x0013E8FA File Offset: 0x0013CAFA
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			return base.CanFireNowSub(parms) && IncidentWorker_HeatWave.IsTemperatureAppropriate((Map)parms.target);
		}

		// Token: 0x06003C4E RID: 15438 RVA: 0x0013E917 File Offset: 0x0013CB17
		public static bool IsTemperatureAppropriate(Map map)
		{
			return map.mapTemperature.SeasonalTemp >= 20f;
		}
	}
}
