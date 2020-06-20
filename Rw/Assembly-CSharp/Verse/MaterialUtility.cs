using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200045F RID: 1119
	public static class MaterialUtility
	{
		// Token: 0x06002140 RID: 8512 RVA: 0x000CBF73 File Offset: 0x000CA173
		public static Texture2D GetMaskTexture(this Material mat)
		{
			if (!mat.HasProperty(ShaderPropertyIDs.MaskTex))
			{
				return null;
			}
			return (Texture2D)mat.GetTexture(ShaderPropertyIDs.MaskTex);
		}

		// Token: 0x06002141 RID: 8513 RVA: 0x000CBF94 File Offset: 0x000CA194
		public static Color GetColorTwo(this Material mat)
		{
			if (!mat.HasProperty(ShaderPropertyIDs.ColorTwo))
			{
				return Color.white;
			}
			return mat.GetColor(ShaderPropertyIDs.ColorTwo);
		}
	}
}
