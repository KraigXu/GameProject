using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BE7 RID: 3047
	public static class DiplomacyTuning
	{
		// Token: 0x0400294B RID: 10571
		public const int MaxGoodwill = 100;

		// Token: 0x0400294C RID: 10572
		public const int MinGoodwill = -100;

		// Token: 0x0400294D RID: 10573
		public const int BecomeHostileThreshold = -75;

		// Token: 0x0400294E RID: 10574
		public const int BecomeNeutralThreshold = 0;

		// Token: 0x0400294F RID: 10575
		public const int BecomeAllyThreshold = 75;

		// Token: 0x04002950 RID: 10576
		public const int InitialHostileThreshold = -10;

		// Token: 0x04002951 RID: 10577
		public const int InitialAllyThreshold = 75;

		// Token: 0x04002952 RID: 10578
		public static readonly IntRange ForcedStartingEnemyGoodwillRange = new IntRange(-100, -40);

		// Token: 0x04002953 RID: 10579
		public const int MinGoodwillToRequestAICoreQuest = 40;

		// Token: 0x04002954 RID: 10580
		public const int RequestAICoreQuestSilverCost = 1500;

		// Token: 0x04002955 RID: 10581
		public static readonly FloatRange RansomFeeMarketValueFactorRange = new FloatRange(1.2f, 2.2f);

		// Token: 0x04002956 RID: 10582
		public const int Goodwill_NaturalChangeStep = 10;

		// Token: 0x04002957 RID: 10583
		public const float Goodwill_PerDirectDamageToPawn = -1.3f;

		// Token: 0x04002958 RID: 10584
		public const float Goodwill_PerDirectDamageToBuilding = -1f;

		// Token: 0x04002959 RID: 10585
		public const int Goodwill_MemberCrushed_Humanlike = -25;

		// Token: 0x0400295A RID: 10586
		public const int Goodwill_MemberCrushed_Animal = -15;

		// Token: 0x0400295B RID: 10587
		public const int Goodwill_MemberNeutrallyDied_Humanlike = -5;

		// Token: 0x0400295C RID: 10588
		public const int Goodwill_MemberNeutrallyDied_Animal = -3;

		// Token: 0x0400295D RID: 10589
		public const int Goodwill_BodyPartRemovalViolation = -70;

		// Token: 0x0400295E RID: 10590
		public const int Goodwill_MemberEuthanized = -100;

		// Token: 0x0400295F RID: 10591
		public const int Goodwill_AttackedSettlement = -50;

		// Token: 0x04002960 RID: 10592
		public const int Goodwill_MilitaryAidRequested = -25;

		// Token: 0x04002961 RID: 10593
		public const int Goodwill_TraderRequested = -15;

		// Token: 0x04002962 RID: 10594
		public const int Goodwill_MemberStripped = -10;

		// Token: 0x04002963 RID: 10595
		public static readonly SimpleCurve Goodwill_PerQuadrumFromSettlementProximity = new SimpleCurve
		{
			{
				new CurvePoint(2f, -30f),
				true
			},
			{
				new CurvePoint(3f, -20f),
				true
			},
			{
				new CurvePoint(4f, -10f),
				true
			},
			{
				new CurvePoint(5f, 0f),
				true
			}
		};

		// Token: 0x04002964 RID: 10596
		public const float Goodwill_BaseGiftSilverForOneGoodwill = 40f;

		// Token: 0x04002965 RID: 10597
		public static readonly SimpleCurve GiftGoodwillFactorRelationsCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(75f, 0.25f),
				true
			}
		};

		// Token: 0x04002966 RID: 10598
		public const float Goodwill_GiftPrisonerOfTheirFactionValueFactor = 2f;

		// Token: 0x04002967 RID: 10599
		public const float Goodwill_TradedMarketValueforOneGoodwill = 600f;

		// Token: 0x04002968 RID: 10600
		public const int Goodwill_DestroyedMutualEnemyBase = 20;

		// Token: 0x04002969 RID: 10601
		public const int Goodwill_MemberExitedMapHealthy = 12;

		// Token: 0x0400296A RID: 10602
		public const int Goodwill_MemberExitedMapHealthy_LeaderBonus = 40;

		// Token: 0x0400296B RID: 10603
		public const float Goodwill_PerTend = 1f;

		// Token: 0x0400296C RID: 10604
		public const int Goodwill_MaxTimesTendedTo = 10;

		// Token: 0x0400296D RID: 10605
		public const int Goodwill_QuestTradeRequestCompleted = 12;

		// Token: 0x0400296E RID: 10606
		public static readonly IntRange Goodwill_PeaceTalksDisasterRange = new IntRange(-50, -40);

		// Token: 0x0400296F RID: 10607
		public static readonly IntRange Goodwill_PeaceTalksBackfireRange = new IntRange(-20, -10);

		// Token: 0x04002970 RID: 10608
		public static readonly IntRange Goodwill_PeaceTalksSuccessRange = new IntRange(60, 70);

		// Token: 0x04002971 RID: 10609
		public static readonly IntRange Goodwill_PeaceTalksTriumphRange = new IntRange(100, 110);

		// Token: 0x04002972 RID: 10610
		public static readonly IntRange RoyalFavor_PeaceTalksSuccessRange = new IntRange(1, 4);

		// Token: 0x04002973 RID: 10611
		public const float VisitorGiftChanceBase = 0.25f;

		// Token: 0x04002974 RID: 10612
		public static readonly SimpleCurve VisitorGiftChanceFactorFromPlayerWealthCurve = new SimpleCurve
		{
			{
				new CurvePoint(30000f, 1f),
				true
			},
			{
				new CurvePoint(80000f, 0.1f),
				true
			},
			{
				new CurvePoint(300000f, 0f),
				true
			}
		};

		// Token: 0x04002975 RID: 10613
		public static readonly SimpleCurve VisitorGiftChanceFactorFromGoodwillCurve = new SimpleCurve
		{
			{
				new CurvePoint(-30f, 0f),
				true
			},
			{
				new CurvePoint(0f, 1f),
				true
			}
		};

		// Token: 0x04002976 RID: 10614
		public static readonly FloatRange VisitorGiftTotalMarketValueRangeBase = new FloatRange(100f, 500f);

		// Token: 0x04002977 RID: 10615
		public static readonly SimpleCurve VisitorGiftTotalMarketValueFactorFromPlayerWealthCurve = new SimpleCurve
		{
			{
				new CurvePoint(10000f, 0.25f),
				true
			},
			{
				new CurvePoint(100000f, 1f),
				true
			}
		};

		// Token: 0x04002978 RID: 10616
		public static readonly FloatRange RequestedMilitaryAidPointsRange = new FloatRange(800f, 1000f);
	}
}
