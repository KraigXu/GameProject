using System;

namespace RimWorld
{
	// Token: 0x02000F7B RID: 3963
	[DefOf]
	public static class StatDefOf
	{
		// Token: 0x06006082 RID: 24706 RVA: 0x00217002 File Offset: 0x00215202
		static StatDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(StatDefOf));
		}

		// Token: 0x04003889 RID: 14473
		public static StatDef MaxHitPoints;

		// Token: 0x0400388A RID: 14474
		public static StatDef MarketValue;

		// Token: 0x0400388B RID: 14475
		public static StatDef RoyalFavorValue;

		// Token: 0x0400388C RID: 14476
		public static StatDef SellPriceFactor;

		// Token: 0x0400388D RID: 14477
		public static StatDef Beauty;

		// Token: 0x0400388E RID: 14478
		public static StatDef Cleanliness;

		// Token: 0x0400388F RID: 14479
		public static StatDef Flammability;

		// Token: 0x04003890 RID: 14480
		public static StatDef DeteriorationRate;

		// Token: 0x04003891 RID: 14481
		public static StatDef WorkToMake;

		// Token: 0x04003892 RID: 14482
		public static StatDef WorkToBuild;

		// Token: 0x04003893 RID: 14483
		public static StatDef Mass;

		// Token: 0x04003894 RID: 14484
		public static StatDef ConstructionSpeedFactor;

		// Token: 0x04003895 RID: 14485
		public static StatDef Nutrition;

		// Token: 0x04003896 RID: 14486
		public static StatDef FoodPoisonChanceFixedHuman;

		// Token: 0x04003897 RID: 14487
		public static StatDef MoveSpeed;

		// Token: 0x04003898 RID: 14488
		public static StatDef GlobalLearningFactor;

		// Token: 0x04003899 RID: 14489
		public static StatDef HungerRateMultiplier;

		// Token: 0x0400389A RID: 14490
		public static StatDef RestRateMultiplier;

		// Token: 0x0400389B RID: 14491
		public static StatDef PsychicSensitivity;

		// Token: 0x0400389C RID: 14492
		public static StatDef ToxicSensitivity;

		// Token: 0x0400389D RID: 14493
		public static StatDef MentalBreakThreshold;

		// Token: 0x0400389E RID: 14494
		public static StatDef EatingSpeed;

		// Token: 0x0400389F RID: 14495
		public static StatDef ComfyTemperatureMin;

		// Token: 0x040038A0 RID: 14496
		public static StatDef ComfyTemperatureMax;

		// Token: 0x040038A1 RID: 14497
		public static StatDef Comfort;

		// Token: 0x040038A2 RID: 14498
		public static StatDef MeatAmount;

		// Token: 0x040038A3 RID: 14499
		public static StatDef LeatherAmount;

		// Token: 0x040038A4 RID: 14500
		public static StatDef MinimumHandlingSkill;

		// Token: 0x040038A5 RID: 14501
		public static StatDef MeleeDPS;

		// Token: 0x040038A6 RID: 14502
		public static StatDef PainShockThreshold;

		// Token: 0x040038A7 RID: 14503
		public static StatDef ForagedNutritionPerDay;

		// Token: 0x040038A8 RID: 14504
		[MayRequireRoyalty]
		public static StatDef PsychicEntropyMax;

		// Token: 0x040038A9 RID: 14505
		[MayRequireRoyalty]
		public static StatDef PsychicEntropyRecoveryRate;

		// Token: 0x040038AA RID: 14506
		[MayRequireRoyalty]
		public static StatDef PsychicEntropyGain;

		// Token: 0x040038AB RID: 14507
		[MayRequireRoyalty]
		public static StatDef MeditationFocusGain;

		// Token: 0x040038AC RID: 14508
		public static StatDef WorkSpeedGlobal;

		// Token: 0x040038AD RID: 14509
		public static StatDef MiningSpeed;

		// Token: 0x040038AE RID: 14510
		public static StatDef DeepDrillingSpeed;

		// Token: 0x040038AF RID: 14511
		public static StatDef MiningYield;

		// Token: 0x040038B0 RID: 14512
		public static StatDef ResearchSpeed;

		// Token: 0x040038B1 RID: 14513
		public static StatDef ConstructionSpeed;

		// Token: 0x040038B2 RID: 14514
		public static StatDef HuntingStealth;

		// Token: 0x040038B3 RID: 14515
		public static StatDef PlantWorkSpeed;

		// Token: 0x040038B4 RID: 14516
		public static StatDef SmoothingSpeed;

		// Token: 0x040038B5 RID: 14517
		public static StatDef FoodPoisonChance;

		// Token: 0x040038B6 RID: 14518
		public static StatDef CarryingCapacity;

		// Token: 0x040038B7 RID: 14519
		public static StatDef PlantHarvestYield;

		// Token: 0x040038B8 RID: 14520
		public static StatDef FixBrokenDownBuildingSuccessChance;

		// Token: 0x040038B9 RID: 14521
		public static StatDef ConstructSuccessChance;

		// Token: 0x040038BA RID: 14522
		public static StatDef GeneralLaborSpeed;

		// Token: 0x040038BB RID: 14523
		[DefAlias("GeneralLaborSpeed")]
		[Obsolete("Use StatDefOf.GeneralLaborSpeed, this field is only here for legacy reasons and will be removed in the future.")]
		public static StatDef UnskilledLaborSpeed;

		// Token: 0x040038BC RID: 14524
		public static StatDef MedicalTendSpeed;

		// Token: 0x040038BD RID: 14525
		public static StatDef MedicalTendQuality;

		// Token: 0x040038BE RID: 14526
		public static StatDef MedicalSurgerySuccessChance;

		// Token: 0x040038BF RID: 14527
		public static StatDef NegotiationAbility;

		// Token: 0x040038C0 RID: 14528
		public static StatDef ArrestSuccessChance;

		// Token: 0x040038C1 RID: 14529
		public static StatDef TradePriceImprovement;

		// Token: 0x040038C2 RID: 14530
		public static StatDef SocialImpact;

		// Token: 0x040038C3 RID: 14531
		public static StatDef PawnBeauty;

		// Token: 0x040038C4 RID: 14532
		public static StatDef AnimalGatherSpeed;

		// Token: 0x040038C5 RID: 14533
		public static StatDef AnimalGatherYield;

		// Token: 0x040038C6 RID: 14534
		public static StatDef TameAnimalChance;

		// Token: 0x040038C7 RID: 14535
		public static StatDef TrainAnimalChance;

		// Token: 0x040038C8 RID: 14536
		public static StatDef ShootingAccuracyPawn;

		// Token: 0x040038C9 RID: 14537
		public static StatDef ShootingAccuracyTurret;

		// Token: 0x040038CA RID: 14538
		public static StatDef AimingDelayFactor;

		// Token: 0x040038CB RID: 14539
		public static StatDef MeleeHitChance;

		// Token: 0x040038CC RID: 14540
		public static StatDef MeleeDodgeChance;

		// Token: 0x040038CD RID: 14541
		public static StatDef PawnTrapSpringChance;

		// Token: 0x040038CE RID: 14542
		public static StatDef IncomingDamageFactor;

		// Token: 0x040038CF RID: 14543
		public static StatDef MeleeWeapon_AverageDPS;

		// Token: 0x040038D0 RID: 14544
		public static StatDef MeleeWeapon_DamageMultiplier;

		// Token: 0x040038D1 RID: 14545
		public static StatDef MeleeWeapon_CooldownMultiplier;

		// Token: 0x040038D2 RID: 14546
		public static StatDef MeleeWeapon_AverageArmorPenetration;

		// Token: 0x040038D3 RID: 14547
		public static StatDef SharpDamageMultiplier;

		// Token: 0x040038D4 RID: 14548
		public static StatDef BluntDamageMultiplier;

		// Token: 0x040038D5 RID: 14549
		public static StatDef StuffPower_Armor_Sharp;

		// Token: 0x040038D6 RID: 14550
		public static StatDef StuffPower_Armor_Blunt;

		// Token: 0x040038D7 RID: 14551
		public static StatDef StuffPower_Armor_Heat;

		// Token: 0x040038D8 RID: 14552
		public static StatDef StuffPower_Insulation_Cold;

		// Token: 0x040038D9 RID: 14553
		public static StatDef StuffPower_Insulation_Heat;

		// Token: 0x040038DA RID: 14554
		public static StatDef RangedWeapon_Cooldown;

		// Token: 0x040038DB RID: 14555
		public static StatDef RangedWeapon_DamageMultiplier;

		// Token: 0x040038DC RID: 14556
		public static StatDef AccuracyTouch;

		// Token: 0x040038DD RID: 14557
		public static StatDef AccuracyShort;

		// Token: 0x040038DE RID: 14558
		public static StatDef AccuracyMedium;

		// Token: 0x040038DF RID: 14559
		public static StatDef AccuracyLong;

		// Token: 0x040038E0 RID: 14560
		public static StatDef StuffEffectMultiplierArmor;

		// Token: 0x040038E1 RID: 14561
		public static StatDef StuffEffectMultiplierInsulation_Cold;

		// Token: 0x040038E2 RID: 14562
		public static StatDef StuffEffectMultiplierInsulation_Heat;

		// Token: 0x040038E3 RID: 14563
		public static StatDef ArmorRating_Sharp;

		// Token: 0x040038E4 RID: 14564
		public static StatDef ArmorRating_Blunt;

		// Token: 0x040038E5 RID: 14565
		public static StatDef ArmorRating_Heat;

		// Token: 0x040038E6 RID: 14566
		public static StatDef Insulation_Cold;

		// Token: 0x040038E7 RID: 14567
		public static StatDef Insulation_Heat;

		// Token: 0x040038E8 RID: 14568
		public static StatDef EnergyShieldRechargeRate;

		// Token: 0x040038E9 RID: 14569
		public static StatDef EnergyShieldEnergyMax;

		// Token: 0x040038EA RID: 14570
		public static StatDef SmokepopBeltRadius;

		// Token: 0x040038EB RID: 14571
		public static StatDef EquipDelay;

		// Token: 0x040038EC RID: 14572
		public static StatDef MedicalPotency;

		// Token: 0x040038ED RID: 14573
		public static StatDef MedicalQualityMax;

		// Token: 0x040038EE RID: 14574
		public static StatDef ImmunityGainSpeed;

		// Token: 0x040038EF RID: 14575
		public static StatDef ImmunityGainSpeedFactor;

		// Token: 0x040038F0 RID: 14576
		public static StatDef DoorOpenSpeed;

		// Token: 0x040038F1 RID: 14577
		public static StatDef BedRestEffectiveness;

		// Token: 0x040038F2 RID: 14578
		public static StatDef TrapMeleeDamage;

		// Token: 0x040038F3 RID: 14579
		public static StatDef TrapSpringChance;

		// Token: 0x040038F4 RID: 14580
		public static StatDef ResearchSpeedFactor;

		// Token: 0x040038F5 RID: 14581
		public static StatDef MedicalTendQualityOffset;

		// Token: 0x040038F6 RID: 14582
		public static StatDef WorkTableWorkSpeedFactor;

		// Token: 0x040038F7 RID: 14583
		public static StatDef WorkTableEfficiencyFactor;

		// Token: 0x040038F8 RID: 14584
		public static StatDef JoyGainFactor;

		// Token: 0x040038F9 RID: 14585
		public static StatDef SurgerySuccessChanceFactor;

		// Token: 0x040038FA RID: 14586
		public static StatDef Ability_CastingTime;

		// Token: 0x040038FB RID: 14587
		public static StatDef Ability_EntropyGain;

		// Token: 0x040038FC RID: 14588
		public static StatDef Ability_PsyfocusCost;

		// Token: 0x040038FD RID: 14589
		public static StatDef Ability_Duration;

		// Token: 0x040038FE RID: 14590
		public static StatDef Ability_Range;

		// Token: 0x040038FF RID: 14591
		public static StatDef Ability_EffectRadius;

		// Token: 0x04003900 RID: 14592
		public static StatDef Ability_RequiredPsylink;

		// Token: 0x04003901 RID: 14593
		public static StatDef Ability_GoodwillImpact;

		// Token: 0x04003902 RID: 14594
		public static StatDef Ability_DetectChancePerEntropy;

		// Token: 0x04003903 RID: 14595
		[Obsolete("Will be removed in the future")]
		public static StatDef Bladelink_DetectionChance;

		// Token: 0x04003904 RID: 14596
		public static StatDef MeditationFocusStrength;
	}
}
