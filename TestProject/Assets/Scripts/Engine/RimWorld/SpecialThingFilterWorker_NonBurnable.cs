using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FE8 RID: 4072
	public class SpecialThingFilterWorker_NonBurnable : SpecialThingFilterWorker
	{
		// Token: 0x060061C1 RID: 25025 RVA: 0x0021FD7D File Offset: 0x0021DF7D
		public override bool Matches(Thing t)
		{
			return this.CanEverMatch(t.def) && !t.BurnableByRecipe;
		}

		// Token: 0x060061C2 RID: 25026 RVA: 0x0021FD98 File Offset: 0x0021DF98
		public override bool CanEverMatch(ThingDef def)
		{
			return !def.burnableByRecipe || def.MadeFromStuff;
		}

		// Token: 0x060061C3 RID: 25027 RVA: 0x0021FDAA File Offset: 0x0021DFAA
		public override bool AlwaysMatches(ThingDef def)
		{
			return !def.burnableByRecipe && !def.MadeFromStuff;
		}
	}
}
