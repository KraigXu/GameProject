using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200021C RID: 540
	public static class DamagedMatPool
	{
		// Token: 0x170002F4 RID: 756
		// (get) Token: 0x06000F28 RID: 3880 RVA: 0x00056665 File Offset: 0x00054865
		public static int MatCount
		{
			get
			{
				return DamagedMatPool.damagedMats.Count;
			}
		}

		// Token: 0x06000F29 RID: 3881 RVA: 0x00056674 File Offset: 0x00054874
		public static Material GetDamageFlashMat(Material baseMat, float damPct)
		{
			if (damPct < 0.01f)
			{
				return baseMat;
			}
			Material material;
			if (!DamagedMatPool.damagedMats.TryGetValue(baseMat, out material))
			{
				material = MaterialAllocator.Create(baseMat);
				DamagedMatPool.damagedMats.Add(baseMat, material);
			}
			Color color = Color.Lerp(baseMat.color, DamagedMatPool.DamagedMatStartingColor, damPct);
			material.color = color;
			return material;
		}

		// Token: 0x04000B3E RID: 2878
		private static Dictionary<Material, Material> damagedMats = new Dictionary<Material, Material>();

		// Token: 0x04000B3F RID: 2879
		private static readonly Color DamagedMatStartingColor = Color.red;
	}
}
