using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class Alert_HunterLacksRangedWeapon : Alert
	{
		
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

		
		public Alert_HunterLacksRangedWeapon()
		{
			this.defaultLabel = "HunterLacksWeapon".Translate();
			this.defaultExplanation = "HunterLacksWeaponDesc".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.HuntersWithoutRangedWeapon);
		}

		
		private List<Pawn> huntersWithoutRangedWeaponResult = new List<Pawn>();
	}
}
