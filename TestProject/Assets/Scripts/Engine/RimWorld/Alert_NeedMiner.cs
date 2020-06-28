using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DE3 RID: 3555
	public class Alert_NeedMiner : Alert
	{
		// Token: 0x0600563A RID: 22074 RVA: 0x001C9347 File Offset: 0x001C7547
		public Alert_NeedMiner()
		{
			this.defaultLabel = "NeedMiner".Translate();
			this.defaultExplanation = "NeedMinerDesc".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x0600563B RID: 22075 RVA: 0x001C9380 File Offset: 0x001C7580
		public override AlertReport GetReport()
		{
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				Map map = maps[i];
				if (map.IsPlayerHome)
				{
					Designation designation = null;
					List<Designation> allDesignations = map.designationManager.allDesignations;
					for (int j = 0; j < allDesignations.Count; j++)
					{
						if (allDesignations[j].def == DesignationDefOf.Mine)
						{
							designation = allDesignations[j];
							break;
						}
					}
					if (designation != null)
					{
						bool flag = false;
						foreach (Pawn pawn in map.mapPawns.FreeColonistsSpawned)
						{
							if (!pawn.Downed && pawn.workSettings != null && pawn.workSettings.GetPriority(WorkTypeDefOf.Mining) > 0)
							{
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							return AlertReport.CulpritIs(designation.target.Thing);
						}
					}
				}
			}
			return false;
		}
	}
}
