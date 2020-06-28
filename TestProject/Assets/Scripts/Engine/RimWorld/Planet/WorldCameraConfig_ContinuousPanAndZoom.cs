using System;

namespace RimWorld.Planet
{
	// Token: 0x020011D9 RID: 4569
	public class WorldCameraConfig_ContinuousPanAndZoom : WorldCameraConfig_ContinuousPan
	{
		// Token: 0x060069CB RID: 27083 RVA: 0x0024EA5A File Offset: 0x0024CC5A
		public WorldCameraConfig_ContinuousPanAndZoom()
		{
			this.zoomSpeed = 0.03f;
			this.zoomPreserveFactor = 1f;
			this.smoothZoom = true;
		}
	}
}
