using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D62 RID: 3426
	public class CompProperties_SpawnSubplant : CompProperties
	{
		// Token: 0x06005375 RID: 21365 RVA: 0x001BED06 File Offset: 0x001BCF06
		public CompProperties_SpawnSubplant()
		{
			this.compClass = typeof(CompSpawnSubplant);
		}

		// Token: 0x04002E22 RID: 11810
		public ThingDef subplant;

		// Token: 0x04002E23 RID: 11811
		public SoundDef spawnSound;
	}
}
