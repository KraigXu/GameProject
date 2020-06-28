using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B31 RID: 2865
	public class PortraitRenderer : MonoBehaviour
	{
		// Token: 0x06004396 RID: 17302 RVA: 0x0016C474 File Offset: 0x0016A674
		public void RenderPortrait(Pawn pawn, RenderTexture renderTexture, Vector3 cameraOffset, float cameraZoom)
		{
			Camera portraitCamera = Find.PortraitCamera;
			portraitCamera.targetTexture = renderTexture;
			Vector3 position = portraitCamera.transform.position;
			float orthographicSize = portraitCamera.orthographicSize;
			portraitCamera.transform.position += cameraOffset;
			portraitCamera.orthographicSize = 1f / cameraZoom;
			this.pawn = pawn;
			portraitCamera.Render();
			this.pawn = null;
			portraitCamera.transform.position = position;
			portraitCamera.orthographicSize = orthographicSize;
			portraitCamera.targetTexture = null;
		}

		// Token: 0x06004397 RID: 17303 RVA: 0x0016C4F2 File Offset: 0x0016A6F2
		public void OnPostRender()
		{
			this.pawn.Drawer.renderer.RenderPortrait();
		}

		// Token: 0x040026C0 RID: 9920
		private Pawn pawn;
	}
}
