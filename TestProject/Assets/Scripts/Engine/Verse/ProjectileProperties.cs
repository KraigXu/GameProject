using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000094 RID: 148
	public class ProjectileProperties
	{
		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x060004E2 RID: 1250 RVA: 0x00018C24 File Offset: 0x00016E24
		public float StoppingPower
		{
			get
			{
				if (this.stoppingPower != 0f)
				{
					return this.stoppingPower;
				}
				if (this.damageDef != null)
				{
					return this.damageDef.defaultStoppingPower;
				}
				return 0f;
			}
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x060004E3 RID: 1251 RVA: 0x00018C53 File Offset: 0x00016E53
		public float SpeedTilesPerTick
		{
			get
			{
				return this.speed / 100f;
			}
		}

		// Token: 0x060004E4 RID: 1252 RVA: 0x00018C64 File Offset: 0x00016E64
		public int GetDamageAmount(Thing weapon, StringBuilder explanation = null)
		{
			float weaponDamageMultiplier = (weapon != null) ? weapon.GetStatValue(StatDefOf.RangedWeapon_DamageMultiplier, true) : 1f;
			return this.GetDamageAmount(weaponDamageMultiplier, explanation);
		}

		// Token: 0x060004E5 RID: 1253 RVA: 0x00018C90 File Offset: 0x00016E90
		public int GetDamageAmount_NewTmp(ThingDef weapon, ThingDef weaponStuff, StringBuilder explanation = null)
		{
			float weaponDamageMultiplier = (weapon != null) ? weapon.GetStatValueAbstract(StatDefOf.RangedWeapon_DamageMultiplier, weaponStuff) : 1f;
			return this.GetDamageAmount(weaponDamageMultiplier, explanation);
		}

		// Token: 0x060004E6 RID: 1254 RVA: 0x00018CBC File Offset: 0x00016EBC
		public int GetDamageAmount(float weaponDamageMultiplier, StringBuilder explanation = null)
		{
			int num;
			if (this.damageAmountBase != -1)
			{
				num = this.damageAmountBase;
			}
			else
			{
				if (this.damageDef == null)
				{
					Log.ErrorOnce("Failed to find sane damage amount", 91094882, false);
					return 1;
				}
				num = this.damageDef.defaultDamage;
			}
			if (explanation != null)
			{
				explanation.AppendLine("StatsReport_BaseValue".Translate() + ": " + num);
				explanation.Append("StatsReport_QualityMultiplier".Translate() + ": " + weaponDamageMultiplier.ToStringPercent());
			}
			num = Mathf.RoundToInt((float)num * weaponDamageMultiplier);
			if (explanation != null)
			{
				explanation.AppendLine();
				explanation.AppendLine();
				explanation.Append("StatsReport_FinalValue".Translate() + ": " + num);
			}
			return num;
		}

		// Token: 0x060004E7 RID: 1255 RVA: 0x00018DA8 File Offset: 0x00016FA8
		public float GetArmorPenetration(Thing weapon, StringBuilder explanation = null)
		{
			float weaponDamageMultiplier = (weapon != null) ? weapon.GetStatValue(StatDefOf.RangedWeapon_DamageMultiplier, true) : 1f;
			return this.GetArmorPenetration(weaponDamageMultiplier, explanation);
		}

		// Token: 0x060004E8 RID: 1256 RVA: 0x00018DD4 File Offset: 0x00016FD4
		public float GetArmorPenetration(float weaponDamageMultiplier, StringBuilder explanation = null)
		{
			if (this.damageDef.armorCategory == null)
			{
				return 0f;
			}
			float num;
			if (this.damageAmountBase != -1 || this.armorPenetrationBase >= 0f)
			{
				num = this.armorPenetrationBase;
			}
			else
			{
				if (this.damageDef == null)
				{
					return 0f;
				}
				num = this.damageDef.defaultArmorPenetration;
			}
			if (num < 0f)
			{
				num = (float)this.GetDamageAmount(null, null) * 0.015f;
			}
			if (explanation != null)
			{
				explanation.AppendLine("StatsReport_BaseValue".Translate() + ": " + num.ToStringPercent());
				explanation.AppendLine();
				explanation.Append("StatsReport_QualityMultiplier".Translate() + ": " + weaponDamageMultiplier.ToStringPercent());
			}
			num *= weaponDamageMultiplier;
			if (explanation != null)
			{
				explanation.AppendLine();
				explanation.AppendLine();
				explanation.Append("StatsReport_FinalValue".Translate() + ": " + num.ToStringPercent());
			}
			return num;
		}

		// Token: 0x060004E9 RID: 1257 RVA: 0x00018EE8 File Offset: 0x000170E8
		public IEnumerable<string> ConfigErrors(ThingDef parent)
		{
			if (this.alwaysFreeIntercept && this.flyOverhead)
			{
				yield return "alwaysFreeIntercept and flyOverhead are both true";
			}
			if (this.damageAmountBase == -1 && this.damageDef != null && this.damageDef.defaultDamage == -1)
			{
				yield return "no damage amount specified for projectile";
			}
			yield break;
		}

		// Token: 0x04000286 RID: 646
		public float speed = 5f;

		// Token: 0x04000287 RID: 647
		public bool flyOverhead;

		// Token: 0x04000288 RID: 648
		public bool alwaysFreeIntercept;

		// Token: 0x04000289 RID: 649
		public DamageDef damageDef;

		// Token: 0x0400028A RID: 650
		private int damageAmountBase = -1;

		// Token: 0x0400028B RID: 651
		private float armorPenetrationBase = -1f;

		// Token: 0x0400028C RID: 652
		public float stoppingPower = 0.5f;

		// Token: 0x0400028D RID: 653
		public List<ExtraDamage> extraDamages;

		// Token: 0x0400028E RID: 654
		public SoundDef soundHitThickRoof;

		// Token: 0x0400028F RID: 655
		public SoundDef soundExplode;

		// Token: 0x04000290 RID: 656
		public SoundDef soundImpactAnticipate;

		// Token: 0x04000291 RID: 657
		public SoundDef soundAmbient;

		// Token: 0x04000292 RID: 658
		public float explosionRadius;

		// Token: 0x04000293 RID: 659
		public int explosionDelay;

		// Token: 0x04000294 RID: 660
		public ThingDef preExplosionSpawnThingDef;

		// Token: 0x04000295 RID: 661
		public float preExplosionSpawnChance = 1f;

		// Token: 0x04000296 RID: 662
		public int preExplosionSpawnThingCount = 1;

		// Token: 0x04000297 RID: 663
		public ThingDef postExplosionSpawnThingDef;

		// Token: 0x04000298 RID: 664
		public float postExplosionSpawnChance = 1f;

		// Token: 0x04000299 RID: 665
		public int postExplosionSpawnThingCount = 1;

		// Token: 0x0400029A RID: 666
		public bool applyDamageToExplosionCellsNeighbors;

		// Token: 0x0400029B RID: 667
		public float explosionChanceToStartFire;

		// Token: 0x0400029C RID: 668
		public bool explosionDamageFalloff;

		// Token: 0x0400029D RID: 669
		public EffecterDef explosionEffect;

		// Token: 0x0400029E RID: 670
		public bool ai_IsIncendiary;
	}
}
