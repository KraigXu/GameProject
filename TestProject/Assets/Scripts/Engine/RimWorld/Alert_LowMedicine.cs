using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DE6 RID: 3558
	public class Alert_LowMedicine : Alert
	{
		// Token: 0x06005642 RID: 22082 RVA: 0x001C96F9 File Offset: 0x001C78F9
		public Alert_LowMedicine()
		{
			this.defaultLabel = "LowMedicine".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x06005643 RID: 22083 RVA: 0x001C9720 File Offset: 0x001C7920
		public override TaggedString GetExplanation()
		{
			Map map = this.MapWithLowMedicine();
			if (map == null)
			{
				return "";
			}
			int num = this.MedicineCount(map);
			if (num == 0)
			{
				return "NoMedicineDesc".Translate();
			}
			return "LowMedicineDesc".Translate(num);
		}

		// Token: 0x06005644 RID: 22084 RVA: 0x001C9768 File Offset: 0x001C7968
		public override AlertReport GetReport()
		{
			if (Find.TickManager.TicksGame < 150000)
			{
				return false;
			}
			return this.MapWithLowMedicine() != null;
		}

		// Token: 0x06005645 RID: 22085 RVA: 0x001C9790 File Offset: 0x001C7990
		private Map MapWithLowMedicine()
		{
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				Map map = maps[i];
				if (map.IsPlayerHome && map.mapPawns.AnyColonistSpawned && (float)this.MedicineCount(map) < 2f * (float)map.mapPawns.FreeColonistsSpawnedCount)
				{
					return map;
				}
			}
			return null;
		}

		// Token: 0x06005646 RID: 22086 RVA: 0x001C97F0 File Offset: 0x001C79F0
		private int MedicineCount(Map map)
		{
			return map.resourceCounter.GetCountIn(ThingRequestGroup.Medicine);
		}

		// Token: 0x04002F1F RID: 12063
		private const float MedicinePerColonistThreshold = 2f;
	}
}
