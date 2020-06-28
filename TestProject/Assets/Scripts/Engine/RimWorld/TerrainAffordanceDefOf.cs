using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FA4 RID: 4004
	[DefOf]
	public static class TerrainAffordanceDefOf
	{
		// Token: 0x060060AB RID: 24747 RVA: 0x002172BB File Offset: 0x002154BB
		static TerrainAffordanceDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(TerrainAffordanceDefOf));
		}

		// Token: 0x04003AA2 RID: 15010
		public static TerrainAffordanceDef Light;

		// Token: 0x04003AA3 RID: 15011
		public static TerrainAffordanceDef Medium;

		// Token: 0x04003AA4 RID: 15012
		public static TerrainAffordanceDef Heavy;

		// Token: 0x04003AA5 RID: 15013
		public static TerrainAffordanceDef GrowSoil;

		// Token: 0x04003AA6 RID: 15014
		public static TerrainAffordanceDef Diggable;

		// Token: 0x04003AA7 RID: 15015
		public static TerrainAffordanceDef SmoothableStone;

		// Token: 0x04003AA8 RID: 15016
		public static TerrainAffordanceDef MovingFluid;

		// Token: 0x04003AA9 RID: 15017
		public static TerrainAffordanceDef Bridgeable;
	}
}
