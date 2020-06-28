using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200078F RID: 1935
	public static class SappersUtility
	{
		// Token: 0x06003282 RID: 12930 RVA: 0x00119086 File Offset: 0x00117286
		public static bool IsGoodSapper(Pawn p)
		{
			return p.kindDef.canBeSapper && SappersUtility.HasBuildingDestroyerWeapon(p) && SappersUtility.CanMineReasonablyFast(p);
		}

		// Token: 0x06003283 RID: 12931 RVA: 0x001190A5 File Offset: 0x001172A5
		public static bool IsGoodBackupSapper(Pawn p)
		{
			return p.kindDef.canBeSapper && SappersUtility.CanMineReasonablyFast(p);
		}

		// Token: 0x06003284 RID: 12932 RVA: 0x001190BC File Offset: 0x001172BC
		private static bool CanMineReasonablyFast(Pawn p)
		{
			return p.RaceProps.Humanlike && !p.skills.GetSkill(SkillDefOf.Mining).TotallyDisabled && !StatDefOf.MiningSpeed.Worker.IsDisabledFor(p) && p.skills.GetSkill(SkillDefOf.Mining).Level >= 4;
		}

		// Token: 0x06003285 RID: 12933 RVA: 0x0011911C File Offset: 0x0011731C
		public static bool HasBuildingDestroyerWeapon(Pawn p)
		{
			if (p.equipment == null || p.equipment.Primary == null)
			{
				return false;
			}
			List<Verb> allVerbs = p.equipment.Primary.GetComp<CompEquippable>().AllVerbs;
			for (int i = 0; i < allVerbs.Count; i++)
			{
				if (allVerbs[i].verbProps.ai_IsBuildingDestroyer)
				{
					return true;
				}
			}
			return false;
		}
	}
}
