using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002AC RID: 684
	public struct MaterialRequest : IEquatable<MaterialRequest>
	{
		// Token: 0x1700041E RID: 1054
		// (set) Token: 0x06001392 RID: 5010 RVA: 0x000704C9 File Offset: 0x0006E6C9
		public string BaseTexPath
		{
			set
			{
				this.mainTex = ContentFinder<Texture2D>.Get(value, true);
			}
		}

		// Token: 0x06001393 RID: 5011 RVA: 0x000704D8 File Offset: 0x0006E6D8
		public MaterialRequest(Texture2D tex)
		{
			this.shader = ShaderDatabase.Cutout;
			this.mainTex = tex;
			this.color = Color.white;
			this.colorTwo = Color.white;
			this.maskTex = null;
			this.renderQueue = 0;
			this.shaderParameters = null;
		}

		// Token: 0x06001394 RID: 5012 RVA: 0x00070517 File Offset: 0x0006E717
		public MaterialRequest(Texture2D tex, Shader shader)
		{
			this.shader = shader;
			this.mainTex = tex;
			this.color = Color.white;
			this.colorTwo = Color.white;
			this.maskTex = null;
			this.renderQueue = 0;
			this.shaderParameters = null;
		}

		// Token: 0x06001395 RID: 5013 RVA: 0x00070552 File Offset: 0x0006E752
		public MaterialRequest(Texture2D tex, Shader shader, Color color)
		{
			this.shader = shader;
			this.mainTex = tex;
			this.color = color;
			this.colorTwo = Color.white;
			this.maskTex = null;
			this.renderQueue = 0;
			this.shaderParameters = null;
		}

		// Token: 0x06001396 RID: 5014 RVA: 0x0007058C File Offset: 0x0006E78C
		public override int GetHashCode()
		{
			return Gen.HashCombine<List<ShaderParameter>>(Gen.HashCombineInt(Gen.HashCombine<Texture2D>(Gen.HashCombine<Texture2D>(Gen.HashCombineStruct<Color>(Gen.HashCombineStruct<Color>(Gen.HashCombine<Shader>(0, this.shader), this.color), this.colorTwo), this.mainTex), this.maskTex), this.renderQueue), this.shaderParameters);
		}

		// Token: 0x06001397 RID: 5015 RVA: 0x000705E7 File Offset: 0x0006E7E7
		public override bool Equals(object obj)
		{
			return obj is MaterialRequest && this.Equals((MaterialRequest)obj);
		}

		// Token: 0x06001398 RID: 5016 RVA: 0x00070600 File Offset: 0x0006E800
		public bool Equals(MaterialRequest other)
		{
			return other.shader == this.shader && other.mainTex == this.mainTex && other.color == this.color && other.colorTwo == this.colorTwo && other.maskTex == this.maskTex && other.renderQueue == this.renderQueue && other.shaderParameters == this.shaderParameters;
		}

		// Token: 0x06001399 RID: 5017 RVA: 0x0007068A File Offset: 0x0006E88A
		public static bool operator ==(MaterialRequest lhs, MaterialRequest rhs)
		{
			return lhs.Equals(rhs);
		}

		// Token: 0x0600139A RID: 5018 RVA: 0x00070694 File Offset: 0x0006E894
		public static bool operator !=(MaterialRequest lhs, MaterialRequest rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x0600139B RID: 5019 RVA: 0x000706A0 File Offset: 0x0006E8A0
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"MaterialRequest(",
				this.shader.name,
				", ",
				this.mainTex.name,
				", ",
				this.color.ToString(),
				", ",
				this.colorTwo.ToString(),
				", ",
				this.maskTex.ToString(),
				", ",
				this.renderQueue.ToString(),
				")"
			});
		}

		// Token: 0x04000D28 RID: 3368
		public Shader shader;

		// Token: 0x04000D29 RID: 3369
		public Texture2D mainTex;

		// Token: 0x04000D2A RID: 3370
		public Color color;

		// Token: 0x04000D2B RID: 3371
		public Color colorTwo;

		// Token: 0x04000D2C RID: 3372
		public Texture2D maskTex;

		// Token: 0x04000D2D RID: 3373
		public int renderQueue;

		// Token: 0x04000D2E RID: 3374
		public List<ShaderParameter> shaderParameters;
	}
}
