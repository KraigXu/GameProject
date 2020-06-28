using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009E2 RID: 2530
	public class IncidentWorker_ColdSnap : IncidentWorker_MakeGameCondition
	{
		// Token: 0x06003C50 RID: 15440 RVA: 0x0013E936 File Offset: 0x0013CB36
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			return base.CanFireNowSub(parms) && IncidentWorker_ColdSnap.IsTemperatureAppropriate((Map)parms.target);
		}

		// Token: 0x06003C51 RID: 15441 RVA: 0x0013E953 File Offset: 0x0013CB53
		public static bool IsTemperatureAppropriate(Map map)
		{
			return map.mapTemperature.SeasonalTemp > 0f && map.mapTemperature.SeasonalTemp < 15f;
		}
	}
}
