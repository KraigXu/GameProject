using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class Alert_HunterHasShieldAndRangedWeapon : Alert
	{
		
		
		private List<Pawn> BadHunters
		{
			get
			{
				this.badHuntersResult.Clear();
				foreach (Pawn pawn in PawnsFinder.AllMaps_FreeColonistsSpawned)
				{
					if (pawn.workSettings.WorkIsActive(WorkTypeDefOf.Hunting) && WorkGiver_HunterHunt.HasShieldAndRangedWeapon(pawn))
					{
						this.badHuntersResult.Add(pawn);
					}
				}
				return this.badHuntersResult;
			}
		}

		
		public Alert_HunterHasShieldAndRangedWeapon()
		{
			this.defaultLabel = "HunterHasShieldAndRangedWeapon".Translate();
			this.defaultExplanation = "HunterHasShieldAndRangedWeaponDesc".Translate();
		}

		
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.BadHunters);
		}

		
		private List<Pawn> badHuntersResult = new List<Pawn>();
	}
}
