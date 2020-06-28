using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DEA RID: 3562
	public class Alert_HunterLacksRangedWeapon : Alert
	{
		// Token: 0x17000F64 RID: 3940
		// (get) Token: 0x0600565A RID: 22106 RVA: 0x001CA168 File Offset: 0x001C8368
		private List<Pawn> HuntersWithoutRangedWeapon
		{
			get
			{
				this.huntersWithoutRangedWeaponResult.Clear();
				foreach (Pawn pawn in PawnsFinder.AllMaps_FreeColonistsSpawned)
				{
					if (pawn.workSettings.WorkIsActive(WorkTypeDefOf.Hunting) && !WorkGiver_HunterHunt.HasHuntingWeapon(pawn) && !pawn.Downed)
					{
						this.huntersWithoutRangedWeaponResult.Add(pawn);
					}
				}
				return this.huntersWithoutRangedWeaponResult;
			}
		}

		// Token: 0x0600565B RID: 22107 RVA: 0x001CA1F4 File Offset: 0x001C83F4
		public Alert_HunterLacksRangedWeapon()
		{
			this.defaultLabel = "HunterLacksWeapon".Translate();
			this.defaultExplanation = "HunterLacksWeaponDesc".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x0600565C RID: 22108 RVA: 0x001CA243 File Offset: 0x001C8443
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.HuntersWithoutRangedWeapon);
		}

		// Token: 0x04002F29 RID: 12073
		private List<Pawn> huntersWithoutRangedWeaponResult = new List<Pawn>();
	}
}
