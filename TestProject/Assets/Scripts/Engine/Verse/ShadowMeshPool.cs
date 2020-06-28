using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002AF RID: 687
	public static class ShadowMeshPool
	{
		// Token: 0x060013A4 RID: 5028 RVA: 0x00070C5A File Offset: 0x0006EE5A
		public static Mesh GetShadowMesh(ShadowData sha)
		{
			return ShadowMeshPool.GetShadowMesh(sha.BaseX, sha.BaseZ, sha.BaseY);
		}

		// Token: 0x060013A5 RID: 5029 RVA: 0x00070C73 File Offset: 0x0006EE73
		public static Mesh GetShadowMesh(float baseEdgeLength, float tallness)
		{
			return ShadowMeshPool.GetShadowMesh(baseEdgeLength, baseEdgeLength, tallness);
		}

		// Token: 0x060013A6 RID: 5030 RVA: 0x00070C80 File Offset: 0x0006EE80
		public static Mesh GetShadowMesh(float baseWidth, float baseHeight, float tallness)
		{
			int key = ShadowMeshPool.HashOf(baseWidth, baseHeight, tallness);
			Mesh mesh;
			if (!ShadowMeshPool.shadowMeshDict.TryGetValue(key, out mesh))
			{
				mesh = MeshMakerShadows.NewShadowMesh(baseWidth, baseHeight, tallness);
				ShadowMeshPool.shadowMeshDict.Add(key, mesh);
			}
			return mesh;
		}

		// Token: 0x060013A7 RID: 5031 RVA: 0x00070CBC File Offset: 0x0006EEBC
		private static int HashOf(float baseWidth, float baseheight, float tallness)
		{
			int num = (int)(baseWidth * 1000f);
			int num2 = (int)(baseheight * 1000f);
			int num3 = (int)(tallness * 1000f);
			return num * 391 ^ 261231 ^ num2 * 612331 ^ num3 * 456123;
		}

		// Token: 0x04000D31 RID: 3377
		private static Dictionary<int, Mesh> shadowMeshDict = new Dictionary<int, Mesh>();
	}
}
