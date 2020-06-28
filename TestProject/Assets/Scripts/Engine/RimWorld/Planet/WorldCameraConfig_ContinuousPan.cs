using System;

namespace RimWorld.Planet
{
	// Token: 0x020011D8 RID: 4568
	public class WorldCameraConfig_ContinuousPan : WorldCameraConfig
	{
		// Token: 0x060069CA RID: 27082 RVA: 0x0024EA26 File Offset: 0x0024CC26
		public WorldCameraConfig_ContinuousPan()
		{
			this.dollyRateKeys = 34f;
			this.dollyRateScreenEdge = 17.85f;
			this.camRotationDecayFactor = 1f;
			this.rotationSpeedScale = 0.15f;
		}
	}
}
