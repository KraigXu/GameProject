using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D14 RID: 3348
	public class CompProperties_GiveThoughtToAllMapPawnsOnDestroy : CompProperties
	{
		// Token: 0x06005167 RID: 20839 RVA: 0x001B490F File Offset: 0x001B2B0F
		public CompProperties_GiveThoughtToAllMapPawnsOnDestroy()
		{
			this.compClass = typeof(CompGiveThoughtToAllMapPawnsOnDestroy);
		}

		// Token: 0x04002D0E RID: 11534
		public ThoughtDef thought;

		// Token: 0x04002D0F RID: 11535
		[MustTranslate]
		public string message;
	}
}
