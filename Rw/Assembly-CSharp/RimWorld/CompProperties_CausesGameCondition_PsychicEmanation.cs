using System;

namespace RimWorld
{
	// Token: 0x02000CDF RID: 3295
	public class CompProperties_CausesGameCondition_PsychicEmanation : CompProperties_CausesGameCondition
	{
		// Token: 0x06004FED RID: 20461 RVA: 0x001AF5BB File Offset: 0x001AD7BB
		public CompProperties_CausesGameCondition_PsychicEmanation()
		{
			this.compClass = typeof(CompCauseGameCondition_PsychicEmanation);
		}

		// Token: 0x04002CB0 RID: 11440
		public PsychicDroneLevel droneLevel = PsychicDroneLevel.BadHigh;

		// Token: 0x04002CB1 RID: 11441
		public int droneLevelIncreaseInterval = int.MinValue;
	}
}
