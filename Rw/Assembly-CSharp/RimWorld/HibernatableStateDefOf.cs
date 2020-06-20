using System;

namespace RimWorld
{
	// Token: 0x02000F9C RID: 3996
	[DefOf]
	public static class HibernatableStateDefOf
	{
		// Token: 0x060060A3 RID: 24739 RVA: 0x00217233 File Offset: 0x00215433
		static HibernatableStateDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(HibernatableStateDefOf));
		}

		// Token: 0x04003A70 RID: 14960
		public static HibernatableStateDef Running;

		// Token: 0x04003A71 RID: 14961
		public static HibernatableStateDef Starting;

		// Token: 0x04003A72 RID: 14962
		public static HibernatableStateDef Hibernating;
	}
}
