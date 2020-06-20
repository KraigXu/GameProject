using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FA1 RID: 4001
	[DefOf]
	public static class ImplementOwnerTypeDefOf
	{
		// Token: 0x060060A8 RID: 24744 RVA: 0x00217288 File Offset: 0x00215488
		static ImplementOwnerTypeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ImplementOwnerTypeDefOf));
		}

		// Token: 0x04003A85 RID: 14981
		public static ImplementOwnerTypeDef Weapon;

		// Token: 0x04003A86 RID: 14982
		public static ImplementOwnerTypeDef Bodypart;

		// Token: 0x04003A87 RID: 14983
		public static ImplementOwnerTypeDef Hediff;

		// Token: 0x04003A88 RID: 14984
		public static ImplementOwnerTypeDef Terrain;

		// Token: 0x04003A89 RID: 14985
		public static ImplementOwnerTypeDef NativeVerb;
	}
}
