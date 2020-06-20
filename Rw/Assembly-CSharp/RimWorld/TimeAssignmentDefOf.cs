using System;

namespace RimWorld
{
	// Token: 0x02000F74 RID: 3956
	[DefOf]
	public static class TimeAssignmentDefOf
	{
		// Token: 0x0600607B RID: 24699 RVA: 0x00216F8B File Offset: 0x0021518B
		static TimeAssignmentDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(TimeAssignmentDefOf));
		}

		// Token: 0x040037BB RID: 14267
		public static TimeAssignmentDef Anything;

		// Token: 0x040037BC RID: 14268
		public static TimeAssignmentDef Work;

		// Token: 0x040037BD RID: 14269
		public static TimeAssignmentDef Joy;

		// Token: 0x040037BE RID: 14270
		public static TimeAssignmentDef Sleep;

		// Token: 0x040037BF RID: 14271
		[MayRequireRoyalty]
		public static TimeAssignmentDef Meditate;
	}
}
