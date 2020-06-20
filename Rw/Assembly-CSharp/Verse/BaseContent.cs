using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200002D RID: 45
	[StaticConstructorOnStartup]
	public static class BaseContent
	{
		// Token: 0x060002E6 RID: 742 RVA: 0x0000EFC0 File Offset: 0x0000D1C0
		public static bool NullOrBad(this Material mat)
		{
			return mat == null || mat == BaseContent.BadMat;
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x0000EFD8 File Offset: 0x0000D1D8
		public static bool NullOrBad(this Texture2D tex)
		{
			return tex == null || tex == BaseContent.BadTex;
		}

		// Token: 0x04000083 RID: 131
		public static readonly string BadTexPath = "UI/Misc/BadTexture";

		// Token: 0x04000084 RID: 132
		public static readonly string PlaceholderImagePath = "PlaceholderImage";

		// Token: 0x04000085 RID: 133
		public static readonly Material BadMat = MaterialPool.MatFrom(BaseContent.BadTexPath, ShaderDatabase.Cutout);

		// Token: 0x04000086 RID: 134
		public static readonly Texture2D BadTex = ContentFinder<Texture2D>.Get(BaseContent.BadTexPath, true);

		// Token: 0x04000087 RID: 135
		public static readonly Graphic BadGraphic = GraphicDatabase.Get<Graphic_Single>(BaseContent.BadTexPath);

		// Token: 0x04000088 RID: 136
		public static readonly Texture2D BlackTex = SolidColorMaterials.NewSolidColorTexture(Color.black);

		// Token: 0x04000089 RID: 137
		public static readonly Texture2D GreyTex = SolidColorMaterials.NewSolidColorTexture(Color.grey);

		// Token: 0x0400008A RID: 138
		public static readonly Texture2D WhiteTex = SolidColorMaterials.NewSolidColorTexture(Color.white);

		// Token: 0x0400008B RID: 139
		public static readonly Texture2D ClearTex = SolidColorMaterials.NewSolidColorTexture(Color.clear);

		// Token: 0x0400008C RID: 140
		public static readonly Texture2D YellowTex = SolidColorMaterials.NewSolidColorTexture(Color.yellow);

		// Token: 0x0400008D RID: 141
		public static readonly Material BlackMat = SolidColorMaterials.SimpleSolidColorMaterial(Color.black, false);

		// Token: 0x0400008E RID: 142
		public static readonly Material WhiteMat = SolidColorMaterials.SimpleSolidColorMaterial(Color.white, false);

		// Token: 0x0400008F RID: 143
		public static readonly Material ClearMat = SolidColorMaterials.SimpleSolidColorMaterial(Color.clear, false);
	}
}
