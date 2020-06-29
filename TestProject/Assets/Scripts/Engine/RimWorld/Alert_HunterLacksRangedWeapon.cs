using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class Alert_HunterLacksRangedWeapon : Alert
	{
		
		
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
