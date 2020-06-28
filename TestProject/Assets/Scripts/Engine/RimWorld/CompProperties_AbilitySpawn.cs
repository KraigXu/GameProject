using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AD2 RID: 2770
	public class CompProperties_AbilitySpawn : CompProperties_AbilityEffect
	{
		// Token: 0x0600419C RID: 16796 RVA: 0x0015EE08 File Offset: 0x0015D008
		public CompProperties_AbilitySpawn()
		{
			this.compClass = typeof(CompAbilityEffect_Spawn);
		}

		// Token: 0x04002607 RID: 9735
		public ThingDef thingDef;
	}
}
