using System;

namespace RimWorld
{
	// Token: 0x02000F55 RID: 3925
	[DefOf]
	public static class NeedDefOf
	{
		// Token: 0x0600605C RID: 24668 RVA: 0x00216D7C File Offset: 0x00214F7C
		static NeedDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(NeedDefOf));
		}

		// Token: 0x040035DC RID: 13788
		public static NeedDef Food;

		// Token: 0x040035DD RID: 13789
		public static NeedDef Rest;

		// Token: 0x040035DE RID: 13790
		public static NeedDef Joy;
	}
}
