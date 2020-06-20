using System;

namespace Verse
{
	// Token: 0x0200022F RID: 559
	public class HealthTuning
	{
		// Token: 0x04000B7C RID: 2940
		public const int StandardInterval = 60;

		// Token: 0x04000B7D RID: 2941
		public const float SmallPawnFragmentedDamageHealthScaleThreshold = 0.5f;

		// Token: 0x04000B7E RID: 2942
		public const int SmallPawnFragmentedDamageMinimumDamageAmount = 10;

		// Token: 0x04000B7F RID: 2943
		public static float ChanceToAdditionallyDamageInnerSolidPart = 0.2f;

		// Token: 0x04000B80 RID: 2944
		public const float MinBleedingRateToBleed = 0.1f;

		// Token: 0x04000B81 RID: 2945
		public const float BleedSeverityRecoveryPerInterval = 0.00033333333f;

		// Token: 0x04000B82 RID: 2946
		public const float BloodFilthDropChanceFactorStanding = 0.004f;

		// Token: 0x04000B83 RID: 2947
		public const float BloodFilthDropChanceFactorLaying = 0.0004f;

		// Token: 0x04000B84 RID: 2948
		public const int BaseTicksAfterInjuryToStopBleeding = 90000;

		// Token: 0x04000B85 RID: 2949
		public const int TicksAfterMissingBodyPartToStopBeingFresh = 90000;

		// Token: 0x04000B86 RID: 2950
		public const float DefaultPainShockThreshold = 0.8f;

		// Token: 0x04000B87 RID: 2951
		public const int InjuryHealInterval = 600;

		// Token: 0x04000B88 RID: 2952
		public const float InjuryHealPerDay_Base = 8f;

		// Token: 0x04000B89 RID: 2953
		public const float InjuryHealPerDayOffset_Laying = 4f;

		// Token: 0x04000B8A RID: 2954
		public const float InjuryHealPerDayOffset_Tended = 8f;

		// Token: 0x04000B8B RID: 2955
		public const int InjurySeverityTendedPerMedicine = 20;

		// Token: 0x04000B8C RID: 2956
		public const float BaseTotalDamageLethalThreshold = 150f;

		// Token: 0x04000B8D RID: 2957
		public const float BecomePermanentBaseChance = 0.02f;

		// Token: 0x04000B8E RID: 2958
		public static readonly SimpleCurve BecomePermanentChanceFactorBySeverityCurve = new SimpleCurve
		{
			{
				new CurvePoint(4f, 0f),
				true
			},
			{
				new CurvePoint(14f, 1f),
				true
			}
		};

		// Token: 0x04000B8F RID: 2959
		public static readonly HealthTuning.PainCategoryWeighted[] InjuryPainCategories = new HealthTuning.PainCategoryWeighted[]
		{
			new HealthTuning.PainCategoryWeighted(PainCategory.Painless, 0.5f),
			new HealthTuning.PainCategoryWeighted(PainCategory.LowPain, 0.2f),
			new HealthTuning.PainCategoryWeighted(PainCategory.MediumPain, 0.2f),
			new HealthTuning.PainCategoryWeighted(PainCategory.HighPain, 0.1f)
		};

		// Token: 0x04000B90 RID: 2960
		public const float MinDamagePartPctForInfection = 0.2f;

		// Token: 0x04000B91 RID: 2961
		public static readonly IntRange InfectionDelayRange = new IntRange(15000, 45000);

		// Token: 0x04000B92 RID: 2962
		public const float AnimalsInfectionChanceFactor = 0.1f;

		// Token: 0x04000B93 RID: 2963
		public const float HypothermiaGrowthPerDegreeUnder = 6.45E-05f;

		// Token: 0x04000B94 RID: 2964
		public const float HeatstrokeGrowthPerDegreeOver = 6.45E-05f;

		// Token: 0x04000B95 RID: 2965
		public const float MinHeatstrokeProgressPerInterval = 0.000375f;

		// Token: 0x04000B96 RID: 2966
		public const float MinHypothermiaProgress = 0.00075f;

		// Token: 0x04000B97 RID: 2967
		public const float HarmfulTemperatureOffset = 10f;

		// Token: 0x04000B98 RID: 2968
		public const float MinTempOverComfyMaxForBurn = 150f;

		// Token: 0x04000B99 RID: 2969
		public const float BurnDamagePerTempOverage = 0.06f;

		// Token: 0x04000B9A RID: 2970
		public const int MinBurnDamage = 3;

		// Token: 0x04000B9B RID: 2971
		public const float ImmunityGainRandomFactorMin = 0.8f;

		// Token: 0x04000B9C RID: 2972
		public const float ImmunityGainRandomFactorMax = 1.2f;

		// Token: 0x04000B9D RID: 2973
		public const float ImpossibleToFallSickIfAboveThisImmunityLevel = 0.6f;

		// Token: 0x04000B9E RID: 2974
		public const int HediffGiverUpdateInterval = 60;

		// Token: 0x04000B9F RID: 2975
		public const int VomitCheckInterval = 600;

		// Token: 0x04000BA0 RID: 2976
		public const int DeathCheckInterval = 200;

		// Token: 0x04000BA1 RID: 2977
		public const int ForgetRandomMemoryThoughtCheckInterval = 400;

		// Token: 0x04000BA2 RID: 2978
		public const float PawnBaseHealthForSummary = 75f;

		// Token: 0x04000BA3 RID: 2979
		public const float DeathOnDownedChance_NonColonyAnimal = 0.5f;

		// Token: 0x04000BA4 RID: 2980
		public const float DeathOnDownedChance_NonColonyMechanoid = 1f;

		// Token: 0x04000BA5 RID: 2981
		public static readonly SimpleCurve DeathOnDownedChance_NonColonyHumanlikeFromPopulationIntentCurve = new SimpleCurve
		{
			{
				new CurvePoint(-1f, 0.92f),
				true
			},
			{
				new CurvePoint(0f, 0.85f),
				true
			},
			{
				new CurvePoint(1f, 0.62f),
				true
			},
			{
				new CurvePoint(2f, 0.55f),
				true
			},
			{
				new CurvePoint(8f, 0.3f),
				true
			}
		};

		// Token: 0x04000BA6 RID: 2982
		public const float TendPriority_LifeThreateningDisease = 1f;

		// Token: 0x04000BA7 RID: 2983
		public const float TendPriority_PerBleedRate = 1.5f;

		// Token: 0x04000BA8 RID: 2984
		public const float TendPriority_DiseaseSeverityDecreasesWhenTended = 0.025f;

		// Token: 0x0200142E RID: 5166
		public struct PainCategoryWeighted
		{
			// Token: 0x0600795F RID: 31071 RVA: 0x00295D04 File Offset: 0x00293F04
			public PainCategoryWeighted(PainCategory category, float weight)
			{
				this.category = category;
				this.weight = weight;
			}

			// Token: 0x04004C9C RID: 19612
			public PainCategory category;

			// Token: 0x04004C9D RID: 19613
			public float weight;
		}
	}
}
