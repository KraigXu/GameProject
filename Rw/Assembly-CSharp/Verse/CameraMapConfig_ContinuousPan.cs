using System;

namespace Verse
{
	// Token: 0x0200005F RID: 95
	public class CameraMapConfig_ContinuousPan : CameraMapConfig
	{
		// Token: 0x06000412 RID: 1042 RVA: 0x00015281 File Offset: 0x00013481
		public CameraMapConfig_ContinuousPan()
		{
			this.dollyRateKeys = 10f;
			this.dollyRateScreenEdge = 5f;
			this.camSpeedDecayFactor = 1f;
			this.moveSpeedScale = 1f;
			this.minSize = 8.2f;
		}
	}
}
