using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class Alert_BrawlerHasRangedWeapon : Alert
	{
		
		
		private List<Pawn> BrawlersWithRangedWeapon
		{
			get
			{
				this.brawlersWithRangedWeaponResult.Clear();
				foreach (Pawn pawn in PawnsFinder.AllMaps_FreeColonistsSpawned)
				{
					if (pawn.story.traits.HasTrait(TraitDefOf.Brawler) && pawn.equipment.Primary != null && pawn.equipment.Primary.def.IsRangedWeapon)
					{
						this.brawlersWithRangedWeaponResult.Add(pawn);
					}
				}
				return this.brawlersWithRangedWeaponResult;
			}
		}

		
		public Alert_BrawlerHasRangedWeapon()
		{
			this.defaultLabel = "BrawlerHasRangedWeapon".Translate();
			this.defaultExplanation = "BrawlerHasRangedWeaponDesc".Translate();
		}

		
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.BrawlersWithRangedWeapon);
		}

		
		private List<Pawn> brawlersWithRangedWeaponResult = new List<Pawn>();
	}
}
