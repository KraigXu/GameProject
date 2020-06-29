using System;
using UnityEngine;

namespace Verse
{
	
	public struct TextureAndColor
	{
		
		// (get) Token: 0x060010FF RID: 4351 RVA: 0x00060022 File Offset: 0x0005E222
		public bool HasValue
		{
			get
			{
				return this.texture != null;
			}
		}

		
		// (get) Token: 0x06001100 RID: 4352 RVA: 0x00060030 File Offset: 0x0005E230
		public Texture2D Texture
		{
			get
			{
				return this.texture;
			}
		}

		
		// (get) Token: 0x06001101 RID: 4353 RVA: 0x00060038 File Offset: 0x0005E238
		public Color Color
		{
			get
			{
				return this.color;
			}
		}

		
		public TextureAndColor(Texture2D texture, Color color)
		{
			this.texture = texture;
			this.color = color;
		}

		
		// (get) Token: 0x06001103 RID: 4355 RVA: 0x00060050 File Offset: 0x0005E250
		public static TextureAndColor None
		{
			get
			{
				return new TextureAndColor(null, Color.white);
			}
		}

		
		public static implicit operator TextureAndColor(Texture2D texture)
		{
			return new TextureAndColor(texture, Color.white);
		}

		
		private Texture2D texture;

		
		private Color color;
	}
}
