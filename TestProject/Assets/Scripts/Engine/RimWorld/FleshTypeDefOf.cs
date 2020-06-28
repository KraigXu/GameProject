using System;

namespace RimWorld
{
	// Token: 0x02000F92 RID: 3986
	[DefOf]
	public static class FleshTypeDefOf
	{
		// Token: 0x06006099 RID: 24729 RVA: 0x00217189 File Offset: 0x00215389
		static FleshTypeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(FleshTypeDefOf));
		}

		// Token: 0x04003A43 RID: 14915
		public static FleshTypeDef Normal;

		// Token: 0x04003A44 RID: 14916
		public static FleshTypeDef Mechanoid;

		// Token: 0x04003A45 RID: 14917
		public static FleshTypeDef Insectoid;
	}
}
