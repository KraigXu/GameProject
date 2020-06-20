using System;

namespace Verse
{
	// Token: 0x02000088 RID: 136
	public class CompProperties_HeatPusher : CompProperties
	{
		// Token: 0x060004C3 RID: 1219 RVA: 0x00017DEB File Offset: 0x00015FEB
		public CompProperties_HeatPusher()
		{
			this.compClass = typeof(CompHeatPusher);
		}

		// Token: 0x04000220 RID: 544
		public float heatPerSecond;

		// Token: 0x04000221 RID: 545
		public float heatPushMaxTemperature = 99999f;

		// Token: 0x04000222 RID: 546
		public float heatPushMinTemperature = -99999f;
	}
}
