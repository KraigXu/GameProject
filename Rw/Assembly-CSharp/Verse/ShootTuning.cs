using System;

namespace Verse
{
	// Token: 0x02000479 RID: 1145
	public static class ShootTuning
	{
		// Token: 0x040014AA RID: 5290
		public const float DistTouch = 3f;

		// Token: 0x040014AB RID: 5291
		public const float DistShort = 12f;

		// Token: 0x040014AC RID: 5292
		public const float DistMedium = 25f;

		// Token: 0x040014AD RID: 5293
		public const float DistLong = 40f;

		// Token: 0x040014AE RID: 5294
		public const float MeleeRange = 1.42f;

		// Token: 0x040014AF RID: 5295
		public const float HitChanceFactorFromEquipmentMin = 0.01f;

		// Token: 0x040014B0 RID: 5296
		public const float MinAccuracyFactorFromShooterAndDistance = 0.0201f;

		// Token: 0x040014B1 RID: 5297
		public const float LayingDownHitChanceFactorMinDistance = 4.5f;

		// Token: 0x040014B2 RID: 5298
		public const float HitChanceFactorIfLayingDown = 0.2f;

		// Token: 0x040014B3 RID: 5299
		public const float ExecutionMaxDistance = 3.9f;

		// Token: 0x040014B4 RID: 5300
		public const float ExecutionAccuracyFactor = 7.5f;

		// Token: 0x040014B5 RID: 5301
		public const float TargetSizeFactorFromFillPercentFactor = 2.5f;

		// Token: 0x040014B6 RID: 5302
		public const float TargetSizeFactorMin = 0.5f;

		// Token: 0x040014B7 RID: 5303
		public const float TargetSizeFactorMax = 2f;

		// Token: 0x040014B8 RID: 5304
		public const float MinAimOnChance_StandardTarget = 0.0201f;

		// Token: 0x040014B9 RID: 5305
		public static readonly SimpleSurface MissDistanceFromAimOnChanceCurves = new SimpleSurface
		{
			new SurfaceColumn(0.02f, new SimpleCurve
			{
				{
					new CurvePoint(0f, 1f),
					true
				},
				{
					new CurvePoint(1f, 10f),
					true
				}
			}),
			new SurfaceColumn(0.04f, new SimpleCurve
			{
				{
					new CurvePoint(0f, 1f),
					true
				},
				{
					new CurvePoint(1f, 8f),
					true
				}
			}),
			new SurfaceColumn(0.07f, new SimpleCurve
			{
				{
					new CurvePoint(0f, 1f),
					true
				},
				{
					new CurvePoint(1f, 6f),
					true
				}
			}),
			new SurfaceColumn(0.11f, new SimpleCurve
			{
				{
					new CurvePoint(0f, 1f),
					true
				},
				{
					new CurvePoint(1f, 4f),
					true
				}
			}),
			new SurfaceColumn(0.22f, new SimpleCurve
			{
				{
					new CurvePoint(0f, 1f),
					true
				},
				{
					new CurvePoint(1f, 2f),
					true
				}
			}),
			new SurfaceColumn(1f, new SimpleCurve
			{
				{
					new CurvePoint(0f, 1f),
					true
				},
				{
					new CurvePoint(1f, 1f),
					true
				}
			})
		};

		// Token: 0x040014BA RID: 5306
		public const float CanInterceptPawnsChanceOnWildOrForcedMissRadius = 0.5f;

		// Token: 0x040014BB RID: 5307
		public const float InterceptDistMin = 5f;

		// Token: 0x040014BC RID: 5308
		public const float InterceptDistMax = 12f;

		// Token: 0x040014BD RID: 5309
		public const float Intercept_Pawn_HitChancePerBodySize = 0.4f;

		// Token: 0x040014BE RID: 5310
		public const float Intercept_Pawn_HitChanceFactor_LayingDown = 0.1f;

		// Token: 0x040014BF RID: 5311
		public const float Intercept_Pawn_HitChanceFactor_NonWildNonEnemy = 0.4f;

		// Token: 0x040014C0 RID: 5312
		public const float Intercept_Object_HitChancePerFillPercent = 0.15f;

		// Token: 0x040014C1 RID: 5313
		public const float Intercept_Object_AdjToTarget_HitChancePerFillPercent = 1f;

		// Token: 0x040014C2 RID: 5314
		public const float Intercept_OpenDoor_HitChance = 0.05f;

		// Token: 0x040014C3 RID: 5315
		public const float ImpactCell_Pawn_HitChancePerBodySize = 0.5f;

		// Token: 0x040014C4 RID: 5316
		public const float ImpactCell_Object_HitChancePerFillPercent = 1.5f;

		// Token: 0x040014C5 RID: 5317
		public const float BodySizeClampMin = 0.1f;

		// Token: 0x040014C6 RID: 5318
		public const float BodySizeClampMax = 2f;
	}
}
