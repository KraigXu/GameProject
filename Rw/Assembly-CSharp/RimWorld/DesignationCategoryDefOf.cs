using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F52 RID: 3922
	[DefOf]
	public static class DesignationCategoryDefOf
	{
		// Token: 0x0600605A RID: 24666 RVA: 0x00216D5A File Offset: 0x00214F5A
		static DesignationCategoryDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(DesignationCategoryDefOf));
		}

		// Token: 0x040035BA RID: 13754
		public static DesignationCategoryDef Production;

		// Token: 0x040035BB RID: 13755
		public static DesignationCategoryDef Structure;

		// Token: 0x040035BC RID: 13756
		public static DesignationCategoryDef Security;

		// Token: 0x040035BD RID: 13757
		public static DesignationCategoryDef Floors;
	}
}
