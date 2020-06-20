using System;

namespace Verse
{
	// Token: 0x02000060 RID: 96
	public class CameraMapConfig_ContinuousPanAndZoom : CameraMapConfig_ContinuousPan
	{
		// Token: 0x06000413 RID: 1043 RVA: 0x000152C0 File Offset: 0x000134C0
		public CameraMapConfig_ContinuousPanAndZoom()
		{
			this.zoomSpeed = 0.043f;
			this.zoomPreserveFactor = 1f;
			this.smoothZoom = true;
			this.minSize = 8.2f;
		}
	}
}
