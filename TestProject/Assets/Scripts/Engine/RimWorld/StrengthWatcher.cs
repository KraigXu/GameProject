using System;
using Verse;

namespace RimWorld
{
	
	public class StrengthWatcher
	{
		
		// (get) Token: 0x0600409A RID: 16538 RVA: 0x0015A170 File Offset: 0x00158370
		public float StrengthRating
		{
			get
			{
				float num = 0f;
				foreach (Pawn pawn in this.map.mapPawns.FreeColonists)
				{
					float num2 = 1f;
					num2 *= pawn.health.summaryHealth.SummaryHealthPercent;
					if (pawn.Downed)
					{
						num2 *= 0.3f;
					}
					num += num2;
				}
				foreach (Building building in this.map.listerBuildings.allBuildingsColonistCombatTargets)
				{
					if (building.def.building != null && building.def.building.IsTurret)
					{
						num += 0.3f;
					}
				}
				return num;
			}
		}

		
		public StrengthWatcher(Map map)
		{
			this.map = map;
		}

		
		private Map map;
	}
}
