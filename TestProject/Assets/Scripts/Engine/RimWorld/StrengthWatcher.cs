using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AA9 RID: 2729
	public class StrengthWatcher
	{
		// Token: 0x17000B69 RID: 2921
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

		// Token: 0x0600409B RID: 16539 RVA: 0x0015A26C File Offset: 0x0015846C
		public StrengthWatcher(Map map)
		{
			this.map = map;
		}

		// Token: 0x04002579 RID: 9593
		private Map map;
	}
}
