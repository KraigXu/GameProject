using System;

namespace Verse
{
	// Token: 0x0200026F RID: 623
	public class HediffCompProperties_SkillDecay : HediffCompProperties
	{
		// Token: 0x060010BB RID: 4283 RVA: 0x0005F05A File Offset: 0x0005D25A
		public HediffCompProperties_SkillDecay()
		{
			this.compClass = typeof(HediffComp_SkillDecay);
		}

		// Token: 0x04000C2B RID: 3115
		public SimpleCurve decayPerDayPercentageLevelCurve;
	}
}
