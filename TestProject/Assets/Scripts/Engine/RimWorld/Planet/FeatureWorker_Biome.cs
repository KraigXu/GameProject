using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020011C4 RID: 4548
	public class FeatureWorker_Biome : FeatureWorker_FloodFill
	{
		// Token: 0x0600692F RID: 26927 RVA: 0x0024BF32 File Offset: 0x0024A132
		protected override bool IsRoot(int tile)
		{
			return this.def.rootBiomes.Contains(Find.WorldGrid[tile].biome);
		}

		// Token: 0x06006930 RID: 26928 RVA: 0x0024BF54 File Offset: 0x0024A154
		protected override bool IsPossiblyAllowed(int tile)
		{
			return this.def.acceptableBiomes.Contains(Find.WorldGrid[tile].biome);
		}
	}
}
