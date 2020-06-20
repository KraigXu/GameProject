using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FAD RID: 4013
	[DefOf]
	public static class ShaderTypeDefOf
	{
		// Token: 0x060060B4 RID: 24756 RVA: 0x00217354 File Offset: 0x00215554
		static ShaderTypeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ShaderTypeDef));
		}

		// Token: 0x04003ACD RID: 15053
		public static ShaderTypeDef Cutout;

		// Token: 0x04003ACE RID: 15054
		public static ShaderTypeDef CutoutComplex;

		// Token: 0x04003ACF RID: 15055
		public static ShaderTypeDef Transparent;

		// Token: 0x04003AD0 RID: 15056
		public static ShaderTypeDef MetaOverlay;

		// Token: 0x04003AD1 RID: 15057
		public static ShaderTypeDef EdgeDetect;
	}
}
