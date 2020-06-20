using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002AB RID: 683
	public static class MaterialPool
	{
		// Token: 0x06001387 RID: 4999 RVA: 0x000701D3 File Offset: 0x0006E3D3
		public static Material MatFrom(string texPath, bool reportFailure)
		{
			if (texPath == null || texPath == "null")
			{
				return null;
			}
			return MaterialPool.MatFrom(new MaterialRequest(ContentFinder<Texture2D>.Get(texPath, reportFailure)));
		}

		// Token: 0x06001388 RID: 5000 RVA: 0x000701F8 File Offset: 0x0006E3F8
		public static Material MatFrom(string texPath)
		{
			if (texPath == null || texPath == "null")
			{
				return null;
			}
			return MaterialPool.MatFrom(new MaterialRequest(ContentFinder<Texture2D>.Get(texPath, true)));
		}

		// Token: 0x06001389 RID: 5001 RVA: 0x0007021D File Offset: 0x0006E41D
		public static Material MatFrom(Texture2D srcTex)
		{
			return MaterialPool.MatFrom(new MaterialRequest(srcTex));
		}

		// Token: 0x0600138A RID: 5002 RVA: 0x0007022A File Offset: 0x0006E42A
		public static Material MatFrom(Texture2D srcTex, Shader shader, Color color)
		{
			return MaterialPool.MatFrom(new MaterialRequest(srcTex, shader, color));
		}

		// Token: 0x0600138B RID: 5003 RVA: 0x0007023C File Offset: 0x0006E43C
		public static Material MatFrom(Texture2D srcTex, Shader shader, Color color, int renderQueue)
		{
			return MaterialPool.MatFrom(new MaterialRequest(srcTex, shader, color)
			{
				renderQueue = renderQueue
			});
		}

		// Token: 0x0600138C RID: 5004 RVA: 0x00070261 File Offset: 0x0006E461
		public static Material MatFrom(string texPath, Shader shader)
		{
			return MaterialPool.MatFrom(new MaterialRequest(ContentFinder<Texture2D>.Get(texPath, true), shader));
		}

		// Token: 0x0600138D RID: 5005 RVA: 0x00070278 File Offset: 0x0006E478
		public static Material MatFrom(string texPath, Shader shader, int renderQueue)
		{
			return MaterialPool.MatFrom(new MaterialRequest(ContentFinder<Texture2D>.Get(texPath, true), shader)
			{
				renderQueue = renderQueue
			});
		}

		// Token: 0x0600138E RID: 5006 RVA: 0x000702A2 File Offset: 0x0006E4A2
		public static Material MatFrom(string texPath, Shader shader, Color color)
		{
			return MaterialPool.MatFrom(new MaterialRequest(ContentFinder<Texture2D>.Get(texPath, true), shader, color));
		}

		// Token: 0x0600138F RID: 5007 RVA: 0x000702B8 File Offset: 0x0006E4B8
		public static Material MatFrom(string texPath, Shader shader, Color color, int renderQueue)
		{
			return MaterialPool.MatFrom(new MaterialRequest(ContentFinder<Texture2D>.Get(texPath, true), shader, color)
			{
				renderQueue = renderQueue
			});
		}

		// Token: 0x06001390 RID: 5008 RVA: 0x000702E4 File Offset: 0x0006E4E4
		public static Material MatFrom(MaterialRequest req)
		{
			if (!UnityData.IsInMainThread)
			{
				Log.Error("Tried to get a material from a different thread.", false);
				return null;
			}
			if (req.mainTex == null)
			{
				Log.Error("MatFrom with null sourceTex.", false);
				return BaseContent.BadMat;
			}
			if (req.shader == null)
			{
				Log.Warning("Matfrom with null shader.", false);
				return BaseContent.BadMat;
			}
			if (req.maskTex != null && !req.shader.SupportsMaskTex())
			{
				Log.Error("MaterialRequest has maskTex but shader does not support it. req=" + req.ToString(), false);
				req.maskTex = null;
			}
			req.color = req.color;
			req.colorTwo = req.colorTwo;
			Material material;
			if (!MaterialPool.matDictionary.TryGetValue(req, out material))
			{
				material = MaterialAllocator.Create(req.shader);
				material.name = req.shader.name + "_" + req.mainTex.name;
				material.mainTexture = req.mainTex;
				material.color = req.color;
				if (req.maskTex != null)
				{
					material.SetTexture(ShaderPropertyIDs.MaskTex, req.maskTex);
					material.SetColor(ShaderPropertyIDs.ColorTwo, req.colorTwo);
				}
				if (req.renderQueue != 0)
				{
					material.renderQueue = req.renderQueue;
				}
				if (!req.shaderParameters.NullOrEmpty<ShaderParameter>())
				{
					for (int i = 0; i < req.shaderParameters.Count; i++)
					{
						req.shaderParameters[i].Apply(material);
					}
				}
				MaterialPool.matDictionary.Add(req, material);
				if (req.shader == ShaderDatabase.CutoutPlant || req.shader == ShaderDatabase.TransparentPlant)
				{
					WindManager.Notify_PlantMaterialCreated(material);
				}
			}
			return material;
		}

		// Token: 0x04000D27 RID: 3367
		private static Dictionary<MaterialRequest, Material> matDictionary = new Dictionary<MaterialRequest, Material>();
	}
}
