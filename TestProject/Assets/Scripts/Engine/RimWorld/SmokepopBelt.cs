using System;
using Verse;

namespace RimWorld
{
	
	public class SmokepopBelt : Apparel
	{
		
		public override bool CheckPreAbsorbDamage(DamageInfo dinfo)
		{
			if (!dinfo.Def.isExplosive && dinfo.Def.harmsHealth && dinfo.Def.ExternalViolenceFor(this) && dinfo.Def.isRanged && base.Wearer.Spawned)
			{
				GenExplosion.DoExplosion(base.Wearer.Position, base.Wearer.Map, this.GetStatValue(StatDefOf.SmokepopBeltRadius, true), DamageDefOf.Smoke, null, -1, -1f, null, null, null, null, ThingDefOf.Gas_Smoke, 1f, 1, false, null, 0f, 1, 0f, false, null, null);
				this.Destroy(DestroyMode.Vanish);
			}
			return false;
		}

		
		public override float GetSpecialApparelScoreOffset()
		{
			return this.GetStatValue(StatDefOf.SmokepopBeltRadius, true) * this.ApparelScorePerBeltRadius;
		}

		
		private float ApparelScorePerBeltRadius = 0.046f;
	}
}
