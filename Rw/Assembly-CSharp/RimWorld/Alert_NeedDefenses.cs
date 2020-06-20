using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DE0 RID: 3552
	public class Alert_NeedDefenses : Alert
	{
		// Token: 0x0600562D RID: 22061 RVA: 0x001C8E85 File Offset: 0x001C7085
		public Alert_NeedDefenses()
		{
			this.defaultLabel = "NeedDefenses".Translate();
			this.defaultExplanation = "NeedDefensesDesc".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x0600562E RID: 22062 RVA: 0x001C8EC0 File Offset: 0x001C70C0
		public override AlertReport GetReport()
		{
			if (GenDate.DaysPassed < 2 || GenDate.DaysPassed > 5)
			{
				return false;
			}
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				if (this.NeedDefenses(maps[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600562F RID: 22063 RVA: 0x001C8F18 File Offset: 0x001C7118
		private bool NeedDefenses(Map map)
		{
			if (!map.IsPlayerHome)
			{
				return false;
			}
			if (!map.mapPawns.AnyColonistSpawned && !map.listerBuildings.allBuildingsColonist.Any<Building>())
			{
				return false;
			}
			List<Building> allBuildingsColonist = map.listerBuildings.allBuildingsColonist;
			for (int i = 0; i < allBuildingsColonist.Count; i++)
			{
				Building building = allBuildingsColonist[i];
				if ((building.def.building != null && (building.def.building.IsTurret || building.def.building.isTrap)) || building.def == ThingDefOf.Sandbags || building.def == ThingDefOf.Barricade)
				{
					return false;
				}
			}
			return true;
		}
	}
}
