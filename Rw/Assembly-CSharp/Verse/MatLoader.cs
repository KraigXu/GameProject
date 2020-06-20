using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000460 RID: 1120
	public static class MatLoader
	{
		// Token: 0x06002142 RID: 8514 RVA: 0x000CBFB4 File Offset: 0x000CA1B4
		public static Material LoadMat(string matPath, int renderQueue = -1)
		{
			Material material = (Material)Resources.Load("Materials/" + matPath, typeof(Material));
			if (material == null)
			{
				Log.Warning("Could not load material " + matPath, false);
			}
			MatLoader.Request key = new MatLoader.Request
			{
				path = matPath,
				renderQueue = renderQueue
			};
			Material material2;
			if (!MatLoader.dict.TryGetValue(key, out material2))
			{
				material2 = MaterialAllocator.Create(material);
				if (renderQueue != -1)
				{
					material2.renderQueue = renderQueue;
				}
				MatLoader.dict.Add(key, material2);
			}
			return material2;
		}

		// Token: 0x04001442 RID: 5186
		private static Dictionary<MatLoader.Request, Material> dict = new Dictionary<MatLoader.Request, Material>();

		// Token: 0x020016AE RID: 5806
		private struct Request
		{
			// Token: 0x06008569 RID: 34153 RVA: 0x002B2913 File Offset: 0x002B0B13
			public override int GetHashCode()
			{
				return Gen.HashCombineInt(Gen.HashCombine<string>(0, this.path), this.renderQueue);
			}

			// Token: 0x0600856A RID: 34154 RVA: 0x002B292C File Offset: 0x002B0B2C
			public override bool Equals(object obj)
			{
				return obj is MatLoader.Request && this.Equals((MatLoader.Request)obj);
			}

			// Token: 0x0600856B RID: 34155 RVA: 0x002B2944 File Offset: 0x002B0B44
			public bool Equals(MatLoader.Request other)
			{
				return other.path == this.path && other.renderQueue == this.renderQueue;
			}

			// Token: 0x0600856C RID: 34156 RVA: 0x002B2969 File Offset: 0x002B0B69
			public static bool operator ==(MatLoader.Request lhs, MatLoader.Request rhs)
			{
				return lhs.Equals(rhs);
			}

			// Token: 0x0600856D RID: 34157 RVA: 0x002B2973 File Offset: 0x002B0B73
			public static bool operator !=(MatLoader.Request lhs, MatLoader.Request rhs)
			{
				return !(lhs == rhs);
			}

			// Token: 0x0600856E RID: 34158 RVA: 0x002B297F File Offset: 0x002B0B7F
			public override string ToString()
			{
				return string.Concat(new object[]
				{
					"MatLoader.Request(",
					this.path,
					", ",
					this.renderQueue,
					")"
				});
			}

			// Token: 0x040056FF RID: 22271
			public string path;

			// Token: 0x04005700 RID: 22272
			public int renderQueue;
		}
	}
}
