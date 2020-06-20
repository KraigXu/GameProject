using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D91 RID: 3473
	public class CompProperties_TargetEffect_BrainDamageChance : CompProperties
	{
		// Token: 0x06005493 RID: 21651 RVA: 0x001C357C File Offset: 0x001C177C
		public CompProperties_TargetEffect_BrainDamageChance()
		{
			this.compClass = typeof(CompTargetEffect_BrainDamageChance);
		}

		// Token: 0x04002E7C RID: 11900
		public float brainDamageChance = 0.3f;
	}
}
