using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020011C0 RID: 4544
	public class FeatureWorker_Archipelago : FeatureWorker_Cluster
	{
		// Token: 0x06006915 RID: 26901 RVA: 0x0024B7B4 File Offset: 0x002499B4
		protected override bool IsRoot(int tile)
		{
			BiomeDef biome = Find.WorldGrid[tile].biome;
			return biome != BiomeDefOf.Ocean && biome != BiomeDefOf.Lake;
		}

		// Token: 0x06006916 RID: 26902 RVA: 0x0024B7E7 File Offset: 0x002499E7
		protected override bool CanTraverse(int tile, out bool ifRootThenRootGroupSizeMustMatch)
		{
			ifRootThenRootGroupSizeMustMatch = true;
			return true;
		}

		// Token: 0x06006917 RID: 26903 RVA: 0x0024B7F0 File Offset: 0x002499F0
		protected override bool IsMember(int tile, out bool ifRootThenRootGroupSizeMustMatch)
		{
			ifRootThenRootGroupSizeMustMatch = true;
			bool flag;
			return base.IsMember(tile, out flag);
		}
	}
}
