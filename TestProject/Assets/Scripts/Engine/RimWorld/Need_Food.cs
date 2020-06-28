using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B90 RID: 2960
	public class Need_Food : Need
	{
		// Token: 0x17000C34 RID: 3124
		// (get) Token: 0x06004563 RID: 17763 RVA: 0x0017706A File Offset: 0x0017526A
		public bool Starving
		{
			get
			{
				return this.CurCategory == HungerCategory.Starving;
			}
		}

		// Token: 0x17000C35 RID: 3125
		// (get) Token: 0x06004564 RID: 17764 RVA: 0x00177075 File Offset: 0x00175275
		public float PercentageThreshUrgentlyHungry
		{
			get
			{
				return this.pawn.RaceProps.FoodLevelPercentageWantEat * 0.4f;
			}
		}

		// Token: 0x17000C36 RID: 3126
		// (get) Token: 0x06004565 RID: 17765 RVA: 0x0017708D File Offset: 0x0017528D
		public float PercentageThreshHungry
		{
			get
			{
				return this.pawn.RaceProps.FoodLevelPercentageWantEat * 0.8f;
			}
		}

		// Token: 0x17000C37 RID: 3127
		// (get) Token: 0x06004566 RID: 17766 RVA: 0x001770A5 File Offset: 0x001752A5
		public float NutritionBetweenHungryAndFed
		{
			get
			{
				return (1f - this.PercentageThreshHungry) * this.MaxLevel;
			}
		}

		// Token: 0x17000C38 RID: 3128
		// (get) Token: 0x06004567 RID: 17767 RVA: 0x001770BA File Offset: 0x001752BA
		public HungerCategory CurCategory
		{
			get
			{
				if (base.CurLevelPercentage <= 0f)
				{
					return HungerCategory.Starving;
				}
				if (base.CurLevelPercentage < this.PercentageThreshUrgentlyHungry)
				{
					return HungerCategory.UrgentlyHungry;
				}
				if (base.CurLevelPercentage < this.PercentageThreshHungry)
				{
					return HungerCategory.Hungry;
				}
				return HungerCategory.Fed;
			}
		}

		// Token: 0x17000C39 RID: 3129
		// (get) Token: 0x06004568 RID: 17768 RVA: 0x001770EC File Offset: 0x001752EC
		public float FoodFallPerTick
		{
			get
			{
				return this.FoodFallPerTickAssumingCategory(this.CurCategory, false);
			}
		}

		// Token: 0x17000C3A RID: 3130
		// (get) Token: 0x06004569 RID: 17769 RVA: 0x001770FB File Offset: 0x001752FB
		public int TicksUntilHungryWhenFed
		{
			get
			{
				return Mathf.CeilToInt(this.NutritionBetweenHungryAndFed / this.FoodFallPerTickAssumingCategory(HungerCategory.Fed, false));
			}
		}

		// Token: 0x17000C3B RID: 3131
		// (get) Token: 0x0600456A RID: 17770 RVA: 0x00177111 File Offset: 0x00175311
		public int TicksUntilHungryWhenFedIgnoringMalnutrition
		{
			get
			{
				return Mathf.CeilToInt(this.NutritionBetweenHungryAndFed / this.FoodFallPerTickAssumingCategory(HungerCategory.Fed, true));
			}
		}

		// Token: 0x17000C3C RID: 3132
		// (get) Token: 0x0600456B RID: 17771 RVA: 0x0010E022 File Offset: 0x0010C222
		public override int GUIChangeArrow
		{
			get
			{
				return -1;
			}
		}

		// Token: 0x17000C3D RID: 3133
		// (get) Token: 0x0600456C RID: 17772 RVA: 0x00177127 File Offset: 0x00175327
		public override float MaxLevel
		{
			get
			{
				return this.pawn.BodySize * this.pawn.ageTracker.CurLifeStage.foodMaxFactor;
			}
		}

		// Token: 0x17000C3E RID: 3134
		// (get) Token: 0x0600456D RID: 17773 RVA: 0x0017714A File Offset: 0x0017534A
		public float NutritionWanted
		{
			get
			{
				return this.MaxLevel - this.CurLevel;
			}
		}

		// Token: 0x17000C3F RID: 3135
		// (get) Token: 0x0600456E RID: 17774 RVA: 0x0017715C File Offset: 0x0017535C
		private float HungerRate
		{
			get
			{
				return this.pawn.ageTracker.CurLifeStage.hungerRateFactor * this.pawn.RaceProps.baseHungerRate * this.pawn.health.hediffSet.HungerRateFactor * ((this.pawn.story == null || this.pawn.story.traits == null) ? 1f : this.pawn.story.traits.HungerRateFactor) * this.pawn.GetStatValue(StatDefOf.HungerRateMultiplier, true);
			}
		}

		// Token: 0x17000C40 RID: 3136
		// (get) Token: 0x0600456F RID: 17775 RVA: 0x001771F4 File Offset: 0x001753F4
		private float HungerRateIgnoringMalnutrition
		{
			get
			{
				return this.pawn.ageTracker.CurLifeStage.hungerRateFactor * this.pawn.RaceProps.baseHungerRate * this.pawn.health.hediffSet.GetHungerRateFactor(HediffDefOf.Malnutrition) * ((this.pawn.story == null || this.pawn.story.traits == null) ? 1f : this.pawn.story.traits.HungerRateFactor) * this.pawn.GetStatValue(StatDefOf.HungerRateMultiplier, true);
			}
		}

		// Token: 0x17000C41 RID: 3137
		// (get) Token: 0x06004570 RID: 17776 RVA: 0x00177290 File Offset: 0x00175490
		public int TicksStarving
		{
			get
			{
				return Mathf.Max(0, Find.TickManager.TicksGame - this.lastNonStarvingTick);
			}
		}

		// Token: 0x17000C42 RID: 3138
		// (get) Token: 0x06004571 RID: 17777 RVA: 0x001772A9 File Offset: 0x001754A9
		private float MalnutritionSeverityPerInterval
		{
			get
			{
				return 0.00113333331f * Mathf.Lerp(0.8f, 1.2f, Rand.ValueSeeded(this.pawn.thingIDNumber ^ 2551674));
			}
		}

		// Token: 0x06004572 RID: 17778 RVA: 0x001772D6 File Offset: 0x001754D6
		public Need_Food(Pawn pawn) : base(pawn)
		{
		}

		// Token: 0x06004573 RID: 17779 RVA: 0x001772EA File Offset: 0x001754EA
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.lastNonStarvingTick, "lastNonStarvingTick", -99999, false);
		}

		// Token: 0x06004574 RID: 17780 RVA: 0x00177308 File Offset: 0x00175508
		public float FoodFallPerTickAssumingCategory(HungerCategory cat, bool ignoreMalnutrition = false)
		{
			float num = ignoreMalnutrition ? this.HungerRateIgnoringMalnutrition : this.HungerRate;
			switch (cat)
			{
			case HungerCategory.Fed:
				return 2.66666666E-05f * num;
			case HungerCategory.Hungry:
				return 2.66666666E-05f * num * 0.5f;
			case HungerCategory.UrgentlyHungry:
				return 2.66666666E-05f * num * 0.25f;
			case HungerCategory.Starving:
				return 2.66666666E-05f * num * 0.15f;
			default:
				return 999f;
			}
		}

		// Token: 0x06004575 RID: 17781 RVA: 0x00177378 File Offset: 0x00175578
		public override void NeedInterval()
		{
			if (!this.IsFrozen)
			{
				this.CurLevel -= this.FoodFallPerTick * 150f;
			}
			if (!this.Starving)
			{
				this.lastNonStarvingTick = Find.TickManager.TicksGame;
			}
			if (!this.IsFrozen)
			{
				if (this.Starving)
				{
					HealthUtility.AdjustSeverity(this.pawn, HediffDefOf.Malnutrition, this.MalnutritionSeverityPerInterval);
					return;
				}
				HealthUtility.AdjustSeverity(this.pawn, HediffDefOf.Malnutrition, -this.MalnutritionSeverityPerInterval);
			}
		}

		// Token: 0x06004576 RID: 17782 RVA: 0x001773FC File Offset: 0x001755FC
		public override void SetInitialLevel()
		{
			if (this.pawn.RaceProps.Humanlike)
			{
				base.CurLevelPercentage = 0.8f;
			}
			else
			{
				base.CurLevelPercentage = Rand.Range(0.5f, 0.9f);
			}
			if (Current.ProgramState == ProgramState.Playing)
			{
				this.lastNonStarvingTick = Find.TickManager.TicksGame;
			}
		}

		// Token: 0x06004577 RID: 17783 RVA: 0x00177458 File Offset: 0x00175658
		public override string GetTipString()
		{
			return string.Concat(new string[]
			{
				base.LabelCap,
				": ",
				base.CurLevelPercentage.ToStringPercent(),
				" (",
				this.CurLevel.ToString("0.##"),
				" / ",
				this.MaxLevel.ToString("0.##"),
				")\n",
				this.def.description
			});
		}

		// Token: 0x06004578 RID: 17784 RVA: 0x001774E4 File Offset: 0x001756E4
		public override void DrawOnGUI(Rect rect, int maxThresholdMarkers = 2147483647, float customMargin = -1f, bool drawArrows = true, bool doTooltip = true)
		{
			if (this.threshPercents == null)
			{
				this.threshPercents = new List<float>();
			}
			this.threshPercents.Clear();
			this.threshPercents.Add(this.PercentageThreshHungry);
			this.threshPercents.Add(this.PercentageThreshUrgentlyHungry);
			base.DrawOnGUI(rect, maxThresholdMarkers, customMargin, drawArrows, doTooltip);
		}

		// Token: 0x040027C6 RID: 10182
		private int lastNonStarvingTick = -99999;

		// Token: 0x040027C7 RID: 10183
		public const float BaseFoodFallPerTick = 2.66666666E-05f;

		// Token: 0x040027C8 RID: 10184
		private const float BaseMalnutritionSeverityPerDay = 0.17f;

		// Token: 0x040027C9 RID: 10185
		private const float BaseMalnutritionSeverityPerInterval = 0.00113333331f;
	}
}
