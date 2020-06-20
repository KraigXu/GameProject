using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A01 RID: 2561
	public class IncidentWorker_Aurora : IncidentWorker_MakeGameCondition
	{
		// Token: 0x06003CEA RID: 15594 RVA: 0x001427DC File Offset: 0x001409DC
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			if (!base.CanFireNowSub(parms))
			{
				return false;
			}
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				if (maps[i].IsPlayerHome && !this.AuroraWillEndSoon(maps[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003CEB RID: 15595 RVA: 0x0014282B File Offset: 0x00140A2B
		private bool AuroraWillEndSoon(Map map)
		{
			return GenCelestial.CurCelestialSunGlow(map) > 0.5f || GenCelestial.CelestialSunGlow(map, Find.TickManager.TicksAbs + 5000) > 0.5f;
		}

		// Token: 0x04002398 RID: 9112
		private const int EnsureMinDurationTicks = 5000;
	}
}
