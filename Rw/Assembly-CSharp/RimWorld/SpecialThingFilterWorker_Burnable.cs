using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FE7 RID: 4071
	public class SpecialThingFilterWorker_Burnable : SpecialThingFilterWorker
	{
		// Token: 0x060061BD RID: 25021 RVA: 0x0021FD48 File Offset: 0x0021DF48
		public override bool Matches(Thing t)
		{
			return this.CanEverMatch(t.def) && t.BurnableByRecipe;
		}

		// Token: 0x060061BE RID: 25022 RVA: 0x0021FD60 File Offset: 0x0021DF60
		public override bool CanEverMatch(ThingDef def)
		{
			return def.burnableByRecipe;
		}

		// Token: 0x060061BF RID: 25023 RVA: 0x0021FD68 File Offset: 0x0021DF68
		public override bool AlwaysMatches(ThingDef def)
		{
			return def.burnableByRecipe && !def.MadeFromStuff;
		}
	}
}
