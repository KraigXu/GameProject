using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D49 RID: 3401
	public class CompProperties_RoyalImplant : CompProperties
	{
		// Token: 0x060052BD RID: 21181 RVA: 0x001BA40E File Offset: 0x001B860E
		public CompProperties_RoyalImplant()
		{
			this.compClass = typeof(CompRoyalImplant);
		}

		// Token: 0x04002DAF RID: 11695
		public HediffDef implantHediff;
	}
}
