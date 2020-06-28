using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000CB1 RID: 3249
	public class Bullet : Projectile
	{
		// Token: 0x06004ED3 RID: 20179 RVA: 0x001A8988 File Offset: 0x001A6B88
		protected override void Impact(Thing hitThing)
		{
			Map map = base.Map;
			base.Impact(hitThing);
			BattleLogEntry_RangedImpact battleLogEntry_RangedImpact = new BattleLogEntry_RangedImpact(this.launcher, hitThing, this.intendedTarget.Thing, this.equipmentDef, this.def, this.targetCoverDef);
			Find.BattleLog.Add(battleLogEntry_RangedImpact);
			if (hitThing != null)
			{
				DamageInfo dinfo = new DamageInfo(this.def.projectile.damageDef, (float)base.DamageAmount, base.ArmorPenetration, this.ExactRotation.eulerAngles.y, this.launcher, null, this.equipmentDef, DamageInfo.SourceCategory.ThingOrUnknown, this.intendedTarget.Thing);
				hitThing.TakeDamage(dinfo).AssociateWithLog(battleLogEntry_RangedImpact);
				Pawn pawn = hitThing as Pawn;
				if (pawn != null && pawn.stances != null && pawn.BodySize <= this.def.projectile.StoppingPower + 0.001f)
				{
					pawn.stances.StaggerFor(95);
				}
				if (this.def.projectile.extraDamages == null)
				{
					return;
				}
				using (List<ExtraDamage>.Enumerator enumerator = this.def.projectile.extraDamages.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ExtraDamage extraDamage = enumerator.Current;
						if (Rand.Chance(extraDamage.chance))
						{
							DamageInfo dinfo2 = new DamageInfo(extraDamage.def, extraDamage.amount, extraDamage.AdjustedArmorPenetration(), this.ExactRotation.eulerAngles.y, this.launcher, null, this.equipmentDef, DamageInfo.SourceCategory.ThingOrUnknown, this.intendedTarget.Thing);
							hitThing.TakeDamage(dinfo2).AssociateWithLog(battleLogEntry_RangedImpact);
						}
					}
					return;
				}
			}
			SoundDefOf.BulletImpact_Ground.PlayOneShot(new TargetInfo(base.Position, map, false));
			if (base.Position.GetTerrain(map).takeSplashes)
			{
				MoteMaker.MakeWaterSplash(this.ExactPosition, map, Mathf.Sqrt((float)base.DamageAmount) * 1f, 4f);
				return;
			}
			MoteMaker.MakeStaticMote(this.ExactPosition, map, ThingDefOf.Mote_ShotHit_Dirt, 1f);
		}
	}
}
