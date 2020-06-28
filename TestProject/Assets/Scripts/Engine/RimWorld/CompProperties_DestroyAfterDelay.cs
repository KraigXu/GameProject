using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D00 RID: 3328
	public class CompProperties_DestroyAfterDelay : CompProperties
	{
		// Token: 0x060050EF RID: 20719 RVA: 0x001B2961 File Offset: 0x001B0B61
		public CompProperties_DestroyAfterDelay()
		{
			this.compClass = typeof(CompDestroyAfterDelay);
		}

		// Token: 0x04002CEC RID: 11500
		public int delayTicks;
	}
}
