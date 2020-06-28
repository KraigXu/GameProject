using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000894 RID: 2196
	public class IngestibleProperties
	{
		// Token: 0x17000979 RID: 2425
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

		// Token: 0x1700097A RID: 2426
		// (get) Token: 0x06003557 RID: 13655 RVA: 0x001235F8 File Offset: 0x001217F8
		public bool HumanEdible
		{
			get
			{
				return (FoodTypeFlags.OmnivoreHuman & this.foodType) > FoodTypeFlags.None;
			}
		}

		// Token: 0x1700097B RID: 2427
		// (get) Token: 0x06003558 RID: 13656 RVA: 0x00123609 File Offset: 0x00121809
		public bool IsMeal
		{
			get
			{
				return this.preferability >= FoodPreferability.MealAwful && this.preferability <= FoodPreferability.MealLavish;
			}
		}

		// Token: 0x1700097C RID: 2428
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

		// Token: 0x0600355A RID: 13658 RVA: 0x0012364F File Offset: 0x0012184F
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

		// Token: 0x0600355B RID: 13659 RVA: 0x00123660 File Offset: 0x00121860
		public RoyalTitleDef MaxSatisfiedTitle()
		{
			return (from t in DefDatabase<FactionDef>.AllDefsListForReading.SelectMany((FactionDef f) => f.RoyalTitlesAwardableInSeniorityOrderForReading)
			where t.foodRequirement.Defined && t.foodRequirement.Acceptable(this.parent)
			orderby t.seniority descending
			select t).FirstOrDefault<RoyalTitleDef>();
		}

		// Token: 0x0600355C RID: 13660 RVA: 0x001236D0 File Offset: 0x001218D0
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

		// Token: 0x04001CF6 RID: 7414
		[Unsaved(false)]
		public ThingDef parent;

		// Token: 0x04001CF7 RID: 7415
		public int maxNumToIngestAtOnce = 20;

		// Token: 0x04001CF8 RID: 7416
		public List<IngestionOutcomeDoer> outcomeDoers;

		// Token: 0x04001CF9 RID: 7417
		public int baseIngestTicks = 500;

		// Token: 0x04001CFA RID: 7418
		public float chairSearchRadius = 32f;

		// Token: 0x04001CFB RID: 7419
		public bool useEatingSpeedStat = true;

		// Token: 0x04001CFC RID: 7420
		public ThoughtDef tasteThought;

		// Token: 0x04001CFD RID: 7421
		public ThoughtDef specialThoughtDirect;

		// Token: 0x04001CFE RID: 7422
		public ThoughtDef specialThoughtAsIngredient;

		// Token: 0x04001CFF RID: 7423
		public EffecterDef ingestEffect;

		// Token: 0x04001D00 RID: 7424
		public EffecterDef ingestEffectEat;

		// Token: 0x04001D01 RID: 7425
		public SoundDef ingestSound;

		// Token: 0x04001D02 RID: 7426
		[MustTranslate]
		public string ingestCommandString;

		// Token: 0x04001D03 RID: 7427
		[MustTranslate]
		public string ingestReportString;

		// Token: 0x04001D04 RID: 7428
		[MustTranslate]
		public string ingestReportStringEat;

		// Token: 0x04001D05 RID: 7429
		public HoldOffsetSet ingestHoldOffsetStanding;

		// Token: 0x04001D06 RID: 7430
		public bool ingestHoldUsesTable = true;

		// Token: 0x04001D07 RID: 7431
		public FoodTypeFlags foodType;

		// Token: 0x04001D08 RID: 7432
		public float joy;

		// Token: 0x04001D09 RID: 7433
		public JoyKindDef joyKind;

		// Token: 0x04001D0A RID: 7434
		public ThingDef sourceDef;

		// Token: 0x04001D0B RID: 7435
		public FoodPreferability preferability;

		// Token: 0x04001D0C RID: 7436
		public bool nurseable;

		// Token: 0x04001D0D RID: 7437
		public float optimalityOffsetHumanlikes;

		// Token: 0x04001D0E RID: 7438
		public float optimalityOffsetFeedingAnimals;

		// Token: 0x04001D0F RID: 7439
		public DrugCategory drugCategory;

		// Token: 0x04001D10 RID: 7440
		[Unsaved(false)]
		private float cachedNutrition = -1f;
	}
}
