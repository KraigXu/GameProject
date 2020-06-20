using System;

namespace RimWorld
{
	// Token: 0x02000F8F RID: 3983
	[DefOf]
	public static class RoadDefOf
	{
		// Token: 0x06006096 RID: 24726 RVA: 0x00217156 File Offset: 0x00215356
		static RoadDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(RoadDefOf));
		}

		// Token: 0x04003A32 RID: 14898
		public static RoadDef DirtRoad;

		// Token: 0x04003A33 RID: 14899
		public static RoadDef AncientAsphaltRoad;

		// Token: 0x04003A34 RID: 14900
		public static RoadDef AncientAsphaltHighway;
	}
}
