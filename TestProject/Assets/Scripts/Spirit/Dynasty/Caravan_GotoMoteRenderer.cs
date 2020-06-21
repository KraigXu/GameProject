using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001227 RID: 4647
	[StaticConstructorOnStartup]
	public class Caravan_GotoMoteRenderer
	{
		// Token: 0x06006C1F RID: 27679 RVA: 0x0025ADBC File Offset: 0x00258FBC
		public void RenderMote()
		{
			float num = (Time.time - this.lastOrderedToTileTime) / 0.5f;
			if (num > 1f)
			{
				return;
			}
			if (Caravan_GotoMoteRenderer.cachedMaterial == null)
			{
				Caravan_GotoMoteRenderer.cachedMaterial = MaterialPool.MatFrom((Texture2D)Caravan_GotoMoteRenderer.FeedbackGoto.mainTexture, Caravan_GotoMoteRenderer.FeedbackGoto.shader, Color.white, WorldMaterials.DynamicObjectRenderQueue);
			}
			WorldGrid worldGrid = Find.WorldGrid;
			Vector3 tileCenter = worldGrid.GetTileCenter(this.tile);
			Color value = new Color(1f, 1f, 1f, 1f - num);
			Caravan_GotoMoteRenderer.propertyBlock.SetColor(ShaderPropertyIDs.Color, value);
			WorldRendererUtility.DrawQuadTangentialToPlanet(tileCenter, 0.8f * worldGrid.averageTileSize, 0.018f, Caravan_GotoMoteRenderer.cachedMaterial, false, false, Caravan_GotoMoteRenderer.propertyBlock);
		}

		// Token: 0x06006C20 RID: 27680 RVA: 0x0025AE80 File Offset: 0x00259080
		public void OrderedToTile(int tile)
		{
			this.tile = tile;
			this.lastOrderedToTileTime = Time.time;
		}

		// Token: 0x04004354 RID: 17236
		private int tile;

		// Token: 0x04004355 RID: 17237
		private float lastOrderedToTileTime = -0.51f;

		// Token: 0x04004356 RID: 17238
		private static MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();

		// Token: 0x04004357 RID: 17239
		private static Material cachedMaterial;

		// Token: 0x04004358 RID: 17240
		public static readonly Material FeedbackGoto = MaterialPool.MatFrom("Things/Mote/FeedbackGoto", ShaderDatabase.WorldOverlayTransparent, WorldMaterials.DynamicObjectRenderQueue);

		// Token: 0x04004359 RID: 17241
		private const float Duration = 0.5f;

		// Token: 0x0400435A RID: 17242
		private const float BaseSize = 0.8f;
	}
}
