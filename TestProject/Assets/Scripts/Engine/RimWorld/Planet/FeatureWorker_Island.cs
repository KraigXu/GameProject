using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020011C6 RID: 4550
	public class FeatureWorker_Island : FeatureWorker_FloodFill
	{
		// Token: 0x0600693E RID: 26942 RVA: 0x0024C500 File Offset: 0x0024A700
		protected override bool IsRoot(int tile)
		{
			BiomeDef biome = Find.WorldGrid[tile].biome;
			return biome != BiomeDefOf.Ocean && biome != BiomeDefOf.Lake;
		}

		// Token: 0x0600693F RID: 26943 RVA: 0x0024C533 File Offset: 0x0024A733
		protected override bool IsPossiblyAllowed(int tile)
		{
			return Find.WorldGrid[tile].biome == BiomeDefOf.Lake;
		}
	}
}
