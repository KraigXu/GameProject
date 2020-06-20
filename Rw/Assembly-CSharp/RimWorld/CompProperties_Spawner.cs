using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D57 RID: 3415
	public class CompProperties_Spawner : CompProperties
	{
		// Token: 0x06005325 RID: 21285 RVA: 0x001BD1E7 File Offset: 0x001BB3E7
		public CompProperties_Spawner()
		{
			this.compClass = typeof(CompSpawner);
		}

		// Token: 0x04002DE6 RID: 11750
		public ThingDef thingToSpawn;

		// Token: 0x04002DE7 RID: 11751
		public int spawnCount = 1;

		// Token: 0x04002DE8 RID: 11752
		public IntRange spawnIntervalRange = new IntRange(100, 100);

		// Token: 0x04002DE9 RID: 11753
		public int spawnMaxAdjacent = -1;

		// Token: 0x04002DEA RID: 11754
		public bool spawnForbidden;

		// Token: 0x04002DEB RID: 11755
		public bool requiresPower;

		// Token: 0x04002DEC RID: 11756
		public bool writeTimeLeftToSpawn;

		// Token: 0x04002DED RID: 11757
		public bool showMessageIfOwned;

		// Token: 0x04002DEE RID: 11758
		public string saveKeysPrefix;

		// Token: 0x04002DEF RID: 11759
		public bool inheritFaction;
	}
}
