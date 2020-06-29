using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	
	public static class InvisibilityMatPool
	{
		
		public static Material GetInvisibleMat(Material baseMat)
		{
			Material material;
			if (!InvisibilityMatPool.materials.TryGetValue(baseMat, out material))
			{
				material = MaterialAllocator.Create(baseMat);
				material.shader = ShaderDatabase.Invisible;
				material.SetTexture(InvisibilityMatPool.NoiseTex, TexGame.RippleTex);
				material.color = InvisibilityMatPool.color;
				InvisibilityMatPool.materials.Add(baseMat, material);
			}
			return material;
		}

		
		private static Dictionary<Material, Material> materials = new Dictionary<Material, Material>();

		
		private static Color color = new Color(0.1f, 0.65f, 0.8f, 0.5f);

		
		private static readonly int NoiseTex = Shader.PropertyToID("_NoiseTex");
	}
}
