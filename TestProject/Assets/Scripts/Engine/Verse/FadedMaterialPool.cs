using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

namespace Verse
{
	// Token: 0x020002A7 RID: 679
	public static class FadedMaterialPool
	{
		// Token: 0x1700041C RID: 1052
		// (get) Token: 0x06001375 RID: 4981 RVA: 0x0006FBB8 File Offset: 0x0006DDB8
		public static int TotalMaterialCount
		{
			get
			{
				return FadedMaterialPool.cachedMats.Count;
			}
		}

		// Token: 0x1700041D RID: 1053
		// (get) Token: 0x06001376 RID: 4982 RVA: 0x0006FBC4 File Offset: 0x0006DDC4
		public static long TotalMaterialBytes
		{
			get
			{
				long num = 0L;
				foreach (KeyValuePair<FadedMaterialPool.FadedMatRequest, Material> keyValuePair in FadedMaterialPool.cachedMats)
				{
					num += Profiler.GetRuntimeMemorySizeLong(keyValuePair.Value);
				}
				return num;
			}
		}

		// Token: 0x06001377 RID: 4983 RVA: 0x0006FC24 File Offset: 0x0006DE24
		public static Material FadedVersionOf(Material sourceMat, float alpha)
		{
			int num = FadedMaterialPool.IndexFromAlpha(alpha);
			if (num == 0)
			{
				return BaseContent.ClearMat;
			}
			if (num == 29)
			{
				return sourceMat;
			}
			FadedMaterialPool.FadedMatRequest key = new FadedMaterialPool.FadedMatRequest(sourceMat, num);
			Material material;
			if (!FadedMaterialPool.cachedMats.TryGetValue(key, out material))
			{
				material = MaterialAllocator.Create(sourceMat);
				material.color = new Color(1f, 1f, 1f, (float)FadedMaterialPool.IndexFromAlpha(alpha) / 30f);
				FadedMaterialPool.cachedMats.Add(key, material);
			}
			return material;
		}

		// Token: 0x06001378 RID: 4984 RVA: 0x0006FC9C File Offset: 0x0006DE9C
		private static int IndexFromAlpha(float alpha)
		{
			int num = Mathf.FloorToInt(alpha * 30f);
			if (num == 30)
			{
				num = 29;
			}
			return num;
		}

		// Token: 0x04000D21 RID: 3361
		private static Dictionary<FadedMaterialPool.FadedMatRequest, Material> cachedMats = new Dictionary<FadedMaterialPool.FadedMatRequest, Material>(FadedMaterialPool.FadedMatRequestComparer.Instance);

		// Token: 0x04000D22 RID: 3362
		private const int NumFadeSteps = 30;

		// Token: 0x02001474 RID: 5236
		private struct FadedMatRequest : IEquatable<FadedMaterialPool.FadedMatRequest>
		{
			// Token: 0x06007AB7 RID: 31415 RVA: 0x00299197 File Offset: 0x00297397
			public FadedMatRequest(Material mat, int alphaIndex)
			{
				this.mat = mat;
				this.alphaIndex = alphaIndex;
			}

			// Token: 0x06007AB8 RID: 31416 RVA: 0x002991A7 File Offset: 0x002973A7
			public override bool Equals(object obj)
			{
				return obj != null && obj is FadedMaterialPool.FadedMatRequest && this.Equals((FadedMaterialPool.FadedMatRequest)obj);
			}

			// Token: 0x06007AB9 RID: 31417 RVA: 0x002991C2 File Offset: 0x002973C2
			public bool Equals(FadedMaterialPool.FadedMatRequest other)
			{
				return this.mat == other.mat && this.alphaIndex == other.alphaIndex;
			}

			// Token: 0x06007ABA RID: 31418 RVA: 0x002991E7 File Offset: 0x002973E7
			public override int GetHashCode()
			{
				return Gen.HashCombineInt(this.mat.GetHashCode(), this.alphaIndex);
			}

			// Token: 0x06007ABB RID: 31419 RVA: 0x002991FF File Offset: 0x002973FF
			public static bool operator ==(FadedMaterialPool.FadedMatRequest lhs, FadedMaterialPool.FadedMatRequest rhs)
			{
				return lhs.Equals(rhs);
			}

			// Token: 0x06007ABC RID: 31420 RVA: 0x00299209 File Offset: 0x00297409
			public static bool operator !=(FadedMaterialPool.FadedMatRequest lhs, FadedMaterialPool.FadedMatRequest rhs)
			{
				return !(lhs == rhs);
			}

			// Token: 0x04004D9D RID: 19869
			private Material mat;

			// Token: 0x04004D9E RID: 19870
			private int alphaIndex;
		}

		// Token: 0x02001475 RID: 5237
		private class FadedMatRequestComparer : IEqualityComparer<FadedMaterialPool.FadedMatRequest>
		{
			// Token: 0x06007ABD RID: 31421 RVA: 0x00299215 File Offset: 0x00297415
			public bool Equals(FadedMaterialPool.FadedMatRequest x, FadedMaterialPool.FadedMatRequest y)
			{
				return x.Equals(y);
			}

			// Token: 0x06007ABE RID: 31422 RVA: 0x0029921F File Offset: 0x0029741F
			public int GetHashCode(FadedMaterialPool.FadedMatRequest obj)
			{
				return obj.GetHashCode();
			}

			// Token: 0x04004D9F RID: 19871
			public static readonly FadedMaterialPool.FadedMatRequestComparer Instance = new FadedMaterialPool.FadedMatRequestComparer();
		}
	}
}
