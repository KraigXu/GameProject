using System;
using RimWorld.Planet;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02000E61 RID: 3681
	public static class NamePlayerSettlementDialogUtility
	{
		// Token: 0x0600593B RID: 22843 RVA: 0x001DC931 File Offset: 0x001DAB31
		public static bool IsValidName(string s)
		{
			return s.Length != 0 && s.Length <= 64 && !GrammarResolver.ContainsSpecialChars(s);
		}

		// Token: 0x0600593C RID: 22844 RVA: 0x001DC954 File Offset: 0x001DAB54
		public static void Named(Settlement factionBase, string s)
		{
			factionBase.Name = s;
			factionBase.namedByPlayer = true;
		}
	}
}
