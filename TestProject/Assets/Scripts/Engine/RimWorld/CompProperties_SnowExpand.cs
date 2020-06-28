using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200087A RID: 2170
	public class CompProperties_SnowExpand : CompProperties
	{
		// Token: 0x0600353C RID: 13628 RVA: 0x0012307E File Offset: 0x0012127E
		public CompProperties_SnowExpand()
		{
			this.compClass = typeof(CompSnowExpand);
		}

		// Token: 0x04001C90 RID: 7312
		public int expandInterval = 500;

		// Token: 0x04001C91 RID: 7313
		public float addAmount = 0.12f;

		// Token: 0x04001C92 RID: 7314
		public float maxRadius = 55f;
	}
}
