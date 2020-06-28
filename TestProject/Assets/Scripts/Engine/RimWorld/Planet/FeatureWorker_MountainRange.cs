using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020011C2 RID: 4546
	public class FeatureWorker_MountainRange : FeatureWorker_Cluster
	{
		// Token: 0x06006928 RID: 26920 RVA: 0x0024BD74 File Offset: 0x00249F74
		protected override bool IsRoot(int tile)
		{
			return Find.WorldGrid[tile].hilliness != Hilliness.Flat;
		}

		// Token: 0x06006929 RID: 26921 RVA: 0x0024BD8C File Offset: 0x00249F8C
		protected override bool CanTraverse(int tile, out bool ifRootThenRootGroupSizeMustMatch)
		{
			ifRootThenRootGroupSizeMustMatch = false;
			return Find.WorldGrid[tile].biome != BiomeDefOf.Ocean;
		}
	}
}
