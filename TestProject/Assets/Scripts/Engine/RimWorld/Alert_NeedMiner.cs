using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class Alert_NeedMiner : Alert
	{
		
		public Alert_NeedMiner()
		{
			this.defaultLabel = "NeedMiner".Translate();
			this.defaultExplanation = "NeedMinerDesc".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		
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
