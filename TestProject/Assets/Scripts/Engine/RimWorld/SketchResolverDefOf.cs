using System;

namespace RimWorld
{
	// Token: 0x02000FB2 RID: 4018
	[DefOf]
	public static class SketchResolverDefOf
	{
		// Token: 0x060060B9 RID: 24761 RVA: 0x002173A9 File Offset: 0x002155A9
		static SketchResolverDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(SketchResolverDefOf));
		}

		// Token: 0x04003ADE RID: 15070
		public static SketchResolverDef Monument;

		// Token: 0x04003ADF RID: 15071
		public static SketchResolverDef MonumentRuin;

		// Token: 0x04003AE0 RID: 15072
		public static SketchResolverDef Symmetry;

		// Token: 0x04003AE1 RID: 15073
		public static SketchResolverDef AssignRandomStuff;

		// Token: 0x04003AE2 RID: 15074
		public static SketchResolverDef FloorFill;

		// Token: 0x04003AE3 RID: 15075
		public static SketchResolverDef AddColumns;

		// Token: 0x04003AE4 RID: 15076
		public static SketchResolverDef AddCornerThings;

		// Token: 0x04003AE5 RID: 15077
		public static SketchResolverDef AddThingsCentral;

		// Token: 0x04003AE6 RID: 15078
		public static SketchResolverDef AddWallEdgeThings;

		// Token: 0x04003AE7 RID: 15079
		public static SketchResolverDef AddInnerMonuments;

		// Token: 0x04003AE8 RID: 15080
		public static SketchResolverDef DamageBuildings;

		// Token: 0x04003AE9 RID: 15081
		[MayRequireRoyalty]
		public static SketchResolverDef MechCluster;

		// Token: 0x04003AEA RID: 15082
		[MayRequireRoyalty]
		public static SketchResolverDef MechClusterWalls;
	}
}
