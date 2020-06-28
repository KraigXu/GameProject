using System;

namespace RimWorld.Planet
{
	// Token: 0x020011DB RID: 4571
	public class WorldCameraConfig_CarWithContinuousZoom : WorldCameraConfig_Car
	{
		// Token: 0x060069CE RID: 27086 RVA: 0x0024EBB2 File Offset: 0x0024CDB2
		public WorldCameraConfig_CarWithContinuousZoom()
		{
			this.zoomSpeed = 0.03f;
			this.zoomPreserveFactor = 1f;
			this.smoothZoom = true;
		}
	}
}
