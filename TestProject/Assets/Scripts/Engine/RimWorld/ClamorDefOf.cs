using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FA9 RID: 4009
	[DefOf]
	public static class ClamorDefOf
	{
		// Token: 0x060060B0 RID: 24752 RVA: 0x00217310 File Offset: 0x00215510
		static ClamorDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ClamorDefOf));
		}

		// Token: 0x04003AC1 RID: 15041
		public static ClamorDef Movement;

		// Token: 0x04003AC2 RID: 15042
		public static ClamorDef Harm;

		// Token: 0x04003AC3 RID: 15043
		public static ClamorDef Construction;

		// Token: 0x04003AC4 RID: 15044
		public static ClamorDef Impact;

		// Token: 0x04003AC5 RID: 15045
		public static ClamorDef Ability;
	}
}
