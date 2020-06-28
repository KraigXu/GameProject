using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C5E RID: 3166
	public static class TurretGunUtility
	{
		// Token: 0x06004BD0 RID: 19408 RVA: 0x001987CE File Offset: 0x001969CE
		public static bool NeedsShells(ThingDef turret)
		{
			return turret.category == ThingCategory.Building && turret.building.IsTurret && turret.building.turretGunDef.HasComp(typeof(CompChangeableProjectile));
		}

		// Token: 0x06004BD1 RID: 19409 RVA: 0x00198804 File Offset: 0x00196A04
		public static ThingDef TryFindRandomShellDef(ThingDef turret, bool allowEMP = true, bool mustHarmHealth = true, TechLevel techLevel = TechLevel.Undefined, bool allowAntigrainWarhead = false, float maxMarketValue = -1f)
		{
			if (!TurretGunUtility.NeedsShells(turret))
			{
				return null;
			}
			ThingFilter fixedFilter = turret.building.turretGunDef.building.fixedStorageSettings.filter;
			ThingDef result;
			if ((from x in DefDatabase<ThingDef>.AllDefsListForReading
			where fixedFilter.Allows(x) && (allowEMP || x.projectileWhenLoaded.projectile.damageDef != DamageDefOf.EMP) && (!mustHarmHealth || x.projectileWhenLoaded.projectile.damageDef.harmsHealth) && (techLevel == TechLevel.Undefined || x.techLevel <= techLevel) && (allowAntigrainWarhead || x != ThingDefOf.Shell_AntigrainWarhead) && (maxMarketValue < 0f || x.BaseMarketValue <= maxMarketValue)
			select x).TryRandomElement(out result))
			{
				return result;
			}
			return null;
		}
	}
}
