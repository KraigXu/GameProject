using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	
	public class RaceProperties
	{
		
		
		public bool Humanlike
		{
			get
			{
				return this.intelligence >= Intelligence.Humanlike;
			}
		}

		
		
		public bool ToolUser
		{
			get
			{
				return this.intelligence >= Intelligence.ToolUser;
			}
		}

		
		
		public bool Animal
		{
			get
			{
				return !this.ToolUser && this.IsFlesh;
			}
		}

		
		
		public bool EatsFood
		{
			get
			{
				return this.foodType > FoodTypeFlags.None;
			}
		}

		
		
		public float FoodLevelPercentageWantEat
		{
			get
			{
				switch (this.ResolvedDietCategory)
				{
				case DietCategory.NeverEats:
					return 0.3f;
				case DietCategory.Herbivorous:
					return 0.45f;
				case DietCategory.Dendrovorous:
					return 0.45f;
				case DietCategory.Ovivorous:
					return 0.4f;
				case DietCategory.Omnivorous:
					return 0.3f;
				case DietCategory.Carnivorous:
					return 0.3f;
				default:
					throw new InvalidOperationException();
				}
			}
		}

		
		
		public DietCategory ResolvedDietCategory
		{
			get
			{
				if (!this.EatsFood)
				{
					return DietCategory.NeverEats;
				}
				if (this.Eats(FoodTypeFlags.Tree))
				{
					return DietCategory.Dendrovorous;
				}
				if (this.Eats(FoodTypeFlags.Meat))
				{
					if (this.Eats(FoodTypeFlags.VegetableOrFruit) || this.Eats(FoodTypeFlags.Plant))
					{
						return DietCategory.Omnivorous;
					}
					return DietCategory.Carnivorous;
				}
				else
				{
					if (this.Eats(FoodTypeFlags.AnimalProduct))
					{
						return DietCategory.Ovivorous;
					}
					return DietCategory.Herbivorous;
				}
			}
		}

		
		
		public DeathActionWorker DeathActionWorker
		{
			get
			{
				if (this.deathActionWorkerInt == null)
				{
					if (this.deathActionWorkerClass != null)
					{
						this.deathActionWorkerInt = (DeathActionWorker)Activator.CreateInstance(this.deathActionWorkerClass);
					}
					else
					{
						this.deathActionWorkerInt = new DeathActionWorker_Simple();
					}
				}
				return this.deathActionWorkerInt;
			}
		}

		
		
		public FleshTypeDef FleshType
		{
			get
			{
				if (this.fleshType != null)
				{
					return this.fleshType;
				}
				return FleshTypeDefOf.Normal;
			}
		}

		
		
		public bool IsMechanoid
		{
			get
			{
				return this.FleshType == FleshTypeDefOf.Mechanoid;
			}
		}

		
		
		public bool IsFlesh
		{
			get
			{
				return this.FleshType != FleshTypeDefOf.Mechanoid;
			}
		}

		
		
		public ThingDef BloodDef
		{
			get
			{
				if (this.bloodDef != null)
				{
					return this.bloodDef;
				}
				if (this.IsFlesh)
				{
					return ThingDefOf.Filth_Blood;
				}
				return null;
			}
		}

		
		
		public bool CanDoHerdMigration
		{
			get
			{
				return this.Animal && this.herdMigrationAllowed;
			}
		}

		
		
		public PawnKindDef AnyPawnKind
		{
			get
			{
				if (this.cachedAnyPawnKind == null)
				{
					List<PawnKindDef> allDefsListForReading = DefDatabase<PawnKindDef>.AllDefsListForReading;
					for (int i = 0; i < allDefsListForReading.Count; i++)
					{
						if (allDefsListForReading[i].race.race == this)
						{
							this.cachedAnyPawnKind = allDefsListForReading[i];
							break;
						}
					}
				}
				return this.cachedAnyPawnKind;
			}
		}

		
		public RulePackDef GetNameGenerator(Gender gender)
		{
			if (gender == Gender.Female && this.nameGeneratorFemale != null)
			{
				return this.nameGeneratorFemale;
			}
			return this.nameGenerator;
		}

		
		public bool CanEverEat(Thing t)
		{
			return this.CanEverEat(t.def);
		}

		
		public bool CanEverEat(ThingDef t)
		{
			return this.EatsFood && t.ingestible != null && t.ingestible.preferability != FoodPreferability.Undefined && (this.willNeverEat == null || !this.willNeverEat.Contains(t)) && this.Eats(t.ingestible.foodType);
		}

		
		public bool Eats(FoodTypeFlags food)
		{
			return this.EatsFood && (this.foodType & food) > FoodTypeFlags.None;
		}

		
		public void ResolveReferencesSpecial()
		{
			if (this.useMeatFrom != null)
			{
				this.meatDef = this.useMeatFrom.race.meatDef;
			}
			if (this.useLeatherFrom != null)
			{
				this.leatherDef = this.useLeatherFrom.race.leatherDef;
			}
		}

		
		public IEnumerable<string> ConfigErrors()
		{
			if (this.soundMeleeHitPawn == null)
			{
				yield return "soundMeleeHitPawn is null";
			}
			if (this.soundMeleeHitBuilding == null)
			{
				yield return "soundMeleeHitBuilding is null";
			}
			if (this.soundMeleeMiss == null)
			{
				yield return "soundMeleeMiss is null";
			}
			if (this.predator && !this.Eats(FoodTypeFlags.Meat))
			{
				yield return "predator but doesn't eat meat";
			}
			int num;
			for (int i = 0; i < this.lifeStageAges.Count; i = num + 1)
			{
				for (int j = 0; j < i; j = num + 1)
				{
					if (this.lifeStageAges[j].minAge > this.lifeStageAges[i].minAge)
					{
						yield return "lifeStages minAges are not in ascending order";
					}
					num = j;
				}
				num = i;
			}
			if (this.litterSizeCurve != null)
			{
				foreach (string text in this.litterSizeCurve.ConfigErrors("litterSizeCurve"))
				{
					
				}
				IEnumerator<string> enumerator = null;
			}
			if (this.nameOnTameChance > 0f && this.nameGenerator == null)
			{
				yield return "can be named, but has no nameGenerator";
			}
			if (this.Animal && this.wildness < 0f)
			{
				yield return "is animal but wildness is not defined";
			}
			if (this.useMeatFrom != null && this.useMeatFrom.category != ThingCategory.Pawn)
			{
				yield return "tries to use meat from non-pawn " + this.useMeatFrom;
			}
			if (this.useMeatFrom != null && this.useMeatFrom.race.useMeatFrom != null)
			{
				yield return string.Concat(new object[]
				{
					"tries to use meat from ",
					this.useMeatFrom,
					" which uses meat from ",
					this.useMeatFrom.race.useMeatFrom
				});
			}
			if (this.useLeatherFrom != null && this.useLeatherFrom.category != ThingCategory.Pawn)
			{
				yield return "tries to use leather from non-pawn " + this.useLeatherFrom;
			}
			if (this.useLeatherFrom != null && this.useLeatherFrom.race.useLeatherFrom != null)
			{
				yield return string.Concat(new object[]
				{
					"tries to use leather from ",
					this.useLeatherFrom,
					" which uses leather from ",
					this.useLeatherFrom.race.useLeatherFrom
				});
			}
			if (this.Animal && this.trainability == null)
			{
				yield return "animal has trainability = null";
			}
			yield break;
			yield break;
		}

		
		public IEnumerable<StatDrawEntry> SpecialDisplayStats(ThingDef parentDef, StatRequest req)
		{
			yield return new StatDrawEntry(StatCategoryDefOf.BasicsPawn, "Race".Translate(), parentDef.LabelCap, parentDef.description, 2100, null, null, false);
			if (!parentDef.race.IsMechanoid)
			{
				string text = this.foodType.ToHumanString().CapitalizeFirst();
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsPawn, "Diet".Translate(), text, "Stat_Race_Diet_Desc".Translate(text), 1500, null, null, false);
			}
			if (req.HasThing && req.Thing is Pawn && (req.Thing as Pawn).needs != null && (req.Thing as Pawn).needs.food != null)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsPawn, "HungerRate".Translate(), ((req.Thing as Pawn).needs.food.FoodFallPerTickAssumingCategory(HungerCategory.Fed, false) * 60000f).ToString("0.##"), RaceProperties.NutritionEatenPerDayExplanation_NewTemp(req.Thing as Pawn, false, false, true), 1600, null, null, false);
			}
			if (parentDef.race.leatherDef != null)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsPawn, "LeatherType".Translate(), parentDef.race.leatherDef.LabelCap, "Stat_Race_LeatherType_Desc".Translate(), 3550, null, new Dialog_InfoCard.Hyperlink[]
				{
					new Dialog_InfoCard.Hyperlink(parentDef.race.leatherDef, -1)
				}, false);
			}
			if (parentDef.race.Animal || this.wildness > 0f)
			{
				StatDrawEntry statDrawEntry = new StatDrawEntry(StatCategoryDefOf.BasicsPawn, "Wildness".Translate(), this.wildness.ToStringPercent(), TrainableUtility.GetWildnessExplanation(parentDef), 2050, null, null, false);
				yield return statDrawEntry;
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsPawn, "HarmedRevengeChance".Translate(), PawnUtility.GetManhunterOnDamageChance(parentDef.race).ToStringPercent(), "HarmedRevengeChanceExplanation".Translate(), 510, null, null, false);
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsPawn, "TameFailedRevengeChance".Translate(), parentDef.race.manhunterOnTameFailChance.ToStringPercent(), "Stat_Race_Animal_TameFailedRevengeChance_Desc".Translate(), 511, null, null, false);
			}
			if (this.intelligence < Intelligence.Humanlike && this.trainability != null)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsPawn, "Trainability".Translate(), this.trainability.LabelCap, "Stat_Race_Trainability_Desc".Translate(), 2500, null, null, false);
			}
			yield return new StatDrawEntry(StatCategoryDefOf.BasicsPawn, "StatsReport_LifeExpectancy".Translate(), this.lifeExpectancy.ToStringByStyle(ToStringStyle.Integer, ToStringNumberSense.Absolute), "Stat_Race_LifeExpectancy_Desc".Translate(), 2000, null, null, false);
			if (this.intelligence < Intelligence.Humanlike && !parentDef.race.IsMechanoid)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsPawn, "AnimalFilthRate".Translate(), (PawnUtility.AnimalFilthChancePerCell(parentDef, parentDef.race.baseBodySize) * 1000f).ToString("F2"), "AnimalFilthRateExplanation".Translate(1000.ToString()), 2203, null, null, false);
			}
			if (parentDef.race.Animal)
			{
				StatDrawEntry statDrawEntry2 = new StatDrawEntry(StatCategoryDefOf.BasicsPawn, "PackAnimal".Translate(), this.packAnimal ? "Yes".Translate() : "No".Translate(), "PackAnimalExplanation".Translate(), 2202, null, null, false);
				yield return statDrawEntry2;
				if (parentDef.race.nuzzleMtbHours > 0f)
				{
					StatDrawEntry statDrawEntry3 = new StatDrawEntry(StatCategoryDefOf.PawnSocial, "NuzzleInterval".Translate(), Mathf.RoundToInt(parentDef.race.nuzzleMtbHours * 2500f).ToStringTicksToPeriod(true, false, true, true), "NuzzleIntervalExplanation".Translate(), 500, null, null, false);
					yield return statDrawEntry3;
				}
			}
			yield break;
		}

		
		[Obsolete("Will be replaced with NutritionEatenPerDayExplanation_NewTemp soon.")]
		public static string NutritionEatenPerDayExplanation(Pawn p)
		{
			return RaceProperties.NutritionEatenPerDayExplanation_NewTemp(p, true, true, false);
		}

		
		public static string NutritionEatenPerDayExplanation_NewTemp(Pawn p, bool showDiet = false, bool showLegend = false, bool showCalculations = true)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("NutritionEatenPerDayTip".Translate(ThingDefOf.MealSimple.GetStatValueAbstract(StatDefOf.Nutrition, null).ToString("0.##")));
			stringBuilder.AppendLine();
			if (showDiet)
			{
				stringBuilder.AppendLine("CanEat".Translate() + ": " + p.RaceProps.foodType.ToHumanString());
				stringBuilder.AppendLine();
			}
			if (showLegend)
			{
				stringBuilder.AppendLine("Legend".Translate() + ":");
				stringBuilder.AppendLine("NoDietCategoryLetter".Translate() + " - " + DietCategory.Omnivorous.ToStringHuman());
				DietCategory[] array = (DietCategory[])Enum.GetValues(typeof(DietCategory));
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i] != DietCategory.NeverEats && array[i] != DietCategory.Omnivorous)
					{
						stringBuilder.AppendLine(array[i].ToStringHumanShort() + " - " + array[i].ToStringHuman());
					}
				}
				stringBuilder.AppendLine();
			}
			if (showCalculations)
			{
				stringBuilder.AppendLine("StatsReport_BaseValue".Translate() + ": " + (p.ageTracker.CurLifeStage.hungerRateFactor * p.RaceProps.baseHungerRate * 2.66666666E-05f * 60000f).ToStringByStyle(ToStringStyle.FloatTwo, ToStringNumberSense.Absolute));
				if (p.health.hediffSet.HungerRateFactor != 1f)
				{
					stringBuilder.AppendLine();
					stringBuilder.AppendLine("StatsReport_RelevantHediffs".Translate() + ": " + p.health.hediffSet.HungerRateFactor.ToStringByStyle(ToStringStyle.PercentOne, ToStringNumberSense.Factor));
					foreach (Hediff hediff in p.health.hediffSet.hediffs)
					{
						if (hediff.CurStage != null && hediff.CurStage.hungerRateFactor != 1f)
						{
							stringBuilder.AppendLine("    " + hediff.LabelCap + ": " + hediff.CurStage.hungerRateFactor.ToStringByStyle(ToStringStyle.PercentOne, ToStringNumberSense.Factor));
						}
					}
					foreach (Hediff hediff2 in p.health.hediffSet.hediffs)
					{
						if (hediff2.CurStage != null && hediff2.CurStage.hungerRateFactorOffset != 0f)
						{
							stringBuilder.AppendLine("    " + hediff2.LabelCap + ": +" + hediff2.CurStage.hungerRateFactorOffset.ToStringByStyle(ToStringStyle.FloatMaxOne, ToStringNumberSense.Factor));
						}
					}
				}
				if (p.story != null && p.story.traits != null && p.story.traits.HungerRateFactor != 1f)
				{
					stringBuilder.AppendLine();
					stringBuilder.AppendLine("StatsReport_RelevantTraits".Translate() + ": " + p.story.traits.HungerRateFactor.ToStringByStyle(ToStringStyle.PercentOne, ToStringNumberSense.Factor));
					foreach (Trait trait in p.story.traits.allTraits)
					{
						if (trait.CurrentData.hungerRateFactor != 1f)
						{
							stringBuilder.AppendLine("    " + trait.LabelCap + ": " + trait.CurrentData.hungerRateFactor.ToStringByStyle(ToStringStyle.PercentOne, ToStringNumberSense.Factor));
						}
					}
				}
				if (p.GetStatValue(StatDefOf.HungerRateMultiplier, true) != 1f)
				{
					stringBuilder.AppendLine();
					stringBuilder.AppendLine(StatDefOf.HungerRateMultiplier.LabelCap + ": " + p.GetStatValue(StatDefOf.HungerRateMultiplier, true).ToStringByStyle(ToStringStyle.FloatMaxOne, ToStringNumberSense.Factor));
				}
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("StatsReport_FinalValue".Translate() + ": " + (p.needs.food.FoodFallPerTickAssumingCategory(HungerCategory.Fed, false) * 60000f).ToStringByStyle(ToStringStyle.FloatMaxTwo, ToStringNumberSense.Absolute));
			}
			return stringBuilder.ToString().TrimEndNewlines();
		}

		
		public Intelligence intelligence;

		
		private FleshTypeDef fleshType;

		
		private ThingDef bloodDef;

		
		public bool hasGenders = true;

		
		public bool needsRest = true;

		
		public ThinkTreeDef thinkTreeMain;

		
		public ThinkTreeDef thinkTreeConstant;

		
		public PawnNameCategory nameCategory;

		
		public FoodTypeFlags foodType;

		
		public BodyDef body;

		
		public Type deathActionWorkerClass;

		
		public List<AnimalBiomeRecord> wildBiomes;

		
		public SimpleCurve ageGenerationCurve;

		
		public bool makesFootprints;

		
		public int executionRange = 2;

		
		public float lifeExpectancy = 10f;

		
		public List<HediffGiverSetDef> hediffGiverSets;

		
		public bool herdAnimal;

		
		public bool packAnimal;

		
		public bool predator;

		
		public float maxPreyBodySize = 99999f;

		
		public float wildness;

		
		public float petness;

		
		public float nuzzleMtbHours = -1f;

		
		public float manhunterOnDamageChance;

		
		public float manhunterOnTameFailChance;

		
		public bool canBePredatorPrey = true;

		
		public bool herdMigrationAllowed = true;

		
		public List<ThingDef> willNeverEat;

		
		public float gestationPeriodDays = 10f;

		
		public SimpleCurve litterSizeCurve;

		
		public float mateMtbHours = 12f;

		
		[NoTranslate]
		public List<string> untrainableTags;

		
		[NoTranslate]
		public List<string> trainableTags;

		
		public TrainabilityDef trainability;

		
		private RulePackDef nameGenerator;

		
		private RulePackDef nameGeneratorFemale;

		
		public float nameOnTameChance;

		
		public float nameOnNuzzleChance;

		
		public float baseBodySize = 1f;

		
		public float baseHealthScale = 1f;

		
		public float baseHungerRate = 1f;

		
		public List<LifeStageAge> lifeStageAges = new List<LifeStageAge>();

		
		[MustTranslate]
		public string meatLabel;

		
		public Color meatColor = Color.white;

		
		public float meatMarketValue = 2f;

		
		public ThingDef useMeatFrom;

		
		public ThingDef useLeatherFrom;

		
		public ThingDef leatherDef;

		
		public ShadowData specialShadowData;

		
		public IntRange soundCallIntervalRange = new IntRange(2000, 4000);

		
		public SoundDef soundMeleeHitPawn;

		
		public SoundDef soundMeleeHitBuilding;

		
		public SoundDef soundMeleeMiss;

		
		public SoundDef soundMeleeDodge;

		
		[Unsaved(false)]
		private DeathActionWorker deathActionWorkerInt;

		
		[Unsaved(false)]
		public ThingDef meatDef;

		
		[Unsaved(false)]
		public ThingDef corpseDef;

		
		[Unsaved(false)]
		private PawnKindDef cachedAnyPawnKind;
	}
}
