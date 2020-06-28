using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000221 RID: 545
	public static class InvisibilityMatPool
	{
		// Token: 0x06000F4D RID: 3917 RVA: 0x000580BC File Offset: 0x000562BC
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

		// Token: 0x04000B73 RID: 2931
		private static Dictionary<Material, Material> materials = new Dictionary<Material, Material>();

		// Token: 0x04000B74 RID: 2932
		private static Color color = new Color(0.1f, 0.65f, 0.8f, 0.5f);

		// Token: 0x04000B75 RID: 2933
		private static readonly int NoiseTex = Shader.PropertyToID("_NoiseTex");
	}
}
