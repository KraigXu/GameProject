using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E00 RID: 3584
	public class Alert_ShieldUserHasRangedWeapon : Alert
	{
		// Token: 0x17000F75 RID: 3957
		// (get) Token: 0x060056AF RID: 22191 RVA: 0x001CBD60 File Offset: 0x001C9F60
		private List<Pawn> ShieldUsersWithRangedWeapon
		{
			get
			{
				this.shieldUsersWithRangedWeaponResult.Clear();
				foreach (Pawn pawn in PawnsFinder.AllMaps_FreeColonistsSpawned)
				{
					if (pawn.equipment.Primary != null && pawn.equipment.Primary.def.IsRangedWeapon)
					{
						List<Apparel> wornApparel = pawn.apparel.WornApparel;
						for (int i = 0; i < wornApparel.Count; i++)
						{
							if (wornApparel[i] is ShieldBelt)
							{
								this.shieldUsersWithRangedWeaponResult.Add(pawn);
								break;
							}
						}
					}
				}
				return this.shieldUsersWithRangedWeaponResult;
			}
		}

		// Token: 0x060056B0 RID: 22192 RVA: 0x001CBE1C File Offset: 0x001CA01C
		public Alert_ShieldUserHasRangedWeapon()
		{
			this.defaultLabel = "ShieldUserHasRangedWeapon".Translate();
			this.defaultExplanation = "ShieldUserHasRangedWeaponDesc".Translate();
		}

		// Token: 0x060056B1 RID: 22193 RVA: 0x001CBE59 File Offset: 0x001CA059
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.ShieldUsersWithRangedWeapon);
		}

		// Token: 0x04002F3D RID: 12093
		private List<Pawn> shieldUsersWithRangedWeaponResult = new List<Pawn>();
	}
}
