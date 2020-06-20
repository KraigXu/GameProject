using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200086F RID: 2159
	public class CompProperties_Glower : CompProperties
	{
		// Token: 0x06003526 RID: 13606 RVA: 0x00122E44 File Offset: 0x00121044
		public CompProperties_Glower()
		{
			this.compClass = typeof(CompGlower);
		}

		// Token: 0x04001C73 RID: 7283
		public float overlightRadius;

		// Token: 0x04001C74 RID: 7284
		public float glowRadius = 14f;

		// Token: 0x04001C75 RID: 7285
		public ColorInt glowColor = new ColorInt(255, 255, 255, 0) * 1.45f;
	}
}
