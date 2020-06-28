using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200045E RID: 1118
	[StaticConstructorOnStartup]
	public static class MatBases
	{
		// Token: 0x0400143B RID: 5179
		public static readonly Material LightOverlay = MatLoader.LoadMat("Lighting/LightOverlay", -1);

		// Token: 0x0400143C RID: 5180
		public static readonly Material SunShadow = MatLoader.LoadMat("Lighting/SunShadow", -1);

		// Token: 0x0400143D RID: 5181
		public static readonly Material SunShadowFade = MatBases.SunShadow;

		// Token: 0x0400143E RID: 5182
		public static readonly Material EdgeShadow = MatLoader.LoadMat("Lighting/EdgeShadow", -1);

		// Token: 0x0400143F RID: 5183
		public static readonly Material IndoorMask = MatLoader.LoadMat("Misc/IndoorMask", -1);

		// Token: 0x04001440 RID: 5184
		public static readonly Material FogOfWar = MatLoader.LoadMat("Misc/FogOfWar", -1);

		// Token: 0x04001441 RID: 5185
		public static readonly Material Snow = MatLoader.LoadMat("Misc/Snow", -1);
	}
}
