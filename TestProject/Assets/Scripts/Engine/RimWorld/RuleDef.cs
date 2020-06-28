using System;
using System.Collections.Generic;
using RimWorld.BaseGen;
using Verse;

namespace RimWorld
{
	// Token: 0x020008FC RID: 2300
	public class RuleDef : Def
	{
		// Token: 0x04001F88 RID: 8072
		[NoTranslate]
		public string symbol;

		// Token: 0x04001F89 RID: 8073
		public List<SymbolResolver> resolvers;
	}
}
