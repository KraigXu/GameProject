using System;

namespace RimWorld
{
	// Token: 0x02000F8D RID: 3981
	[DefOf]
	public static class SitePartDefOf
	{
		// Token: 0x06006094 RID: 24724 RVA: 0x00217134 File Offset: 0x00215334
		static SitePartDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(SitePartDefOf));
		}

		// Token: 0x04003A29 RID: 14889
		public static SitePartDef Outpost;

		// Token: 0x04003A2A RID: 14890
		public static SitePartDef Turrets;

		// Token: 0x04003A2B RID: 14891
		public static SitePartDef Manhunters;

		// Token: 0x04003A2C RID: 14892
		public static SitePartDef SleepingMechanoids;

		// Token: 0x04003A2D RID: 14893
		public static SitePartDef AmbushHidden;

		// Token: 0x04003A2E RID: 14894
		public static SitePartDef AmbushEdge;

		// Token: 0x04003A2F RID: 14895
		public static SitePartDef PreciousLump;

		// Token: 0x04003A30 RID: 14896
		public static SitePartDef PossibleUnknownThreatMarker;
	}
}
