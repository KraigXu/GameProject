using System;

namespace RimWorld
{
	// Token: 0x02000F89 RID: 3977
	[DefOf]
	public static class WorldObjectDefOf
	{
		// Token: 0x06006090 RID: 24720 RVA: 0x002170F0 File Offset: 0x002152F0
		static WorldObjectDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(WorldObjectDefOf));
		}

		// Token: 0x04003A14 RID: 14868
		public static WorldObjectDef Caravan;

		// Token: 0x04003A15 RID: 14869
		public static WorldObjectDef Settlement;

		// Token: 0x04003A16 RID: 14870
		public static WorldObjectDef AbandonedSettlement;

		// Token: 0x04003A17 RID: 14871
		public static WorldObjectDef EscapeShip;

		// Token: 0x04003A18 RID: 14872
		public static WorldObjectDef Ambush;

		// Token: 0x04003A19 RID: 14873
		public static WorldObjectDef DestroyedSettlement;

		// Token: 0x04003A1A RID: 14874
		public static WorldObjectDef AttackedNonPlayerCaravan;

		// Token: 0x04003A1B RID: 14875
		public static WorldObjectDef TravelingTransportPods;

		// Token: 0x04003A1C RID: 14876
		public static WorldObjectDef RoutePlannerWaypoint;

		// Token: 0x04003A1D RID: 14877
		public static WorldObjectDef Site;

		// Token: 0x04003A1E RID: 14878
		public static WorldObjectDef PeaceTalks;

		// Token: 0x04003A1F RID: 14879
		public static WorldObjectDef Debug_Arena;
	}
}
