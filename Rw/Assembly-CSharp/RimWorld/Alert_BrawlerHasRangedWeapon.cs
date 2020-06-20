using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DFF RID: 3583
	public class Alert_BrawlerHasRangedWeapon : Alert
	{
		// Token: 0x17000F74 RID: 3956
		// (get) Token: 0x060056AC RID: 22188 RVA: 0x001CBC70 File Offset: 0x001C9E70
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

		// Token: 0x060056AD RID: 22189 RVA: 0x001CBD14 File Offset: 0x001C9F14
		public Alert_BrawlerHasRangedWeapon()
		{
			this.defaultLabel = "BrawlerHasRangedWeapon".Translate();
			this.defaultExplanation = "BrawlerHasRangedWeaponDesc".Translate();
		}

		// Token: 0x060056AE RID: 22190 RVA: 0x001CBD51 File Offset: 0x001C9F51
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.BrawlersWithRangedWeapon);
		}

		// Token: 0x04002F3C RID: 12092
		private List<Pawn> brawlersWithRangedWeaponResult = new List<Pawn>();
	}
}
