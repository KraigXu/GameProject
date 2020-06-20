using System;

namespace Verse
{
	// Token: 0x0200025C RID: 604
	public static class HediffGrowthModeUtility
	{
		// Token: 0x06001077 RID: 4215 RVA: 0x0005E1A4 File Offset: 0x0005C3A4
		public static string GetLabel(this HediffGrowthMode m)
		{
			switch (m)
			{
			case HediffGrowthMode.Growing:
				return "HediffGrowthMode_Growing".Translate();
			case HediffGrowthMode.Stable:
				return "HediffGrowthMode_Stable".Translate();
			case HediffGrowthMode.Remission:
				return "HediffGrowthMode_Remission".Translate();
			default:
				throw new ArgumentException();
			}
		}
	}
}
