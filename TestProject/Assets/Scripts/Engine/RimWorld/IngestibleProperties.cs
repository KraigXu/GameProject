using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	
	public class IngestibleProperties
	{
		
		// (get) Token: 0x06003556 RID: 13654 RVA: 0x001235E2 File Offset: 0x001217E2
		public JoyKindDef JoyKind
		{
			get
			{
				if (this.joyKind == null)
				{
					return JoyKindDefOf.Gluttonous;
				}
				return this.joyKind;
			}
		}

		
		// (get) Token: 0x06003557 RID: 13655 RVA: 0x001235F8 File Offset: 0x001217F8
		public bool HumanEdible
		{
			get
			{
				return (FoodTypeFlags.OmnivoreHuman & this.foodType) > FoodTypeFlags.None;
			}
		}

		
		// (get) Token: 0x06003558 RID: 13656 RVA: 0x00123609 File Offset: 0x00121809
		public bool IsMeal
		{
			get
			{
				return this.preferability >= FoodPreferability.MealAwful && this.preferability <= FoodPreferability.MealLavish;
			}
		}

		
		// (get) Token: 0x06003559 RID: 13657 RVA: 0x00123623 File Offset: 0x00121823
		public float CachedNutrition
		{
			get
			{
				if (this.cachedNutrition == -1f)
				{
					this.cachedNutrition = this.parent.GetStatValueAbstract(StatDefOf.Nutrition, null);
				}
				return this.cachedNutrition;
			}
		}

		
		public IEnumerable<string> ConfigErrors()
		{
			if (this.preferability == FoodPreferability.Undefined)
			{
				yield return "undefined preferability";
			}
			if (this.foodType == FoodTypeFlags.None)
			{
				yield return "no foodType";
			}
			if (this.parent.GetStatValueAbstract(StatDefOf.Nutrition, null) == 0f && this.preferability != FoodPreferability.NeverForNutrition)
			{
				yield return string.Concat(new object[]
				{
					"Nutrition == 0 but preferability is ",
					this.preferability,
					" instead of ",
					FoodPreferability.NeverForNutrition
				});
			}
			if (!this.parent.IsCorpse && this.preferability > FoodPreferability.DesperateOnlyForHumanlikes && !this.parent.socialPropernessMatters && this.parent.EverHaulable)
			{
				yield return "ingestible preferability > DesperateOnlyForHumanlikes but socialPropernessMatters=false. This will cause bugs wherein wardens will look in prison cells for food to give to prisoners and so will repeatedly pick up and drop food inside the cell.";
			}
			if (this.joy > 0f && this.joyKind == null)
			{
				yield return "joy > 0 with no joy kind";
			}
			if (this.joy == 0f && this.joyKind != null)
			{
				yield return "joy is 0 but joyKind is " + this.joyKind;
			}
			yield break;
		}

		
		public RoyalTitleDef MaxSatisfiedTitle()
		{
			return (from t in DefDatabase<FactionDef>.AllDefsListForReading.SelectMany((FactionDef f) => f.RoyalTitlesAwardableInSeniorityOrderForReading)
			where t.foodRequirement.Defined && t.foodRequirement.Acceptable(this.parent)
			orderby t.seniority descending
			select t).FirstOrDefault<RoyalTitleDef>();
		}

		
		internal IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			if (this.joy > 0f)
			{
				StatCategoryDef category = (this.drugCategory != DrugCategory.None) ? StatCategoryDefOf.Drug : StatCategoryDefOf.Basics;
				yield return new StatDrawEntry(category, "Joy".Translate(), this.joy.ToStringPercent("F0") + " (" + this.JoyKind.label + ")", "Stat_Thing_Ingestible_Joy_Desc".Translate(), 4751, null, null, false);
			}
			if (this.HumanEdible)
			{
				RoyalTitleDef royalTitleDef = this.MaxSatisfiedTitle();
				if (royalTitleDef != null)
				{
					yield return new StatDrawEntry(StatCategoryDefOf.Basics, "Stat_Thing_Ingestible_MaxSatisfiedTitle".Translate(), royalTitleDef.GetLabelCapForBothGenders(), "Stat_Thing_Ingestible_MaxSatisfiedTitle_Desc".Translate(), 4752, null, null, false);
				}
			}
			if (this.outcomeDoers != null)
			{
				int num;
				for (int i = 0; i < this.outcomeDoers.Count; i = num + 1)
				{
					foreach (StatDrawEntry statDrawEntry in this.outcomeDoers[i].SpecialDisplayStats(this.parent))
					{
						yield return statDrawEntry;
					}
					IEnumerator<StatDrawEntry> enumerator = null;
					num = i;
				}
			}
			yield break;
			yield break;
		}

		
		[Unsaved(false)]
		public ThingDef parent;

		
		public int maxNumToIngestAtOnce = 20;

		
		public List<IngestionOutcomeDoer> outcomeDoers;

		
		public int baseIngestTicks = 500;

		
		public float chairSearchRadius = 32f;

		
		public bool useEatingSpeedStat = true;

		
		public ThoughtDef tasteThought;

		
		public ThoughtDef specialThoughtDirect;

		
		public ThoughtDef specialThoughtAsIngredient;

		
		public EffecterDef ingestEffect;

		
		public EffecterDef ingestEffectEat;

		
		public SoundDef ingestSound;

		
		[MustTranslate]
		public string ingestCommandString;

		
		[MustTranslate]
		public string ingestReportString;

		
		[MustTranslate]
		public string ingestReportStringEat;

		
		public HoldOffsetSet ingestHoldOffsetStanding;

		
		public bool ingestHoldUsesTable = true;

		
		public FoodTypeFlags foodType;

		
		public float joy;

		
		public JoyKindDef joyKind;

		
		public ThingDef sourceDef;

		
		public FoodPreferability preferability;

		
		public bool nurseable;

		
		public float optimalityOffsetHumanlikes;

		
		public float optimalityOffsetFeedingAnimals;

		
		public DrugCategory drugCategory;

		
		[Unsaved(false)]
		private float cachedNutrition = -1f;
	}
}
