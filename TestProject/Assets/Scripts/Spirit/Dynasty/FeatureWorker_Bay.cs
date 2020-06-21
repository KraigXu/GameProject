using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020011C7 RID: 4551
	public class FeatureWorker_Bay : FeatureWorker_Protrusion
	{
		// Token: 0x06006941 RID: 26945 RVA: 0x0024C54C File Offset: 0x0024A74C
		protected override bool IsRoot(int tile)
		{
			BiomeDef biome = Find.WorldGrid[tile].biome;
			return biome == BiomeDefOf.Ocean || biome == BiomeDefOf.Lake;
		}
	}
}
