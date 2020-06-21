using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020011C8 RID: 4552
	public class FeatureWorker_Peninsula : FeatureWorker_Protrusion
	{
		// Token: 0x06006943 RID: 26947 RVA: 0x0024C584 File Offset: 0x0024A784
		protected override bool IsRoot(int tile)
		{
			BiomeDef biome = Find.WorldGrid[tile].biome;
			return biome != BiomeDefOf.Ocean && biome != BiomeDefOf.Lake;
		}
	}
}
