using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AE0 RID: 2784
	public class CompProperties_EffectWithDest : CompProperties_AbilityEffect
	{
		// Token: 0x04002610 RID: 9744
		public AbilityEffectDestination destination;

		// Token: 0x04002611 RID: 9745
		public bool requiresLineOfSight;

		// Token: 0x04002612 RID: 9746
		public float range;

		// Token: 0x04002613 RID: 9747
		public FloatRange randomRange;

		// Token: 0x04002614 RID: 9748
		public ClamorDef destClamorType;

		// Token: 0x04002615 RID: 9749
		public int destClamorRadius;
	}
}
