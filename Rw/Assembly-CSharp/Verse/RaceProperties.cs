using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200009B RID: 155
	public class RaceProperties
	{
		// Token: 0x170000DA RID: 218
		// (get) Token: 0x060004F6 RID: 1270 RVA: 0x0001912B File Offset: 0x0001732B
		public bool Humanlike
		{
			get
			{
				return this.intelligence >= Intelligence.Humanlike;
			}
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x060004F7 RID: 1271 RVA: 0x00019139 File Offset: 0x00017339
		public bool ToolUser
		{
			get
			{
				return this.intelligence >= Intelligence.ToolUser;
			}
		}

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x060004F8 RID: 1272 RVA: 0x00019147 File Offset: 0x00017347
		public bool Animal
		{
			get
			{
				return !this.ToolUser && this.IsFlesh;
			}
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x060004F9 RID: 1273 RVA: 0x00019159 File Offset: 0x00017359
		public bool EatsFood
		{
			get
			{
				return this.foodType > FoodTypeFlags.None;
			}
		}

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x060004FA RID: 1274 RVA: 0x00019164 File Offset: 0x00017364
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

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x060004FB RID: 1275 RVA: 0x000191C4 File Offset: 0x000173C4
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

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x060004FC RID: 1276 RVA: 0x00019218 File Offset: 0x00017418
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

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x060004FD RID: 1277 RVA: 0x00019264 File Offset: 0x00017464
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

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x060004FE RID: 1278 RVA: 0x0001927A File Offset: 0x0001747A
		public bool IsMechanoid
		{
			get
			{
				return this.FleshType == FleshTypeDefOf.Mechanoid;
			}
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x060004FF RID: 1279 RVA: 0x00019289 File Offset: 0x00017489
		public bool IsFlesh
		{
			get
			{
				return this.FleshType != FleshTypeDefOf.Mechanoid;
			}
		}

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x06000500 RID: 1280 RVA: 0x0001929B File Offset: 0x0001749B
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

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x06000501 RID: 1281 RVA: 0x000192BB File Offset: 0x000174BB
		public bool CanDoHerdMigration
		{
			get
			{
				return this.Animal && this.herdMigrationAllowed;
			}
		}

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x06000502 RID: 1282 RVA: 0x000192D0 File Offset: 0x000174D0
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

		// Token: 0x06000503 RID: 1283 RVA: 0x00019325 File Offset: 0x00017525
		public RulePackDef GetNameGenerator(Gender gender)
		{
			if (gender == Gender.Female && this.nameGeneratorFemale != null)
			{
				return this.nameGeneratorFemale;
			}
			return this.nameGenerator;
		}

		// Token: 0x06000504 RID: 1284 RVA: 0x00019340 File Offset: 0x00017540
		public bool CanEverEat(Thing t)
		{
			return this.CanEverEat(t.def);
		}

		// Token: 0x06000505 RID: 1285 RVA: 0x00019350 File Offset: 0x00017550
		public bool CanEverEat(ThingDef t)
		{
			return this.EatsFood && t.ingestible != null && t.ingestible.preferability != FoodPreferability.Undefined && (this.willNeverEat == null || !this.willNeverEat.Contains(t)) && this.Eats(t.ingestible.foodType);
		}

		// Token: 0x06000506 RID: 1286 RVA: 0x000193A9 File Offset: 0x000175A9
		public bool Eats(FoodTypeFlags food)
		{
			return this.EatsFood && (this.foodType & food) > FoodTypeFlags.None;
		}

		// Token: 0x06000507 RID: 1287 RVA: 0x000193C0 File Offset: 0x000175C0
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

		// Token: 0x06000508 RID: 1288 RVA: 0x000193FE File Offset: 0x000175FE
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
					yield return text;
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

		// Token: 0x06000509 RID: 1289 RVA: 0x0001940E File Offset: 0x0001760E
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

		// Token: 0x0600050A RID: 1290 RVA: 0x0001942C File Offset: 0x0001762C
		[Obsolete("Will be replaced with NutritionEatenPerDayExplanation_NewTemp soon.")]
		public static string NutritionEatenPerDayExplanation(Pawn p)
		{
			return RaceProperties.NutritionEatenPerDayExplanation_NewTemp(p, true, true, false);
		}

		// Token: 0x0600050B RID: 1291 RVA: 0x00019438 File Offset: 0x00017638
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

		// Token: 0x040002B3 RID: 691
		public Intelligence intelligence;

		// Token: 0x040002B4 RID: 692
		private FleshTypeDef fleshType;

		// Token: 0x040002B5 RID: 693
		private ThingDef bloodDef;

		// Token: 0x040002B6 RID: 694
		public bool hasGenders = true;

		// Token: 0x040002B7 RID: 695
		public bool needsRest = true;

		// Token: 0x040002B8 RID: 696
		public ThinkTreeDef thinkTreeMain;

		// Token: 0x040002B9 RID: 697
		public ThinkTreeDef thinkTreeConstant;

		// Token: 0x040002BA RID: 698
		public PawnNameCategory nameCategory;

		// Token: 0x040002BB RID: 699
		public FoodTypeFlags foodType;

		// Token: 0x040002BC RID: 700
		public BodyDef body;

		// Token: 0x040002BD RID: 701
		public Type deathActionWorkerClass;

		// Token: 0x040002BE RID: 702
		public List<AnimalBiomeRecord> wildBiomes;

		// Token: 0x040002BF RID: 703
		public SimpleCurve ageGenerationCurve;

		// Token: 0x040002C0 RID: 704
		public bool makesFootprints;

		// Token: 0x040002C1 RID: 705
		public int executionRange = 2;

		// Token: 0x040002C2 RID: 706
		public float lifeExpectancy = 10f;

		// Token: 0x040002C3 RID: 707
		public List<HediffGiverSetDef> hediffGiverSets;

		// Token: 0x040002C4 RID: 708
		public bool herdAnimal;

		// Token: 0x040002C5 RID: 709
		public bool packAnimal;

		// Token: 0x040002C6 RID: 710
		public bool predator;

		// Token: 0x040002C7 RID: 711
		public float maxPreyBodySize = 99999f;

		// Token: 0x040002C8 RID: 712
		public float wildness;

		// Token: 0x040002C9 RID: 713
		public float petness;

		// Token: 0x040002CA RID: 714
		public float nuzzleMtbHours = -1f;

		// Token: 0x040002CB RID: 715
		public float manhunterOnDamageChance;

		// Token: 0x040002CC RID: 716
		public float manhunterOnTameFailChance;

		// Token: 0x040002CD RID: 717
		public bool canBePredatorPrey = true;

		// Token: 0x040002CE RID: 718
		public bool herdMigrationAllowed = true;

		// Token: 0x040002CF RID: 719
		public List<ThingDef> willNeverEat;

		// Token: 0x040002D0 RID: 720
		public float gestationPeriodDays = 10f;

		// Token: 0x040002D1 RID: 721
		public SimpleCurve litterSizeCurve;

		// Token: 0x040002D2 RID: 722
		public float mateMtbHours = 12f;

		// Token: 0x040002D3 RID: 723
		[NoTranslate]
		public List<string> untrainableTags;

		// Token: 0x040002D4 RID: 724
		[NoTranslate]
		public List<string> trainableTags;

		// Token: 0x040002D5 RID: 725
		public TrainabilityDef trainability;

		// Token: 0x040002D6 RID: 726
		private RulePackDef nameGenerator;

		// Token: 0x040002D7 RID: 727
		private RulePackDef nameGeneratorFemale;

		// Token: 0x040002D8 RID: 728
		public float nameOnTameChance;

		// Token: 0x040002D9 RID: 729
		public float nameOnNuzzleChance;

		// Token: 0x040002DA RID: 730
		public float baseBodySize = 1f;

		// Token: 0x040002DB RID: 731
		public float baseHealthScale = 1f;

		// Token: 0x040002DC RID: 732
		public float baseHungerRate = 1f;

		// Token: 0x040002DD RID: 733
		public List<LifeStageAge> lifeStageAges = new List<LifeStageAge>();

		// Token: 0x040002DE RID: 734
		[MustTranslate]
		public string meatLabel;

		// Token: 0x040002DF RID: 735
		public Color meatColor = Color.white;

		// Token: 0x040002E0 RID: 736
		public float meatMarketValue = 2f;

		// Token: 0x040002E1 RID: 737
		public ThingDef useMeatFrom;

		// Token: 0x040002E2 RID: 738
		public ThingDef useLeatherFrom;

		// Token: 0x040002E3 RID: 739
		public ThingDef leatherDef;

		// Token: 0x040002E4 RID: 740
		public ShadowData specialShadowData;

		// Token: 0x040002E5 RID: 741
		public IntRange soundCallIntervalRange = new IntRange(2000, 4000);

		// Token: 0x040002E6 RID: 742
		public SoundDef soundMeleeHitPawn;

		// Token: 0x040002E7 RID: 743
		public SoundDef soundMeleeHitBuilding;

		// Token: 0x040002E8 RID: 744
		public SoundDef soundMeleeMiss;

		// Token: 0x040002E9 RID: 745
		public SoundDef soundMeleeDodge;

		// Token: 0x040002EA RID: 746
		[Unsaved(false)]
		private DeathActionWorker deathActionWorkerInt;

		// Token: 0x040002EB RID: 747
		[Unsaved(false)]
		public ThingDef meatDef;

		// Token: 0x040002EC RID: 748
		[Unsaved(false)]
		public ThingDef corpseDef;

		// Token: 0x040002ED RID: 749
		[Unsaved(false)]
		private PawnKindDef cachedAnyPawnKind;
	}
}
