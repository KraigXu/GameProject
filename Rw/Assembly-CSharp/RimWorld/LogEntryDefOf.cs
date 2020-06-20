using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FAC RID: 4012
	[DefOf]
	public static class LogEntryDefOf
	{
		// Token: 0x060060B3 RID: 24755 RVA: 0x00217343 File Offset: 0x00215543
		static LogEntryDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(LogEntryDef));
		}

		// Token: 0x04003ACC RID: 15052
		public static LogEntryDef MeleeAttack;
	}
}
