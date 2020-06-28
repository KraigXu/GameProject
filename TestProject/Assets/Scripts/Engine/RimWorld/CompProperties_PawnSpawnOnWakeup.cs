using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D34 RID: 3380
	public class CompProperties_PawnSpawnOnWakeup : CompProperties
	{
		// Token: 0x0600521D RID: 21021 RVA: 0x001B6E46 File Offset: 0x001B5046
		public CompProperties_PawnSpawnOnWakeup()
		{
			this.compClass = typeof(CompPawnSpawnOnWakeup);
		}

		// Token: 0x04002D46 RID: 11590
		public List<PawnKindDef> spawnablePawnKinds;

		// Token: 0x04002D47 RID: 11591
		public SoundDef spawnSound;

		// Token: 0x04002D48 RID: 11592
		public EffecterDef spawnEffecter;

		// Token: 0x04002D49 RID: 11593
		public Type lordJob;

		// Token: 0x04002D4A RID: 11594
		public bool shouldJoinParentLord;

		// Token: 0x04002D4B RID: 11595
		public string activatedMessageKey;

		// Token: 0x04002D4C RID: 11596
		public FloatRange points;

		// Token: 0x04002D4D RID: 11597
		public IntRange pawnSpawnRadius = new IntRange(2, 2);

		// Token: 0x04002D4E RID: 11598
		public bool aggressive = true;

		// Token: 0x04002D4F RID: 11599
		public bool dropInPods;

		// Token: 0x04002D50 RID: 11600
		public float defendRadius = 21f;
	}
}
