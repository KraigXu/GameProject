using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020008EE RID: 2286
	public class PawnTableDef : Def
	{
		// Token: 0x04001F46 RID: 8006
		public List<PawnColumnDef> columns;

		// Token: 0x04001F47 RID: 8007
		public Type workerClass = typeof(PawnTable);

		// Token: 0x04001F48 RID: 8008
		public int minWidth = 998;
	}
}
