using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DE5 RID: 3557
	public class Alert_LowFood : Alert
	{
		// Token: 0x0600563E RID: 22078 RVA: 0x001C95C4 File Offset: 0x001C77C4
		public Alert_LowFood()
		{
			this.defaultLabel = "LowFood".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x0600563F RID: 22079 RVA: 0x001C95E8 File Offset: 0x001C77E8
		public override TaggedString GetExplanation()
		{
			Map map = this.MapWithLowFood();
			if (map == null)
			{
				return "";
			}
			float totalHumanEdibleNutrition = map.resourceCounter.TotalHumanEdibleNutrition;
			int num = map.mapPawns.FreeColonistsSpawnedCount + map.mapPawns.PrisonersOfColonyCount;
			int num2 = Mathf.FloorToInt(totalHumanEdibleNutrition / (float)num);
			return "LowFoodDesc".Translate(totalHumanEdibleNutrition.ToString("F0"), num.ToStringCached(), num2.ToStringCached());
		}

		// Token: 0x06005640 RID: 22080 RVA: 0x001C9669 File Offset: 0x001C7869
		public override AlertReport GetReport()
		{
			if (Find.TickManager.TicksGame < 150000)
			{
				return false;
			}
			return this.MapWithLowFood() != null;
		}

		// Token: 0x06005641 RID: 22081 RVA: 0x001C9694 File Offset: 0x001C7894
		private Map MapWithLowFood()
		{
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				Map map = maps[i];
				if (map.IsPlayerHome && map.mapPawns.AnyColonistSpawned)
				{
					int freeColonistsSpawnedCount = map.mapPawns.FreeColonistsSpawnedCount;
					if (map.resourceCounter.TotalHumanEdibleNutrition < 4f * (float)freeColonistsSpawnedCount)
					{
						return map;
					}
				}
			}
			return null;
		}

		// Token: 0x04002F1E RID: 12062
		private const float NutritionThresholdPerColonist = 4f;
	}
}
