using System;

namespace Verse
{
	// Token: 0x020001DD RID: 477
	public static class TemperatureTuning
	{
		// Token: 0x04000A51 RID: 2641
		public const float MinimumTemperature = -273.15f;

		// Token: 0x04000A52 RID: 2642
		public const float MaximumTemperature = 1000f;

		// Token: 0x04000A53 RID: 2643
		public const float DefaultTemperature = 21f;

		// Token: 0x04000A54 RID: 2644
		public const float DeepUndergroundTemperature = 15f;

		// Token: 0x04000A55 RID: 2645
		public static readonly SimpleCurve SeasonalTempVariationCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 3f),
				true
			},
			{
				new CurvePoint(0.1f, 4f),
				true
			},
			{
				new CurvePoint(1f, 28f),
				true
			}
		};

		// Token: 0x04000A56 RID: 2646
		public const float DailyTempVariationAmplitude = 7f;

		// Token: 0x04000A57 RID: 2647
		public const float DailySunEffect = 14f;

		// Token: 0x04000A58 RID: 2648
		public const float FoodRefrigerationTemp = 10f;

		// Token: 0x04000A59 RID: 2649
		public const float FoodFreezingTemp = 0f;

		// Token: 0x04000A5A RID: 2650
		public const int RoomTempEqualizeInterval = 120;

		// Token: 0x04000A5B RID: 2651
		public const int Door_TempEqualizeIntervalOpen = 34;

		// Token: 0x04000A5C RID: 2652
		public const int Door_TempEqualizeIntervalClosed = 375;

		// Token: 0x04000A5D RID: 2653
		public const float Door_TempEqualizeRate = 1f;

		// Token: 0x04000A5E RID: 2654
		public const float Vent_TempEqualizeRate = 14f;

		// Token: 0x04000A5F RID: 2655
		public const float InventoryTemperature = 14f;

		// Token: 0x04000A60 RID: 2656
		public const float DropPodTemperature = 14f;

		// Token: 0x04000A61 RID: 2657
		public const float TradeShipTemperature = 14f;
	}
}
