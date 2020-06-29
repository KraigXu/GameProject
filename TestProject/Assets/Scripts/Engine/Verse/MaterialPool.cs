using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	
	public static class MaterialPool
	{
		
		public static Material MatFrom(string texPath, bool reportFailure)
		{
			if (texPath == null || texPath == "null")
			{
				return null;
			}
			return MaterialPool.MatFrom(new MaterialRequest(ContentFinder<Texture2D>.Get(texPath, reportFailure)));
		}

		
		public static Material MatFrom(string texPath)
		{
			if (texPath == null || texPath == "null")
			{
				return null;
			}
			return MaterialPool.MatFrom(new MaterialRequest(ContentFinder<Texture2D>.Get(texPath, true)));
		}

		
		public static Material MatFrom(Texture2D srcTex)
		{
			return MaterialPool.MatFrom(new MaterialRequest(srcTex));
		}

		
		public static Material MatFrom(Texture2D srcTex, Shader shader, Color color)
		{
			return MaterialPool.MatFrom(new MaterialRequest(srcTex, shader, color));
		}

		
		public static Material MatFrom(Texture2D srcTex, Shader shader, Color color, int renderQueue)
		{
			return MaterialPool.MatFrom(new MaterialRequest(srcTex, shader, color)
			{
				renderQueue = renderQueue
			});
		}

		
		public static Material MatFrom(string texPath, Shader shader)
		{
			return MaterialPool.MatFrom(new MaterialRequest(ContentFinder<Texture2D>.Get(texPath, true), shader));
		}

		
		public static Material MatFrom(string texPath, Shader shader, int renderQueue)
		{
			return MaterialPool.MatFrom(new MaterialRequest(ContentFinder<Texture2D>.Get(texPath, true), shader)
			{
				renderQueue = renderQueue
			});
		}

		
		public static Material MatFrom(string texPath, Shader shader, Color color)
		{
			return MaterialPool.MatFrom(new MaterialRequest(ContentFinder<Texture2D>.Get(texPath, true), shader, color));
		}

		
		public static Material MatFrom(string texPath, Shader shader, Color color, int renderQueue)
		{
			return MaterialPool.MatFrom(new MaterialRequest(ContentFinder<Texture2D>.Get(texPath, true), shader, color)
			{
				renderQueue = renderQueue
			});
		}

		
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

		
		private static Dictionary<MaterialRequest, Material> matDictionary = new Dictionary<MaterialRequest, Material>();
	}
}
