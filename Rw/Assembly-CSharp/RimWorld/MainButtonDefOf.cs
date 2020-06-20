using System;

namespace RimWorld
{
	// Token: 0x02000F63 RID: 3939
	[DefOf]
	public static class MainButtonDefOf
	{
		// Token: 0x0600606A RID: 24682 RVA: 0x00216E6A File Offset: 0x0021506A
		static MainButtonDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(MainButtonDefOf));
		}

		// Token: 0x04003748 RID: 14152
		public static MainButtonDef Inspect;

		// Token: 0x04003749 RID: 14153
		public static MainButtonDef Architect;

		// Token: 0x0400374A RID: 14154
		public static MainButtonDef Research;

		// Token: 0x0400374B RID: 14155
		public static MainButtonDef Menu;

		// Token: 0x0400374C RID: 14156
		public static MainButtonDef World;

		// Token: 0x0400374D RID: 14157
		public static MainButtonDef Quests;

		// Token: 0x0400374E RID: 14158
		public static MainButtonDef Factions;
	}
}
