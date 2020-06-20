using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200005D RID: 93
	public abstract class CameraMapConfig
	{
		// Token: 0x0600040E RID: 1038 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void ConfigFixedUpdate_60(ref Vector3 velocity)
		{
		}

		// Token: 0x0600040F RID: 1039 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void ConfigOnGUI()
		{
		}

		// Token: 0x0400014B RID: 331
		public float dollyRateKeys = 50f;

		// Token: 0x0400014C RID: 332
		public float dollyRateScreenEdge = 35f;

		// Token: 0x0400014D RID: 333
		public float camSpeedDecayFactor = 0.85f;

		// Token: 0x0400014E RID: 334
		public float moveSpeedScale = 2f;

		// Token: 0x0400014F RID: 335
		public float zoomSpeed = 2.6f;

		// Token: 0x04000150 RID: 336
		public float minSize = 11f;

		// Token: 0x04000151 RID: 337
		public float zoomPreserveFactor;

		// Token: 0x04000152 RID: 338
		public bool smoothZoom;
	}
}
