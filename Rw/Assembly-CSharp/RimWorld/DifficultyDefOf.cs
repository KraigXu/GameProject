using System;

namespace RimWorld
{
	// Token: 0x02000F70 RID: 3952
	[DefOf]
	public static class DifficultyDefOf
	{
		// Token: 0x06006077 RID: 24695 RVA: 0x00216F47 File Offset: 0x00215147
		static DifficultyDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(DifficultyDefOf));
		}

		// Token: 0x040037B3 RID: 14259
		public static DifficultyDef Easy;

		// Token: 0x040037B4 RID: 14260
		public static DifficultyDef Rough;
	}
}
