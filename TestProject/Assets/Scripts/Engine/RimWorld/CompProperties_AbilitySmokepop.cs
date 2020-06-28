using System;

namespace RimWorld
{
	// Token: 0x02000AD0 RID: 2768
	public class CompProperties_AbilitySmokepop : CompProperties_AbilityEffect
	{
		// Token: 0x06004197 RID: 16791 RVA: 0x0015ED5B File Offset: 0x0015CF5B
		public CompProperties_AbilitySmokepop()
		{
			this.compClass = typeof(CompAbilityEffect_Smokepop);
		}

		// Token: 0x04002606 RID: 9734
		public float smokeRadius;
	}
}
