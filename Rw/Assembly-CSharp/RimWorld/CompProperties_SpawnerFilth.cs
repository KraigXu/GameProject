using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D5A RID: 3418
	public class CompProperties_SpawnerFilth : CompProperties
	{
		// Token: 0x0600533F RID: 21311 RVA: 0x001BD98F File Offset: 0x001BBB8F
		public CompProperties_SpawnerFilth()
		{
			this.compClass = typeof(CompSpawnerFilth);
		}

		// Token: 0x04002DF2 RID: 11762
		public ThingDef filthDef;

		// Token: 0x04002DF3 RID: 11763
		public int spawnCountOnSpawn = 5;

		// Token: 0x04002DF4 RID: 11764
		public float spawnMtbHours = 12f;

		// Token: 0x04002DF5 RID: 11765
		public float spawnRadius = 3f;

		// Token: 0x04002DF6 RID: 11766
		public float spawnEveryDays = -1f;

		// Token: 0x04002DF7 RID: 11767
		public RotStage? requiredRotStage;
	}
}
