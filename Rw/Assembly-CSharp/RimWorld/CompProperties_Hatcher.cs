using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D17 RID: 3351
	public class CompProperties_Hatcher : CompProperties
	{
		// Token: 0x06005176 RID: 20854 RVA: 0x001B4BAE File Offset: 0x001B2DAE
		public CompProperties_Hatcher()
		{
			this.compClass = typeof(CompHatcher);
		}

		// Token: 0x04002D11 RID: 11537
		public float hatcherDaystoHatch = 1f;

		// Token: 0x04002D12 RID: 11538
		public PawnKindDef hatcherPawn;
	}
}
