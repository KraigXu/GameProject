using System;

namespace RimWorld
{
	// Token: 0x02000F59 RID: 3929
	[DefOf]
	public static class FactionDefOf
	{
		// Token: 0x06006060 RID: 24672 RVA: 0x00216DC0 File Offset: 0x00214FC0
		static FactionDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(FactionDefOf));
		}

		// Token: 0x0400360B RID: 13835
		public static FactionDef PlayerColony;

		// Token: 0x0400360C RID: 13836
		public static FactionDef PlayerTribe;

		// Token: 0x0400360D RID: 13837
		public static FactionDef Ancients;

		// Token: 0x0400360E RID: 13838
		public static FactionDef AncientsHostile;

		// Token: 0x0400360F RID: 13839
		public static FactionDef Mechanoid;

		// Token: 0x04003610 RID: 13840
		public static FactionDef Insect;

		// Token: 0x04003611 RID: 13841
		[MayRequireRoyalty]
		public static FactionDef Empire;
	}
}
