using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200027B RID: 635
	public struct TextureAndColor
	{
		// Token: 0x1700036D RID: 877
		// (get) Token: 0x060010FF RID: 4351 RVA: 0x00060022 File Offset: 0x0005E222
		public bool HasValue
		{
			get
			{
				return this.texture != null;
			}
		}

		// Token: 0x1700036E RID: 878
		// (get) Token: 0x06001100 RID: 4352 RVA: 0x00060030 File Offset: 0x0005E230
		public Texture2D Texture
		{
			get
			{
				return this.texture;
			}
		}

		// Token: 0x1700036F RID: 879
		// (get) Token: 0x06001101 RID: 4353 RVA: 0x00060038 File Offset: 0x0005E238
		public Color Color
		{
			get
			{
				return this.color;
			}
		}

		// Token: 0x06001102 RID: 4354 RVA: 0x00060040 File Offset: 0x0005E240
		public TextureAndColor(Texture2D texture, Color color)
		{
			this.texture = texture;
			this.color = color;
		}

		// Token: 0x17000370 RID: 880
		// (get) Token: 0x06001103 RID: 4355 RVA: 0x00060050 File Offset: 0x0005E250
		public static TextureAndColor None
		{
			get
			{
				return new TextureAndColor(null, Color.white);
			}
		}

		// Token: 0x06001104 RID: 4356 RVA: 0x0006005D File Offset: 0x0005E25D
		public static implicit operator TextureAndColor(Texture2D texture)
		{
			return new TextureAndColor(texture, Color.white);
		}

		// Token: 0x04000C4B RID: 3147
		private Texture2D texture;

		// Token: 0x04000C4C RID: 3148
		private Color color;
	}
}
