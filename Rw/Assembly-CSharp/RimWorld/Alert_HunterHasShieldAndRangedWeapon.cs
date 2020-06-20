using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E04 RID: 3588
	public class Alert_HunterHasShieldAndRangedWeapon : Alert
	{
		// Token: 0x17000F79 RID: 3961
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

		// Token: 0x060056BD RID: 22205 RVA: 0x001CC338 File Offset: 0x001CA538
		public Alert_HunterHasShieldAndRangedWeapon()
		{
			this.defaultLabel = "HunterHasShieldAndRangedWeapon".Translate();
			this.defaultExplanation = "HunterHasShieldAndRangedWeaponDesc".Translate();
		}

		// Token: 0x060056BE RID: 22206 RVA: 0x001CC375 File Offset: 0x001CA575
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.BadHunters);
		}

		// Token: 0x04002F42 RID: 12098
		private List<Pawn> badHuntersResult = new List<Pawn>();
	}
}
