using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CC4 RID: 3268
	public class SmokepopBelt : Apparel
	{
		// Token: 0x06004F46 RID: 20294 RVA: 0x001AB4E4 File Offset: 0x001A96E4
		public override bool CheckPreAbsorbDamage(DamageInfo dinfo)
		{
			if (!dinfo.Def.isExplosive && dinfo.Def.harmsHealth && dinfo.Def.ExternalViolenceFor(this) && dinfo.Def.isRanged && base.Wearer.Spawned)
			{
				GenExplosion.DoExplosion(base.Wearer.Position, base.Wearer.Map, this.GetStatValue(StatDefOf.SmokepopBeltRadius, true), DamageDefOf.Smoke, null, -1, -1f, null, null, null, null, ThingDefOf.Gas_Smoke, 1f, 1, false, null, 0f, 1, 0f, false, null, null);
				this.Destroy(DestroyMode.Vanish);
			}
			return false;
		}

		// Token: 0x06004F47 RID: 20295 RVA: 0x001AB59F File Offset: 0x001A979F
		public override float GetSpecialApparelScoreOffset()
		{
			return this.GetStatValue(StatDefOf.SmokepopBeltRadius, true) * this.ApparelScorePerBeltRadius;
		}

		// Token: 0x04002C7D RID: 11389
		private float ApparelScorePerBeltRadius = 0.046f;
	}
}
