using System;

namespace RimWorld
{
	// Token: 0x02000F99 RID: 3993
	[DefOf]
	public static class PawnTableDefOf
	{
		// Token: 0x060060A0 RID: 24736 RVA: 0x00217200 File Offset: 0x00215400
		static PawnTableDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(PawnTableDefOf));
		}

		// Token: 0x04003A58 RID: 14936
		public static PawnTableDef Work;

		// Token: 0x04003A59 RID: 14937
		public static PawnTableDef Assign;

		// Token: 0x04003A5A RID: 14938
		public static PawnTableDef Restrict;

		// Token: 0x04003A5B RID: 14939
		public static PawnTableDef Animals;

		// Token: 0x04003A5C RID: 14940
		public static PawnTableDef Wildlife;
	}
}
