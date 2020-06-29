using System;
using Verse;

namespace RimWorld
{
	
	public static class QuestTuning
	{
		
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

		
		public const float RecentQuestSelectionWeightFactor0 = 0.01f;

		
		public const float RecentQuestSelectionWeightFactor1 = 0.3f;

		
		public const float RecentQuestSelectionWeightFactor2 = 0.5f;

		
		public const float RecentQuestSelectionWeightFactor3 = 0.7f;

		
		public const float RecentQuestSelectionWeightFactor4 = 0.9f;

		
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

		
		public const int MinFavorAtOnce = 1;

		
		public const int MaxFavorAtOnce = 12;

		
		public const int MaxGoodwillToAllowGoodwillReward = 92;

		
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

		
		public const float FutureResearchProjectTechprintSelectionWeightFactor = 0.02f;

		
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

		
		public const float MinDaysBetweenRaidSourceRaids = 1.5f;

		
		public const float RaidSourceRaidThreatPointsFactor = 0.6f;

		
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
