using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000ED RID: 237
	public class SubcameraDef : Def
	{
		// Token: 0x17000134 RID: 308
		// (get) Token: 0x0600065E RID: 1630 RVA: 0x0001E2C0 File Offset: 0x0001C4C0
		public int LayerId
		{
			get
			{
				if (this.layerCached == -1)
				{
					this.layerCached = LayerMask.NameToLayer(this.layer);
				}
				return this.layerCached;
			}
		}

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x0600065F RID: 1631 RVA: 0x0001E2E4 File Offset: 0x0001C4E4
		public RenderTextureFormat BestFormat
		{
			get
			{
				if (SystemInfo.SupportsRenderTextureFormat(this.format))
				{
					return this.format;
				}
				if (this.format == RenderTextureFormat.R8 && SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.RG16))
				{
					return RenderTextureFormat.RG16;
				}
				if ((this.format == RenderTextureFormat.R8 || this.format == RenderTextureFormat.RG16) && SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGB32))
				{
					return RenderTextureFormat.ARGB32;
				}
				if ((this.format == RenderTextureFormat.R8 || this.format == RenderTextureFormat.RHalf || this.format == RenderTextureFormat.RFloat) && SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.RGFloat))
				{
					return RenderTextureFormat.RGFloat;
				}
				if ((this.format == RenderTextureFormat.R8 || this.format == RenderTextureFormat.RHalf || this.format == RenderTextureFormat.RFloat || this.format == RenderTextureFormat.RGFloat) && SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBFloat))
				{
					return RenderTextureFormat.ARGBFloat;
				}
				return this.format;
			}
		}

		// Token: 0x04000581 RID: 1409
		[NoTranslate]
		public string layer;

		// Token: 0x04000582 RID: 1410
		public int depth;

		// Token: 0x04000583 RID: 1411
		public RenderTextureFormat format;

		// Token: 0x04000584 RID: 1412
		[Unsaved(false)]
		private int layerCached = -1;
	}
}
