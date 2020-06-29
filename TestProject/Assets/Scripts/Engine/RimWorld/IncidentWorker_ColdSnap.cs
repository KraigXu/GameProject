using System;
using Verse;

namespace RimWorld
{
	
	public class IncidentWorker_ColdSnap : IncidentWorker_MakeGameCondition
	{
		
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			return base.CanFireNowSub(parms) && IncidentWorker_ColdSnap.IsTemperatureAppropriate((Map)parms.target);
		}

		
		public static bool IsTemperatureAppropriate(Map map)
		{
			return map.mapTemperature.SeasonalTemp > 0f && map.mapTemperature.SeasonalTemp < 15f;
		}
	}
}
