using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200087C RID: 2172
	public class CompProperties_TempControl : CompProperties
	{
		// Token: 0x0600353E RID: 13630 RVA: 0x001230D0 File Offset: 0x001212D0
		public CompProperties_TempControl()
		{
			this.compClass = typeof(CompTempControl);
		}

		// Token: 0x04001C9B RID: 7323
		public float energyPerSecond = 12f;

		// Token: 0x04001C9C RID: 7324
		public float defaultTargetTemperature = 21f;

		// Token: 0x04001C9D RID: 7325
		public float minTargetTemperature = -50f;

		// Token: 0x04001C9E RID: 7326
		public float maxTargetTemperature = 50f;

		// Token: 0x04001C9F RID: 7327
		public float lowPowerConsumptionFactor = 0.1f;
	}
}
