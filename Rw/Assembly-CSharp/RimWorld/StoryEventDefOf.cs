using System;

namespace RimWorld
{
	// Token: 0x02000F9F RID: 3999
	[DefOf]
	public static class StoryEventDefOf
	{
		// Token: 0x060060A6 RID: 24742 RVA: 0x00217266 File Offset: 0x00215466
		static StoryEventDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(StoryEventDefOf));
		}

		// Token: 0x04003A79 RID: 14969
		public static StoryEventDef DamageTaken;

		// Token: 0x04003A7A RID: 14970
		public static StoryEventDef DamageDealt;

		// Token: 0x04003A7B RID: 14971
		public static StoryEventDef AttackedPlayer;

		// Token: 0x04003A7C RID: 14972
		public static StoryEventDef KilledPlayer;

		// Token: 0x04003A7D RID: 14973
		public static StoryEventDef TendedByPlayer;

		// Token: 0x04003A7E RID: 14974
		public static StoryEventDef Seen;

		// Token: 0x04003A7F RID: 14975
		public static StoryEventDef TaleCreated;
	}
}
