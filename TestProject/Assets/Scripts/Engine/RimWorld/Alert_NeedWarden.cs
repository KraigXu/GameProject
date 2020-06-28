using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DE4 RID: 3556
	public class Alert_NeedWarden : Alert
	{
		// Token: 0x0600563C RID: 22076 RVA: 0x001C949C File Offset: 0x001C769C
		public Alert_NeedWarden()
		{
			this.defaultLabel = "NeedWarden".Translate();
			this.defaultExplanation = "NeedWardenDesc".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x0600563D RID: 22077 RVA: 0x001C94D8 File Offset: 0x001C76D8
		public override AlertReport GetReport()
		{
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				Map map = maps[i];
				if (map.IsPlayerHome && map.mapPawns.PrisonersOfColonySpawned.Any<Pawn>())
				{
					bool flag = false;
					foreach (Pawn pawn in map.mapPawns.FreeColonistsSpawned)
					{
						if (!pawn.Downed && pawn.workSettings != null && pawn.workSettings.GetPriority(WorkTypeDefOf.Warden) > 0)
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						return AlertReport.CulpritIs(map.mapPawns.PrisonersOfColonySpawned[0]);
					}
				}
			}
			return false;
		}
	}
}
