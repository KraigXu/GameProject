using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000464 RID: 1124
	public static class SolidColorMaterials
	{
		// Token: 0x1700066D RID: 1645
		// (get) Token: 0x06002148 RID: 8520 RVA: 0x000CC29E File Offset: 0x000CA49E
		public static int SimpleColorMatCount
		{
			get
			{
				return SolidColorMaterials.simpleColorMats.Count + SolidColorMaterials.simpleColorAndVertexColorMats.Count;
			}
		}

		// Token: 0x06002149 RID: 8521 RVA: 0x000CC2B8 File Offset: 0x000CA4B8
		public static Material SimpleSolidColorMaterial(Color col, bool careAboutVertexColors = false)
		{
			col = col;
			Material material;
			if (careAboutVertexColors)
			{
				if (!SolidColorMaterials.simpleColorAndVertexColorMats.TryGetValue(col, out material))
				{
					material = SolidColorMaterials.NewSolidColorMaterial(col, ShaderDatabase.VertexColor);
					SolidColorMaterials.simpleColorAndVertexColorMats.Add(col, material);
				}
			}
			else if (!SolidColorMaterials.simpleColorMats.TryGetValue(col, out material))
			{
				material = SolidColorMaterials.NewSolidColorMaterial(col, ShaderDatabase.SolidColor);
				SolidColorMaterials.simpleColorMats.Add(col, material);
			}
			return material;
		}

		// Token: 0x0600214A RID: 8522 RVA: 0x000CC328 File Offset: 0x000CA528
		public static Material NewSolidColorMaterial(Color col, Shader shader)
		{
			if (!UnityData.IsInMainThread)
			{
				Log.Error("Tried to create a material from a different thread.", false);
				return null;
			}
			Material material = MaterialAllocator.Create(shader);
			material.color = col;
			material.name = string.Concat(new object[]
			{
				"SolidColorMat-",
				shader.name,
				"-",
				col
			});
			return material;
		}

		// Token: 0x0600214B RID: 8523 RVA: 0x000CC38B File Offset: 0x000CA58B
		public static Texture2D NewSolidColorTexture(float r, float g, float b, float a)
		{
			return SolidColorMaterials.NewSolidColorTexture(new Color(r, g, b, a));
		}

		// Token: 0x0600214C RID: 8524 RVA: 0x000CC39C File Offset: 0x000CA59C
		public static Texture2D NewSolidColorTexture(Color color)
		{
			if (!UnityData.IsInMainThread)
			{
				Log.Error("Tried to create a texture from a different thread.", false);
				return null;
			}
			Texture2D texture2D = new Texture2D(1, 1);
			texture2D.name = "SolidColorTex-" + color;
			texture2D.SetPixel(0, 0, color);
			texture2D.Apply();
			return texture2D;
		}

		// Token: 0x04001462 RID: 5218
		private static Dictionary<Color, Material> simpleColorMats = new Dictionary<Color, Material>();

		// Token: 0x04001463 RID: 5219
		private static Dictionary<Color, Material> simpleColorAndVertexColorMats = new Dictionary<Color, Material>();
	}
}
