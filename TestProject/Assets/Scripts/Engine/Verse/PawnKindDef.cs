using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	
	public class PawnKindDef : Def
	{
		
		// (get) Token: 0x060005BE RID: 1470 RVA: 0x0001C01A File Offset: 0x0001A21A
		public RaceProperties RaceProps
		{
			get
			{
				return this.race.race;
			}
		}

		
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			for (int i = 0; i < this.lifeStages.Count; i++)
			{
				this.lifeStages[i].ResolveReferences();
			}
		}

		
		public string GetLabelPlural(int count = -1)
		{
			if (!this.labelPlural.NullOrEmpty())
			{
				return this.labelPlural;
			}
			return Find.ActiveLanguageWorker.Pluralize(this.label, count);
		}

		
		public override void PostLoad()
		{
			if (this.backstoryCategories != null && this.backstoryCategories.Count > 0)
			{
				if (this.backstoryFilters == null)
				{
					this.backstoryFilters = new List<BackstoryCategoryFilter>();
				}
				this.backstoryFilters.Add(new BackstoryCategoryFilter
				{
					categories = this.backstoryCategories
				});
			}
		}

		
		public float GetAnimalPointsToHuntOrSlaughter()
		{
			return this.combatPower * 5f * (1f + this.RaceProps.manhunterOnDamageChance * 0.5f) * (1f + this.RaceProps.manhunterOnTameFailChance * 0.2f) * (1f + this.RaceProps.wildness) + this.race.BaseMarketValue;
		}

		public override IEnumerable<string> ConfigErrors()
		{

			IEnumerator<string> enumerator = null;
			if (this.backstoryFilters != null && this.backstoryFiltersOverride != null)
			{
				yield return "both backstoryCategories and backstoryCategoriesOverride are defined";
			}
			if (this.race == null)
			{
				yield return "no race";
			}
			if (this.baseRecruitDifficulty > 1.0001f)
			{
				yield return this.defName + " recruitDifficulty is greater than 1. 1 means impossible to recruit.";
			}
			if (this.combatPower < 0f)
			{
				yield return this.defName + " has no combatPower.";
			}
			if (this.weaponMoney != FloatRange.Zero)
			{
				float num = 999999f;
				int num2;
				int i;
				for (i = 0; i < this.weaponTags.Count; i = num2 + 1)
				{
					IEnumerable<ThingDef> source = from d in DefDatabase<ThingDef>.AllDefs
					where d.weaponTags != null && d.weaponTags.Contains(this.weaponTags[i])
					select d;
					if (source.Any<ThingDef>())
					{
						num = Mathf.Min(num, source.Min((ThingDef d) => PawnWeaponGenerator.CheapestNonDerpPriceFor(d)));
					}
					num2 = i;
				}
				if (num < 999999f && num > this.weaponMoney.min)
				{
					yield return string.Concat(new object[]
					{
						"Cheapest weapon with one of my weaponTags costs ",
						num,
						" but weaponMoney min is ",
						this.weaponMoney.min,
						", so could end up weaponless."
					});
				}
			}
			if (!this.RaceProps.Humanlike && this.lifeStages.Count != this.RaceProps.lifeStageAges.Count)
			{
				yield return string.Concat(new object[]
				{
					"PawnKindDef defines ",
					this.lifeStages.Count,
					" lifeStages while race def defines ",
					this.RaceProps.lifeStageAges.Count
				});
			}
			if (this.apparelRequired != null)
			{
				int num2;
				for (int k = 0; k < this.apparelRequired.Count; k = num2 + 1)
				{
					for (int j = k + 1; j < this.apparelRequired.Count; j = num2 + 1)
					{
						if (!ApparelUtility.CanWearTogether(this.apparelRequired[k], this.apparelRequired[j], this.race.race.body))
						{
							yield return string.Concat(new object[]
							{
								"required apparel can't be worn together (",
								this.apparelRequired[k],
								", ",
								this.apparelRequired[j],
								")"
							});
						}
						num2 = j;
					}
					num2 = k;
				}
			}
			if (this.alternateGraphics != null)
			{
				using (List<AlternateGraphic>.Enumerator enumerator2 = this.alternateGraphics.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						if (enumerator2.Current.Weight < 0f)
						{
							yield return "alternate graphic has negative weight.";
						}
					}
				}

			}
			yield break;
		}

		
		public static PawnKindDef Named(string defName)
		{
			return DefDatabase<PawnKindDef>.GetNamed(defName, true);
		}

		
		public ThingDef race;

		
		public FactionDef defaultFactionType;

		
		[NoTranslate]
		public List<BackstoryCategoryFilter> backstoryFilters;

		
		[NoTranslate]
		public List<BackstoryCategoryFilter> backstoryFiltersOverride;

		
		[NoTranslate]
		public List<string> backstoryCategories;

		
		[MustTranslate]
		public string labelPlural;

		
		public List<PawnKindLifeStage> lifeStages = new List<PawnKindLifeStage>();

		
		public List<AlternateGraphic> alternateGraphics;

		
		public List<TraitDef> disallowedTraits;

		
		public float alternateGraphicChance;

		
		public float backstoryCryptosleepCommonality;

		
		public int minGenerationAge;

		
		public int maxGenerationAge = 999999;

		
		public bool factionLeader;

		
		public bool destroyGearOnDrop;

		
		public float defendPointRadius = -1f;

		
		public float royalTitleChance;

		
		public RoyalTitleDef titleRequired;

		
		public RoyalTitleDef minTitleRequired;

		
		public List<RoyalTitleDef> titleSelectOne;

		
		public bool allowRoyalRoomRequirements = true;

		
		public bool allowRoyalApparelRequirements = true;

		
		public bool isFighter = true;

		
		public float combatPower = -1f;

		
		public bool canArriveManhunter = true;

		
		public bool canBeSapper;

		
		public float baseRecruitDifficulty = 0.5f;

		
		public bool aiAvoidCover;

		
		public FloatRange fleeHealthThresholdRange = new FloatRange(-0.4f, 0.4f);

		
		public QualityCategory itemQuality = QualityCategory.Normal;

		
		public bool forceNormalGearQuality;

		
		public FloatRange gearHealthRange = FloatRange.One;

		
		public FloatRange weaponMoney = FloatRange.Zero;

		
		[NoTranslate]
		public List<string> weaponTags;

		
		public FloatRange apparelMoney = FloatRange.Zero;

		
		public List<ThingDef> apparelRequired;

		
		[NoTranslate]
		public List<string> apparelTags;

		
		[NoTranslate]
		public List<string> apparelDisallowTags;

		
		[NoTranslate]
		public List<string> hairTags;

		
		public float apparelAllowHeadgearChance = 1f;

		
		public bool apparelIgnoreSeasons;

		
		public Color apparelColor = Color.white;

		
		public List<SpecificApparelRequirement> specificApparelRequirements;

		
		public List<ThingDef> techHediffsRequired;

		
		public FloatRange techHediffsMoney = FloatRange.Zero;

		
		[NoTranslate]
		public List<string> techHediffsTags;

		
		[NoTranslate]
		public List<string> techHediffsDisallowTags;

		
		public float techHediffsChance;

		
		public int techHediffsMaxAmount = 1;

		
		public float biocodeWeaponChance;

		
		public List<ThingDefCountClass> fixedInventory = new List<ThingDefCountClass>();

		
		public PawnInventoryOption inventoryOptions;

		
		public float invNutrition;

		
		public ThingDef invFoodDef;

		
		public float chemicalAddictionChance;

		
		public float combatEnhancingDrugsChance;

		
		public IntRange combatEnhancingDrugsCount = IntRange.zero;

		
		public bool trader;

		
		public List<SkillRange> skills;

		
		public WorkTags requiredWorkTags;

		
		[MustTranslate]
		public string labelMale;

		
		[MustTranslate]
		public string labelMalePlural;

		
		[MustTranslate]
		public string labelFemale;

		
		[MustTranslate]
		public string labelFemalePlural;

		
		public IntRange wildGroupSize = IntRange.one;

		
		public float ecoSystemWeight = 1f;

		
		private const int MaxWeaponMoney = 999999;
	}
}
