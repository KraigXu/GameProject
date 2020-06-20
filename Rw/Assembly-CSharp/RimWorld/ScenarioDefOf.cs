using System;

namespace RimWorld
{
	// Token: 0x02000F72 RID: 3954
	[DefOf]
	public static class ScenarioDefOf
	{
		// Token: 0x06006079 RID: 24697 RVA: 0x00216F69 File Offset: 0x00215169
		static ScenarioDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ScenarioDefOf));
		}

		// Token: 0x040037B7 RID: 14263
		public static ScenarioDef Crashlanded;

		// Token: 0x040037B8 RID: 14264
		public static ScenarioDef Tutorial;
	}
}
