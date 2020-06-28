using System;

namespace RimWorld
{
	// Token: 0x02000F90 RID: 3984
	[DefOf]
	public static class RoadPathingDefOf
	{
		// Token: 0x06006097 RID: 24727 RVA: 0x00217167 File Offset: 0x00215367
		static RoadPathingDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(RoadPathingDefOf));
		}

		// Token: 0x04003A35 RID: 14901
		public static RoadPathingDef Avoid;

		// Token: 0x04003A36 RID: 14902
		public static RoadPathingDef Bulldoze;
	}
}
