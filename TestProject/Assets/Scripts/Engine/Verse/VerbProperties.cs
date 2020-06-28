using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000A9 RID: 169
	public class VerbProperties
	{
		// Token: 0x170000EA RID: 234
		// (get) Token: 0x0600053A RID: 1338 RVA: 0x0001A587 File Offset: 0x00018787
		public bool CausesTimeSlowdown
		{
			get
			{
				return this.ai_IsWeapon && this.forceNormalTimeSpeed;
			}
		}

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x0600053B RID: 1339 RVA: 0x0001A599 File Offset: 0x00018799
		public bool LaunchesProjectile
		{
			get
			{
				return typeof(Verb_LaunchProjectile).IsAssignableFrom(this.verbClass);
			}
		}

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x0600053C RID: 1340 RVA: 0x0001A5B0 File Offset: 0x000187B0
		public string AccuracySummaryString
		{
			get
			{
				return string.Concat(new string[]
				{
					this.accuracyTouch.ToStringPercent(),
					" - ",
					this.accuracyShort.ToStringPercent(),
					" - ",
					this.accuracyMedium.ToStringPercent(),
					" - ",
					this.accuracyLong.ToStringPercent()
				});
			}
		}

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x0600053D RID: 1341 RVA: 0x0001A618 File Offset: 0x00018818
		public bool IsMeleeAttack
		{
			get
			{
				return typeof(Verb_MeleeAttack).IsAssignableFrom(this.verbClass);
			}
		}

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x0600053E RID: 1342 RVA: 0x0001A630 File Offset: 0x00018830
		public bool CausesExplosion
		{
			get
			{
				return this.defaultProjectile != null && (typeof(Projectile_Explosive).IsAssignableFrom(this.defaultProjectile.thingClass) || typeof(Projectile_DoomsdayRocket).IsAssignableFrom(this.defaultProjectile.thingClass) || this.defaultProjectile.GetCompProperties<CompProperties_Explosive>() != null);
			}
		}

		// Token: 0x0600053F RID: 1343 RVA: 0x0001A68F File Offset: 0x0001888F
		public float AdjustedMeleeDamageAmount(Verb ownerVerb, Pawn attacker)
		{
			if (ownerVerb.verbProps != this)
			{
				Log.ErrorOnce("Tried to calculate melee damage amount for a verb with different verb props. verb=" + ownerVerb, 5469809, false);
				return 0f;
			}
			return this.AdjustedMeleeDamageAmount(ownerVerb.tool, attacker, ownerVerb.EquipmentSource, ownerVerb.HediffCompSource);
		}

		// Token: 0x06000540 RID: 1344 RVA: 0x0001A6D0 File Offset: 0x000188D0
		public float AdjustedMeleeDamageAmount(Tool tool, Pawn attacker, Thing equipment, HediffComp_VerbGiver hediffCompSource)
		{
			if (!this.IsMeleeAttack)
			{
				Log.ErrorOnce(string.Format("Attempting to get melee damage for a non-melee verb {0}", this), 26181238, false);
			}
			float num;
			if (tool != null)
			{
				num = tool.AdjustedBaseMeleeDamageAmount(equipment, this.meleeDamageDef);
			}
			else
			{
				num = (float)this.meleeDamageBaseAmount;
			}
			if (attacker != null)
			{
				num *= this.GetDamageFactorFor(tool, attacker, hediffCompSource);
			}
			return num;
		}

		// Token: 0x06000541 RID: 1345 RVA: 0x0001A728 File Offset: 0x00018928
		public float AdjustedMeleeDamageAmount_NewTmp(Tool tool, Pawn attacker, ThingDef equipment, ThingDef equipmentStuff, HediffComp_VerbGiver hediffCompSource)
		{
			if (!this.IsMeleeAttack)
			{
				Log.ErrorOnce(string.Format("Attempting to get melee damage for a non-melee verb {0}", this), 26181238, false);
			}
			float num;
			if (tool != null)
			{
				num = tool.AdjustedBaseMeleeDamageAmount_NewTmp(equipment, equipmentStuff, this.meleeDamageDef);
			}
			else
			{
				num = (float)this.meleeDamageBaseAmount;
			}
			if (attacker != null)
			{
				num *= this.GetDamageFactorFor(tool, attacker, hediffCompSource);
			}
			return num;
		}

		// Token: 0x06000542 RID: 1346 RVA: 0x0001A781 File Offset: 0x00018981
		public float AdjustedArmorPenetration(Verb ownerVerb, Pawn attacker)
		{
			if (ownerVerb.verbProps != this)
			{
				Log.ErrorOnce("Tried to calculate armor penetration for a verb with different verb props. verb=" + ownerVerb, 9865767, false);
				return 0f;
			}
			return this.AdjustedArmorPenetration(ownerVerb.tool, attacker, ownerVerb.EquipmentSource, ownerVerb.HediffCompSource);
		}

		// Token: 0x06000543 RID: 1347 RVA: 0x0001A7C4 File Offset: 0x000189C4
		public float AdjustedArmorPenetration(Tool tool, Pawn attacker, Thing equipment, HediffComp_VerbGiver hediffCompSource)
		{
			float num;
			if (tool != null)
			{
				num = tool.armorPenetration;
			}
			else
			{
				num = this.meleeArmorPenetrationBase;
			}
			if (num < 0f)
			{
				num = this.AdjustedMeleeDamageAmount(tool, attacker, equipment, hediffCompSource) * 0.015f;
			}
			else if (equipment != null)
			{
				float statValue = equipment.GetStatValue(StatDefOf.MeleeWeapon_DamageMultiplier, true);
				num *= statValue;
			}
			return num;
		}

		// Token: 0x06000544 RID: 1348 RVA: 0x0001A818 File Offset: 0x00018A18
		public float AdjustedArmorPenetration_NewTmp(Tool tool, Pawn attacker, ThingDef equipment, ThingDef equipmentStuff, HediffComp_VerbGiver hediffCompSource)
		{
			float num;
			if (tool != null)
			{
				num = tool.armorPenetration;
			}
			else
			{
				num = this.meleeArmorPenetrationBase;
			}
			if (num < 0f)
			{
				num = this.AdjustedMeleeDamageAmount_NewTmp(tool, attacker, equipment, equipmentStuff, hediffCompSource) * 0.015f;
			}
			else if (equipment != null)
			{
				float statValueAbstract = equipment.GetStatValueAbstract(StatDefOf.MeleeWeapon_DamageMultiplier, equipmentStuff);
				num *= statValueAbstract;
			}
			return num;
		}

		// Token: 0x06000545 RID: 1349 RVA: 0x0001A86C File Offset: 0x00018A6C
		private float AdjustedExpectedDamageForVerbUsableInMelee(Tool tool, Pawn attacker, Thing equipment, HediffComp_VerbGiver hediffCompSource)
		{
			if (this.IsMeleeAttack)
			{
				return this.AdjustedMeleeDamageAmount(tool, attacker, equipment, hediffCompSource);
			}
			if (this.LaunchesProjectile && this.defaultProjectile != null)
			{
				return (float)this.defaultProjectile.projectile.GetDamageAmount(equipment, null);
			}
			return 0f;
		}

		// Token: 0x06000546 RID: 1350 RVA: 0x0001A8AC File Offset: 0x00018AAC
		private float AdjustedExpectedDamageForVerbUsableInMelee_NewTmp(Tool tool, Pawn attacker, ThingDef equipment, ThingDef equipmentStuff, HediffComp_VerbGiver hediffCompSource)
		{
			if (this.IsMeleeAttack)
			{
				return this.AdjustedMeleeDamageAmount_NewTmp(tool, attacker, equipment, equipmentStuff, hediffCompSource);
			}
			if (this.LaunchesProjectile && this.defaultProjectile != null)
			{
				return (float)this.defaultProjectile.projectile.GetDamageAmount_NewTmp(equipment, equipmentStuff, null);
			}
			return 0f;
		}

		// Token: 0x06000547 RID: 1351 RVA: 0x0001A8FC File Offset: 0x00018AFC
		public float AdjustedMeleeSelectionWeight(Verb ownerVerb, Pawn attacker)
		{
			if (ownerVerb.verbProps != this)
			{
				Log.ErrorOnce("Tried to calculate melee selection weight for a verb with different verb props. verb=" + ownerVerb, 385716351, false);
				return 0f;
			}
			return this.AdjustedMeleeSelectionWeight(ownerVerb.tool, attacker, ownerVerb.EquipmentSource, ownerVerb.HediffCompSource, ownerVerb.DirectOwner is Pawn);
		}

		// Token: 0x06000548 RID: 1352 RVA: 0x0001A958 File Offset: 0x00018B58
		public float AdjustedMeleeSelectionWeight(Tool tool, Pawn attacker, Thing equipment, HediffComp_VerbGiver hediffCompSource, bool comesFromPawnNativeVerbs)
		{
			if (!this.IsMeleeAttack)
			{
				return 0f;
			}
			if (attacker != null && attacker.RaceProps.intelligence < this.minIntelligence)
			{
				return 0f;
			}
			float num = 1f;
			float num2 = this.AdjustedExpectedDamageForVerbUsableInMelee(tool, attacker, equipment, hediffCompSource);
			if (num2 >= 0.001f || !typeof(Verb_MeleeApplyHediff).IsAssignableFrom(this.verbClass))
			{
				num *= num2 * num2;
			}
			num *= this.commonality;
			if (tool != null)
			{
				num *= tool.chanceFactor;
			}
			if (comesFromPawnNativeVerbs && (tool == null || !tool.alwaysTreatAsWeapon))
			{
				num *= 0.3f;
			}
			return num;
		}

		// Token: 0x06000549 RID: 1353 RVA: 0x0001A9F4 File Offset: 0x00018BF4
		public float AdjustedMeleeSelectionWeight_NewTmp(Tool tool, Pawn attacker, ThingDef equipment, ThingDef equipmentStuff, HediffComp_VerbGiver hediffCompSource, bool comesFromPawnNativeVerbs)
		{
			if (!this.IsMeleeAttack)
			{
				return 0f;
			}
			if (attacker != null && attacker.RaceProps.intelligence < this.minIntelligence)
			{
				return 0f;
			}
			float num = 1f;
			float num2 = this.AdjustedExpectedDamageForVerbUsableInMelee_NewTmp(tool, attacker, equipment, equipmentStuff, hediffCompSource);
			if (num2 >= 0.001f || !typeof(Verb_MeleeApplyHediff).IsAssignableFrom(this.verbClass))
			{
				num *= num2 * num2;
			}
			num *= this.commonality;
			if (tool != null)
			{
				num *= tool.chanceFactor;
			}
			if (comesFromPawnNativeVerbs && (tool == null || !tool.alwaysTreatAsWeapon))
			{
				num *= 0.3f;
			}
			return num;
		}

		// Token: 0x0600054A RID: 1354 RVA: 0x0001AA91 File Offset: 0x00018C91
		public float AdjustedCooldown(Verb ownerVerb, Pawn attacker)
		{
			if (ownerVerb.verbProps != this)
			{
				Log.ErrorOnce("Tried to calculate cooldown for a verb with different verb props. verb=" + ownerVerb, 19485711, false);
				return 0f;
			}
			return this.AdjustedCooldown(ownerVerb.tool, attacker, ownerVerb.EquipmentSource);
		}

		// Token: 0x0600054B RID: 1355 RVA: 0x0001AACB File Offset: 0x00018CCB
		public float AdjustedCooldown(Tool tool, Pawn attacker, Thing equipment)
		{
			if (tool != null)
			{
				return tool.AdjustedCooldown(equipment);
			}
			if (equipment != null && !this.IsMeleeAttack)
			{
				return equipment.GetStatValue(StatDefOf.RangedWeapon_Cooldown, true);
			}
			return this.defaultCooldownTime;
		}

		// Token: 0x0600054C RID: 1356 RVA: 0x0001AAF6 File Offset: 0x00018CF6
		public float AdjustedCooldown_NewTmp(Tool tool, Pawn attacker, ThingDef equipment, ThingDef equipmentStuff)
		{
			if (tool != null)
			{
				return tool.AdjustedCooldown_NewTmp(equipment, equipmentStuff);
			}
			if (equipment != null && !this.IsMeleeAttack)
			{
				return equipment.GetStatValueAbstract(StatDefOf.RangedWeapon_Cooldown, equipmentStuff);
			}
			return this.defaultCooldownTime;
		}

		// Token: 0x0600054D RID: 1357 RVA: 0x0001AB24 File Offset: 0x00018D24
		public int AdjustedCooldownTicks(Verb ownerVerb, Pawn attacker)
		{
			return this.AdjustedCooldown(ownerVerb, attacker).SecondsToTicks();
		}

		// Token: 0x0600054E RID: 1358 RVA: 0x0001AB34 File Offset: 0x00018D34
		private float AdjustedAccuracy(VerbProperties.RangeCategory cat, Thing equipment)
		{
			if (equipment != null)
			{
				StatDef stat = null;
				switch (cat)
				{
				case VerbProperties.RangeCategory.Touch:
					stat = StatDefOf.AccuracyTouch;
					break;
				case VerbProperties.RangeCategory.Short:
					stat = StatDefOf.AccuracyShort;
					break;
				case VerbProperties.RangeCategory.Medium:
					stat = StatDefOf.AccuracyMedium;
					break;
				case VerbProperties.RangeCategory.Long:
					stat = StatDefOf.AccuracyLong;
					break;
				}
				return equipment.GetStatValue(stat, true);
			}
			switch (cat)
			{
			case VerbProperties.RangeCategory.Touch:
				return this.accuracyTouch;
			case VerbProperties.RangeCategory.Short:
				return this.accuracyShort;
			case VerbProperties.RangeCategory.Medium:
				return this.accuracyMedium;
			case VerbProperties.RangeCategory.Long:
				return this.accuracyLong;
			default:
				throw new InvalidOperationException();
			}
		}

		// Token: 0x0600054F RID: 1359 RVA: 0x0001ABBE File Offset: 0x00018DBE
		public float AdjustedFullCycleTime(Verb ownerVerb, Pawn attacker)
		{
			return this.warmupTime + this.AdjustedCooldown(ownerVerb, attacker) + ((this.burstShotCount - 1) * this.ticksBetweenBurstShots).TicksToSeconds();
		}

		// Token: 0x06000550 RID: 1360 RVA: 0x0001ABE4 File Offset: 0x00018DE4
		public float GetDamageFactorFor(Verb ownerVerb, Pawn attacker)
		{
			if (ownerVerb.verbProps != this)
			{
				Log.ErrorOnce("Tried to calculate damage factor for a verb with different verb props. verb=" + ownerVerb, 94324562, false);
				return 1f;
			}
			return this.GetDamageFactorFor(ownerVerb.tool, attacker, ownerVerb.HediffCompSource);
		}

		// Token: 0x06000551 RID: 1361 RVA: 0x0001AC20 File Offset: 0x00018E20
		public float GetDamageFactorFor(Tool tool, Pawn attacker, HediffComp_VerbGiver hediffCompSource)
		{
			float num = 1f;
			if (attacker != null)
			{
				if (hediffCompSource != null)
				{
					num *= PawnCapacityUtility.CalculatePartEfficiency(hediffCompSource.Pawn.health.hediffSet, hediffCompSource.parent.Part, true, null);
				}
				else if (attacker != null && this.AdjustedLinkedBodyPartsGroup(tool) != null)
				{
					float num2 = PawnCapacityUtility.CalculateNaturalPartsAverageEfficiency(attacker.health.hediffSet, this.AdjustedLinkedBodyPartsGroup(tool));
					if (this.AdjustedEnsureLinkedBodyPartsGroupAlwaysUsable(tool))
					{
						num2 = Mathf.Max(num2, 0.4f);
					}
					num *= num2;
				}
				if (attacker != null && this.IsMeleeAttack)
				{
					num *= attacker.ageTracker.CurLifeStage.meleeDamageFactor;
				}
			}
			return num;
		}

		// Token: 0x06000552 RID: 1362 RVA: 0x0001ACBF File Offset: 0x00018EBF
		public BodyPartGroupDef AdjustedLinkedBodyPartsGroup(Tool tool)
		{
			if (tool != null)
			{
				return tool.linkedBodyPartsGroup;
			}
			return this.linkedBodyPartsGroup;
		}

		// Token: 0x06000553 RID: 1363 RVA: 0x0001ACD1 File Offset: 0x00018ED1
		public bool AdjustedEnsureLinkedBodyPartsGroupAlwaysUsable(Tool tool)
		{
			if (tool != null)
			{
				return tool.ensureLinkedBodyPartsGroupAlwaysUsable;
			}
			return this.ensureLinkedBodyPartsGroupAlwaysUsable;
		}

		// Token: 0x06000554 RID: 1364 RVA: 0x0001ACE3 File Offset: 0x00018EE3
		public float EffectiveMinRange(LocalTargetInfo target, Thing caster)
		{
			return this.EffectiveMinRange(VerbUtility.AllowAdjacentShot(target, caster));
		}

		// Token: 0x06000555 RID: 1365 RVA: 0x0001ACF4 File Offset: 0x00018EF4
		public float EffectiveMinRange(bool allowAdjacentShot)
		{
			float num = this.minRange;
			if (!allowAdjacentShot && !this.IsMeleeAttack && this.LaunchesProjectile)
			{
				num = Mathf.Max(num, 1.421f);
			}
			return num;
		}

		// Token: 0x06000556 RID: 1366 RVA: 0x0001AD28 File Offset: 0x00018F28
		public float GetHitChanceFactor(Thing equipment, float dist)
		{
			float value;
			if (dist <= 3f)
			{
				value = this.AdjustedAccuracy(VerbProperties.RangeCategory.Touch, equipment);
			}
			else if (dist <= 12f)
			{
				value = Mathf.Lerp(this.AdjustedAccuracy(VerbProperties.RangeCategory.Touch, equipment), this.AdjustedAccuracy(VerbProperties.RangeCategory.Short, equipment), (dist - 3f) / 9f);
			}
			else if (dist <= 25f)
			{
				value = Mathf.Lerp(this.AdjustedAccuracy(VerbProperties.RangeCategory.Short, equipment), this.AdjustedAccuracy(VerbProperties.RangeCategory.Medium, equipment), (dist - 12f) / 13f);
			}
			else if (dist <= 40f)
			{
				value = Mathf.Lerp(this.AdjustedAccuracy(VerbProperties.RangeCategory.Medium, equipment), this.AdjustedAccuracy(VerbProperties.RangeCategory.Long, equipment), (dist - 25f) / 15f);
			}
			else
			{
				value = this.AdjustedAccuracy(VerbProperties.RangeCategory.Long, equipment);
			}
			return Mathf.Clamp(value, 0.01f, 1f);
		}

		// Token: 0x06000557 RID: 1367 RVA: 0x0001ADF0 File Offset: 0x00018FF0
		public void DrawRadiusRing(IntVec3 center)
		{
			if (Find.CurrentMap == null)
			{
				return;
			}
			if (!this.IsMeleeAttack)
			{
				float num = this.EffectiveMinRange(true);
				if (num > 0f && num < GenRadial.MaxRadialPatternRadius)
				{
					GenDraw.DrawRadiusRing(center, num);
				}
				if (this.range < (float)(Find.CurrentMap.Size.x + Find.CurrentMap.Size.z) && this.range < GenRadial.MaxRadialPatternRadius)
				{
					GenDraw.DrawRadiusRing(center, this.range);
				}
			}
		}

		// Token: 0x06000558 RID: 1368 RVA: 0x0001AE70 File Offset: 0x00019070
		public override string ToString()
		{
			string str;
			if (!this.label.NullOrEmpty())
			{
				str = this.label;
			}
			else
			{
				str = string.Concat(new object[]
				{
					"range=",
					this.range,
					", defaultProjectile=",
					this.defaultProjectile.ToStringSafe<ThingDef>()
				});
			}
			return "VerbProperties(" + str + ")";
		}

		// Token: 0x06000559 RID: 1369 RVA: 0x0001AEDB File Offset: 0x000190DB
		public new VerbProperties MemberwiseClone()
		{
			return (VerbProperties)base.MemberwiseClone();
		}

		// Token: 0x0600055A RID: 1370 RVA: 0x0001AEE8 File Offset: 0x000190E8
		public IEnumerable<string> ConfigErrors(ThingDef parent)
		{
			if (parent.race != null && this.linkedBodyPartsGroup != null && !parent.race.body.AllParts.Any((BodyPartRecord part) => part.groups.Contains(this.linkedBodyPartsGroup)))
			{
				yield return string.Concat(new object[]
				{
					"has verb with linkedBodyPartsGroup ",
					this.linkedBodyPartsGroup,
					" but body ",
					parent.race.body,
					" has no parts with that group."
				});
			}
			if (this.LaunchesProjectile && this.defaultProjectile != null && this.forcedMissRadius > 0f != this.CausesExplosion)
			{
				yield return "has incorrect forcedMiss settings; explosive projectiles and only explosive projectiles should have forced miss enabled";
			}
			yield break;
		}

		// Token: 0x0600055B RID: 1371 RVA: 0x0001AEFF File Offset: 0x000190FF
		public void PostLoad()
		{
			this.untranslatedLabel = this.label;
		}

		// Token: 0x0400033E RID: 830
		public VerbCategory category = VerbCategory.Misc;

		// Token: 0x0400033F RID: 831
		[TranslationHandle]
		public Type verbClass = typeof(Verb);

		// Token: 0x04000340 RID: 832
		[MustTranslate]
		public string label;

		// Token: 0x04000341 RID: 833
		[Unsaved(false)]
		[TranslationHandle(Priority = 100)]
		public string untranslatedLabel;

		// Token: 0x04000342 RID: 834
		public bool isPrimary = true;

		// Token: 0x04000343 RID: 835
		public float minRange;

		// Token: 0x04000344 RID: 836
		public float range = 1.42f;

		// Token: 0x04000345 RID: 837
		public int burstShotCount = 1;

		// Token: 0x04000346 RID: 838
		public int ticksBetweenBurstShots = 15;

		// Token: 0x04000347 RID: 839
		public float noiseRadius = 3f;

		// Token: 0x04000348 RID: 840
		public bool hasStandardCommand;

		// Token: 0x04000349 RID: 841
		public bool targetable = true;

		// Token: 0x0400034A RID: 842
		public TargetingParameters targetParams = new TargetingParameters();

		// Token: 0x0400034B RID: 843
		public bool requireLineOfSight = true;

		// Token: 0x0400034C RID: 844
		public bool mustCastOnOpenGround;

		// Token: 0x0400034D RID: 845
		public bool forceNormalTimeSpeed = true;

		// Token: 0x0400034E RID: 846
		public bool onlyManualCast;

		// Token: 0x0400034F RID: 847
		public bool stopBurstWithoutLos = true;

		// Token: 0x04000350 RID: 848
		public SurpriseAttackProps surpriseAttack;

		// Token: 0x04000351 RID: 849
		public float commonality = 1f;

		// Token: 0x04000352 RID: 850
		public Intelligence minIntelligence;

		// Token: 0x04000353 RID: 851
		public float consumeFuelPerShot;

		// Token: 0x04000354 RID: 852
		public float warmupTime;

		// Token: 0x04000355 RID: 853
		public float defaultCooldownTime;

		// Token: 0x04000356 RID: 854
		public SoundDef soundCast;

		// Token: 0x04000357 RID: 855
		public SoundDef soundCastTail;

		// Token: 0x04000358 RID: 856
		public SoundDef soundAiming;

		// Token: 0x04000359 RID: 857
		public float muzzleFlashScale;

		// Token: 0x0400035A RID: 858
		public ThingDef impactMote;

		// Token: 0x0400035B RID: 859
		public BodyPartGroupDef linkedBodyPartsGroup;

		// Token: 0x0400035C RID: 860
		public bool ensureLinkedBodyPartsGroupAlwaysUsable;

		// Token: 0x0400035D RID: 861
		public DamageDef meleeDamageDef;

		// Token: 0x0400035E RID: 862
		public int meleeDamageBaseAmount = 1;

		// Token: 0x0400035F RID: 863
		public float meleeArmorPenetrationBase = -1f;

		// Token: 0x04000360 RID: 864
		public bool ai_IsWeapon = true;

		// Token: 0x04000361 RID: 865
		public bool ai_IsBuildingDestroyer;

		// Token: 0x04000362 RID: 866
		public float ai_AvoidFriendlyFireRadius;

		// Token: 0x04000363 RID: 867
		public ThingDef defaultProjectile;

		// Token: 0x04000364 RID: 868
		public float forcedMissRadius;

		// Token: 0x04000365 RID: 869
		public float accuracyTouch = 1f;

		// Token: 0x04000366 RID: 870
		public float accuracyShort = 1f;

		// Token: 0x04000367 RID: 871
		public float accuracyMedium = 1f;

		// Token: 0x04000368 RID: 872
		public float accuracyLong = 1f;

		// Token: 0x04000369 RID: 873
		public ThingDef spawnDef;

		// Token: 0x0400036A RID: 874
		public TaleDef colonyWideTaleDef;

		// Token: 0x0400036B RID: 875
		public BodyPartTagDef bodypartTagTarget;

		// Token: 0x0400036C RID: 876
		public RulePackDef rangedFireRulepack;

		// Token: 0x0400036D RID: 877
		public const float DefaultArmorPenetrationPerDamage = 0.015f;

		// Token: 0x0400036E RID: 878
		private const float VerbSelectionWeightFactor_BodyPart = 0.3f;

		// Token: 0x0400036F RID: 879
		private const float MinLinkedBodyPartGroupEfficiencyIfMustBeAlwaysUsable = 0.4f;

		// Token: 0x0200133F RID: 4927
		private enum RangeCategory : byte
		{
			// Token: 0x040048E7 RID: 18663
			Touch,
			// Token: 0x040048E8 RID: 18664
			Short,
			// Token: 0x040048E9 RID: 18665
			Medium,
			// Token: 0x040048EA RID: 18666
			Long
		}
	}
}
