using System;

namespace RimWorld
{
	// Token: 0x02000F9E RID: 3998
	[DefOf]
	public static class IncidentTargetTagDefOf
	{
		// Token: 0x060060A5 RID: 24741 RVA: 0x00217255 File Offset: 0x00215455
		static IncidentTargetTagDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(IncidentTargetTagDefOf));
		}

		// Token: 0x04003A74 RID: 14964
		public static IncidentTargetTagDef World;

		// Token: 0x04003A75 RID: 14965
		public static IncidentTargetTagDef Caravan;

		// Token: 0x04003A76 RID: 14966
		public static IncidentTargetTagDef Map_RaidBeacon;

		// Token: 0x04003A77 RID: 14967
		public static IncidentTargetTagDef Map_PlayerHome;

		// Token: 0x04003A78 RID: 14968
		public static IncidentTargetTagDef Map_Misc;
	}
}
