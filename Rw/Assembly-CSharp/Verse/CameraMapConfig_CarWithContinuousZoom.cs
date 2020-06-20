using System;

namespace Verse
{
	// Token: 0x02000062 RID: 98
	public class CameraMapConfig_CarWithContinuousZoom : CameraMapConfig_Car
	{
		// Token: 0x06000416 RID: 1046 RVA: 0x00015422 File Offset: 0x00013622
		public CameraMapConfig_CarWithContinuousZoom()
		{
			this.zoomSpeed = 0.043f;
			this.zoomPreserveFactor = 1f;
			this.smoothZoom = true;
		}
	}
}
