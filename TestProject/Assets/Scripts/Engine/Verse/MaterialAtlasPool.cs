using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002A9 RID: 681
	public static class MaterialAtlasPool
	{
		// Token: 0x06001383 RID: 4995 RVA: 0x000700DA File Offset: 0x0006E2DA
		public static Material SubMaterialFromAtlas(Material mat, LinkDirections LinkSet)
		{
			if (!MaterialAtlasPool.atlasDict.ContainsKey(mat))
			{
				MaterialAtlasPool.atlasDict.Add(mat, new MaterialAtlasPool.MaterialAtlas(mat));
			}
			return MaterialAtlasPool.atlasDict[mat].SubMat(LinkSet);
		}

		// Token: 0x04000D26 RID: 3366
		private static Dictionary<Material, MaterialAtlasPool.MaterialAtlas> atlasDict = new Dictionary<Material, MaterialAtlasPool.MaterialAtlas>();

		// Token: 0x02001479 RID: 5241
		private class MaterialAtlas
		{
			// Token: 0x06007ACD RID: 31437 RVA: 0x002992E4 File Offset: 0x002974E4
			public MaterialAtlas(Material newRootMat)
			{
				Vector2 mainTextureScale = new Vector2(0.1875f, 0.1875f);
				for (int i = 0; i < 16; i++)
				{
					float x = (float)(i % 4) * 0.25f + 0.03125f;
					float y = (float)(i / 4) * 0.25f + 0.03125f;
					Vector2 mainTextureOffset = new Vector2(x, y);
					Material material = MaterialAllocator.Create(newRootMat);
					material.name = newRootMat.name + "_ASM" + i;
					material.mainTextureScale = mainTextureScale;
					material.mainTextureOffset = mainTextureOffset;
					this.subMats[i] = material;
				}
			}

			// Token: 0x06007ACE RID: 31438 RVA: 0x00299390 File Offset: 0x00297590
			public Material SubMat(LinkDirections linkSet)
			{
				if ((int)linkSet >= this.subMats.Length)
				{
					Log.Warning("Cannot get submat of index " + (int)linkSet + ": out of range.", false);
					return BaseContent.BadMat;
				}
				return this.subMats[(int)linkSet];
			}

			// Token: 0x04004DAB RID: 19883
			protected Material[] subMats = new Material[16];

			// Token: 0x04004DAC RID: 19884
			private const float TexPadding = 0.03125f;
		}
	}
}
