using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class Alert_HunterHasShieldAndRangedWeapon : Alert
	{
		
		// (get) Token: 0x060056BC RID: 22204 RVA: 0x001CC2B4 File Offset: 0x001CA4B4
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
