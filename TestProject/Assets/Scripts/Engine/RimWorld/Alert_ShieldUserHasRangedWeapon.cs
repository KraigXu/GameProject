using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class Alert_ShieldUserHasRangedWeapon : Alert
	{
		
		
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

		
		public Alert_ShieldUserHasRangedWeapon()
		{
			this.defaultLabel = "ShieldUserHasRangedWeapon".Translate();
			this.defaultExplanation = "ShieldUserHasRangedWeaponDesc".Translate();
		}

		
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.ShieldUsersWithRangedWeapon);
		}

		
		private List<Pawn> shieldUsersWithRangedWeaponResult = new List<Pawn>();
	}
}
