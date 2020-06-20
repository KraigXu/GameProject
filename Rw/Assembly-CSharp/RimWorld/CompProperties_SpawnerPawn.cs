using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D60 RID: 3424
	public class CompProperties_SpawnerPawn : CompProperties
	{
		// Token: 0x0600535C RID: 21340 RVA: 0x001BE28C File Offset: 0x001BC48C
		public CompProperties_SpawnerPawn()
		{
			this.compClass = typeof(CompSpawnerPawn);
		}

		// Token: 0x04002E09 RID: 11785
		public List<PawnKindDef> spawnablePawnKinds;

		// Token: 0x04002E0A RID: 11786
		public SoundDef spawnSound;

		// Token: 0x04002E0B RID: 11787
		public string spawnMessageKey;

		// Token: 0x04002E0C RID: 11788
		public string noPawnsLeftToSpawnKey;

		// Token: 0x04002E0D RID: 11789
		public string pawnsLeftToSpawnKey;

		// Token: 0x04002E0E RID: 11790
		public bool showNextSpawnInInspect;

		// Token: 0x04002E0F RID: 11791
		public bool shouldJoinParentLord;

		// Token: 0x04002E10 RID: 11792
		public Type lordJob;

		// Token: 0x04002E11 RID: 11793
		public float defendRadius = 21f;

		// Token: 0x04002E12 RID: 11794
		public int initialPawnsCount;

		// Token: 0x04002E13 RID: 11795
		public float initialPawnsPoints;

		// Token: 0x04002E14 RID: 11796
		public float maxSpawnedPawnsPoints = -1f;

		// Token: 0x04002E15 RID: 11797
		public FloatRange pawnSpawnIntervalDays = new FloatRange(0.85f, 1.15f);

		// Token: 0x04002E16 RID: 11798
		public int pawnSpawnRadius = 2;

		// Token: 0x04002E17 RID: 11799
		public IntRange maxPawnsToSpawn = IntRange.zero;

		// Token: 0x04002E18 RID: 11800
		public bool chooseSingleTypeToSpawn;

		// Token: 0x04002E19 RID: 11801
		public string nextSpawnInspectStringKey;

		// Token: 0x04002E1A RID: 11802
		public string nextSpawnInspectStringKeyDormant;
	}
}
