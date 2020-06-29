using System;
using UnityEngine;

namespace Verse
{
	
	public class SubcameraDef : Def
	{
		
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

		
		[NoTranslate]
		public string layer;

		
		public int depth;

		
		public RenderTextureFormat format;

		
		[Unsaved(false)]
		private int layerCached = -1;
	}
}
