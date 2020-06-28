using System;

namespace RimWorld
{
	// Token: 0x02000B91 RID: 2961
	public static class JoyTunings
	{
		// Token: 0x040027CA RID: 10186
		public const float BaseJoyGainPerHour = 0.36f;

		// Token: 0x040027CB RID: 10187
		public const float ThreshLow = 0.15f;

		// Token: 0x040027CC RID: 10188
		public const float ThreshSatisfied = 0.3f;

		// Token: 0x040027CD RID: 10189
		public const float ThreshHigh = 0.7f;

		// Token: 0x040027CE RID: 10190
		public const float ThreshVeryHigh = 0.85f;

		// Token: 0x040027CF RID: 10191
		public const float BaseFallPerInterval = 0.0015f;

		// Token: 0x040027D0 RID: 10192
		public const float FallRateFactorWhenLow = 0.7f;

		// Token: 0x040027D1 RID: 10193
		public const float FallRateFactorWhenVeryLow = 0.4f;

		// Token: 0x040027D2 RID: 10194
		public const float ToleranceGainPerJoy = 0.65f;

		// Token: 0x040027D3 RID: 10195
		public const float BoredStartToleranceThreshold = 0.5f;

		// Token: 0x040027D4 RID: 10196
		public const float BoredEndToleranceThreshold = 0.3f;
	}
}
