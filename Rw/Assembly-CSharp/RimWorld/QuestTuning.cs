using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009A9 RID: 2473
	public static class QuestTuning
	{
		// Token: 0x040022AE RID: 8878
		public static readonly SimpleCurve IncreasesPopQuestChanceByPopIntentCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0.05f),
				true
			},
			{
				new CurvePoint(1f, 0.3f),
				true
			},
			{
				new CurvePoint(3f, 0.45f),
				true
			}
		};

		// Token: 0x040022AF RID: 8879
		public const float RecentQuestSelectionWeightFactor0 = 0.01f;

		// Token: 0x040022B0 RID: 8880
		public const float RecentQuestSelectionWeightFactor1 = 0.3f;

		// Token: 0x040022B1 RID: 8881
		public const float RecentQuestSelectionWeightFactor2 = 0.5f;

		// Token: 0x040022B2 RID: 8882
		public const float RecentQuestSelectionWeightFactor3 = 0.7f;

		// Token: 0x040022B3 RID: 8883
		public const float RecentQuestSelectionWeightFactor4 = 0.9f;

		// Token: 0x040022B4 RID: 8884
		public static readonly SimpleCurve NonFavorQuestSelectionWeightFactorByDaysSinceFavorQuestCurve = new SimpleCurve
		{
			{
				new CurvePoint(10f, 1f),
				true
			},
			{
				new CurvePoint(25f, 0.01f),
				true
			}
		};

		// Token: 0x040022B5 RID: 8885
		public static readonly SimpleCurve PointsToRewardMarketValueCurve = new SimpleCurve
		{
			{
				new CurvePoint(300f, 800f),
				true
			},
			{
				new CurvePoint(700f, 1500f),
				true
			},
			{
				new CurvePoint(5000f, 4000f),
				true
			}
		};

		// Token: 0x040022B6 RID: 8886
		public const int MinFavorAtOnce = 1;

		// Token: 0x040022B7 RID: 8887
		public const int MaxFavorAtOnce = 12;

		// Token: 0x040022B8 RID: 8888
		public const int MaxGoodwillToAllowGoodwillReward = 92;

		// Token: 0x040022B9 RID: 8889
		public static readonly SimpleCurve PopIncreasingRewardWeightByPopIntentCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0.05f),
				true
			},
			{
				new CurvePoint(1f, 1f),
				true
			},
			{
				new CurvePoint(3f, 2f),
				true
			}
		};

		// Token: 0x040022BA RID: 8890
		public const float FutureResearchProjectTechprintSelectionWeightFactor = 0.02f;

		// Token: 0x040022BB RID: 8891
		public static readonly SimpleCurve DaysSincePsylinkAvailableToGuaranteedNeuroformerChance = new SimpleCurve
		{
			{
				new CurvePoint(45f, 0f),
				true
			},
			{
				new CurvePoint(60f, 1f),
				true
			}
		};

		// Token: 0x040022BC RID: 8892
		public const float MinDaysBetweenRaidSourceRaids = 1.5f;

		// Token: 0x040022BD RID: 8893
		public const float RaidSourceRaidThreatPointsFactor = 0.6f;

		// Token: 0x040022BE RID: 8894
		public static readonly SimpleCurve PointsToRaidSourceRaidsMTBDaysCurve = new SimpleCurve
		{
			{
				new CurvePoint(400f, 25f),
				true
			},
			{
				new CurvePoint(1500f, 10f),
				true
			},
			{
				new CurvePoint(5000f, 5f),
				true
			}
		};
	}
}
