using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F79 RID: 3961
	[DefOf]
	public static class SpecialThingFilterDefOf
	{
		// Token: 0x06006080 RID: 24704 RVA: 0x00216FE0 File Offset: 0x002151E0
		static SpecialThingFilterDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(SpecialThingFilterDefOf));
		}

		// Token: 0x04003819 RID: 14361
		public static SpecialThingFilterDef AllowFresh;

		// Token: 0x0400381A RID: 14362
		public static SpecialThingFilterDef AllowDeadmansApparel;

		// Token: 0x0400381B RID: 14363
		public static SpecialThingFilterDef AllowNonDeadmansApparel;
	}
}
