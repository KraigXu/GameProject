using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D08 RID: 3336
	public class CompProperties_FadesInOut : CompProperties
	{
		// Token: 0x0600512F RID: 20783 RVA: 0x001B3E1C File Offset: 0x001B201C
		public CompProperties_FadesInOut()
		{
			this.compClass = typeof(CompFadesInOut);
		}

		// Token: 0x04002CFB RID: 11515
		public float fadeInSecs;

		// Token: 0x04002CFC RID: 11516
		public float fadeOutSecs;

		// Token: 0x04002CFD RID: 11517
		public float solidTimeSecs;
	}
}
