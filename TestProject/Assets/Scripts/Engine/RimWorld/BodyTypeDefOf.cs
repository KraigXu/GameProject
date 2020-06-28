using System;

namespace RimWorld
{
	// Token: 0x02000FAB RID: 4011
	[DefOf]
	public static class BodyTypeDefOf
	{
		// Token: 0x060060B2 RID: 24754 RVA: 0x00217332 File Offset: 0x00215532
		static BodyTypeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(BodyTypeDef));
		}

		// Token: 0x04003AC7 RID: 15047
		public static BodyTypeDef Male;

		// Token: 0x04003AC8 RID: 15048
		public static BodyTypeDef Female;

		// Token: 0x04003AC9 RID: 15049
		public static BodyTypeDef Thin;

		// Token: 0x04003ACA RID: 15050
		public static BodyTypeDef Hulk;

		// Token: 0x04003ACB RID: 15051
		public static BodyTypeDef Fat;
	}
}
