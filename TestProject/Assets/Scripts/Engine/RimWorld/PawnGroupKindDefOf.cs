using System;

namespace RimWorld
{
	// Token: 0x02000F8B RID: 3979
	[DefOf]
	public static class PawnGroupKindDefOf
	{
		// Token: 0x06006092 RID: 24722 RVA: 0x00217112 File Offset: 0x00215312
		static PawnGroupKindDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(PawnGroupKindDefOf));
		}

		// Token: 0x04003A24 RID: 14884
		public static PawnGroupKindDef Combat;

		// Token: 0x04003A25 RID: 14885
		public static PawnGroupKindDef Trader;

		// Token: 0x04003A26 RID: 14886
		public static PawnGroupKindDef Peaceful;

		// Token: 0x04003A27 RID: 14887
		public static PawnGroupKindDef Settlement;
	}
}
