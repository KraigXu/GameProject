using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000977 RID: 2423
	public static class ExtraFactionTypeExt
	{
		// Token: 0x0600395F RID: 14687 RVA: 0x00131494 File Offset: 0x0012F694
		public static string GetLabel(this ExtraFactionType factionType)
		{
			return ("ExtraFactionType_" + factionType.ToString()).Translate();
		}
	}
}
