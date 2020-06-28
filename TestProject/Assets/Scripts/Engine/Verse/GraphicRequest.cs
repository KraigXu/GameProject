using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002F8 RID: 760
	public struct GraphicRequest : IEquatable<GraphicRequest>
	{
		// Token: 0x06001578 RID: 5496 RVA: 0x0007D6EC File Offset: 0x0007B8EC
		public GraphicRequest(Type graphicClass, string path, Shader shader, Vector2 drawSize, Color color, Color colorTwo, GraphicData graphicData, int renderQueue, List<ShaderParameter> shaderParameters)
		{
			this.graphicClass = graphicClass;
			this.path = path;
			this.shader = shader;
			this.drawSize = drawSize;
			this.color = color;
			this.colorTwo = colorTwo;
			this.graphicData = graphicData;
			this.renderQueue = renderQueue;
			this.shaderParameters = (shaderParameters.NullOrEmpty<ShaderParameter>() ? null : shaderParameters);
		}

		// Token: 0x06001579 RID: 5497 RVA: 0x0007D74C File Offset: 0x0007B94C
		public override int GetHashCode()
		{
			if (this.path == null)
			{
				this.path = BaseContent.BadTexPath;
			}
			return Gen.HashCombine<List<ShaderParameter>>(Gen.HashCombine<int>(Gen.HashCombine<GraphicData>(Gen.HashCombineStruct<Color>(Gen.HashCombineStruct<Color>(Gen.HashCombineStruct<Vector2>(Gen.HashCombine<Shader>(Gen.HashCombine<string>(Gen.HashCombine<Type>(0, this.graphicClass), this.path), this.shader), this.drawSize), this.color), this.colorTwo), this.graphicData), this.renderQueue), this.shaderParameters);
		}

		// Token: 0x0600157A RID: 5498 RVA: 0x0007D7D0 File Offset: 0x0007B9D0
		public override bool Equals(object obj)
		{
			return obj is GraphicRequest && this.Equals((GraphicRequest)obj);
		}

		// Token: 0x0600157B RID: 5499 RVA: 0x0007D7E8 File Offset: 0x0007B9E8
		public bool Equals(GraphicRequest other)
		{
			return this.graphicClass == other.graphicClass && this.path == other.path && this.shader == other.shader && this.drawSize == other.drawSize && this.color == other.color && this.colorTwo == other.colorTwo && this.graphicData == other.graphicData && this.renderQueue == other.renderQueue && this.shaderParameters == other.shaderParameters;
		}

		// Token: 0x0600157C RID: 5500 RVA: 0x0007D896 File Offset: 0x0007BA96
		public static bool operator ==(GraphicRequest lhs, GraphicRequest rhs)
		{
			return lhs.Equals(rhs);
		}

		// Token: 0x0600157D RID: 5501 RVA: 0x0007D8A0 File Offset: 0x0007BAA0
		public static bool operator !=(GraphicRequest lhs, GraphicRequest rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x04000E0C RID: 3596
		public Type graphicClass;

		// Token: 0x04000E0D RID: 3597
		public string path;

		// Token: 0x04000E0E RID: 3598
		public Shader shader;

		// Token: 0x04000E0F RID: 3599
		public Vector2 drawSize;

		// Token: 0x04000E10 RID: 3600
		public Color color;

		// Token: 0x04000E11 RID: 3601
		public Color colorTwo;

		// Token: 0x04000E12 RID: 3602
		public GraphicData graphicData;

		// Token: 0x04000E13 RID: 3603
		public int renderQueue;

		// Token: 0x04000E14 RID: 3604
		public List<ShaderParameter> shaderParameters;
	}
}
