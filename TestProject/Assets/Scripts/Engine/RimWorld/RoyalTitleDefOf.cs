using System;

namespace RimWorld
{
	// Token: 0x02000FB3 RID: 4019
	[DefOf]
	public static class RoyalTitleDefOf
	{
		// Token: 0x060060BA RID: 24762 RVA: 0x002173BA File Offset: 0x002155BA
		static RoyalTitleDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(RoyalTitleDefOf));
		}

		// Token: 0x04003AEB RID: 15083
		[MayRequireRoyalty]
		public static RoyalTitleDef Knight;

		// Token: 0x04003AEC RID: 15084
		[MayRequireRoyalty]
		public static RoyalTitleDef Count;
	}
}
