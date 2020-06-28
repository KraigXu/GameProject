using System;

namespace RimWorld.Planet
{
	// Token: 0x020011D1 RID: 4561
	public static class OverallPopulationUtility
	{
		// Token: 0x170011AA RID: 4522
		// (get) Token: 0x060069BD RID: 27069 RVA: 0x0024E184 File Offset: 0x0024C384
		public static int EnumValuesCount
		{
			get
			{
				if (OverallPopulationUtility.cachedEnumValuesCount < 0)
				{
					OverallPopulationUtility.cachedEnumValuesCount = Enum.GetNames(typeof(OverallPopulation)).Length;
				}
				return OverallPopulationUtility.cachedEnumValuesCount;
			}
		}

		// Token: 0x060069BE RID: 27070 RVA: 0x0024E1AC File Offset: 0x0024C3AC
		public static float GetScaleFactor(this OverallPopulation population)
		{
			switch (population)
			{
			case OverallPopulation.AlmostNone:
				return 0.1f;
			case OverallPopulation.Little:
				return 0.4f;
			case OverallPopulation.LittleBitLess:
				return 0.7f;
			case OverallPopulation.Normal:
				return 1f;
			case OverallPopulation.LittleBitMore:
				return 1.5f;
			case OverallPopulation.High:
				return 2f;
			case OverallPopulation.VeryHigh:
				return 2.75f;
			default:
				return 1f;
			}
		}

		// Token: 0x0400419F RID: 16799
		private static int cachedEnumValuesCount = -1;

		// Token: 0x040041A0 RID: 16800
		private const float ScaleFactor_AlmostNone = 0.1f;

		// Token: 0x040041A1 RID: 16801
		private const float ScaleFactor_Little = 0.4f;

		// Token: 0x040041A2 RID: 16802
		private const float ScaleFactor_LittleBitLess = 0.7f;

		// Token: 0x040041A3 RID: 16803
		private const float ScaleFactor_Normal = 1f;

		// Token: 0x040041A4 RID: 16804
		private const float ScaleFactor_LittleBitMore = 1.5f;

		// Token: 0x040041A5 RID: 16805
		private const float ScaleFactor_High = 2f;

		// Token: 0x040041A6 RID: 16806
		private const float ScaleFactor_VeryHigh = 2.75f;
	}
}
