using System;
using UnityEngine;

namespace RimWorld.Planet
{
	// Token: 0x020011D6 RID: 4566
	public abstract class WorldCameraConfig
	{
		// Token: 0x060069C6 RID: 27078 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void ConfigFixedUpdate_60(ref Vector2 rotationVelocity)
		{
		}

		// Token: 0x060069C7 RID: 27079 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void ConfigOnGUI()
		{
		}

		// Token: 0x040041C5 RID: 16837
		public float dollyRateKeys = 170f;

		// Token: 0x040041C6 RID: 16838
		public float dollyRateScreenEdge = 125f;

		// Token: 0x040041C7 RID: 16839
		public float camRotationDecayFactor = 0.9f;

		// Token: 0x040041C8 RID: 16840
		public float rotationSpeedScale = 0.3f;

		// Token: 0x040041C9 RID: 16841
		public float zoomSpeed = 2.6f;

		// Token: 0x040041CA RID: 16842
		public float zoomPreserveFactor;

		// Token: 0x040041CB RID: 16843
		public bool smoothZoom;
	}
}
