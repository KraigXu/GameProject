using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FE5 RID: 4069
	public class SpecialThingFilterWorker_NonSmeltable : SpecialThingFilterWorker
	{
		// Token: 0x060061B5 RID: 25013 RVA: 0x0021FC88 File Offset: 0x0021DE88
		public override bool Matches(Thing t)
		{
			return this.CanEverMatch(t.def) && !t.Smeltable;
		}

		// Token: 0x060061B6 RID: 25014 RVA: 0x0021FCA3 File Offset: 0x0021DEA3
		public override bool CanEverMatch(ThingDef def)
		{
			return !def.smeltable || def.MadeFromStuff;
		}

		// Token: 0x060061B7 RID: 25015 RVA: 0x0021FCB5 File Offset: 0x0021DEB5
		public override bool AlwaysMatches(ThingDef def)
		{
			return !def.smeltable && !def.MadeFromStuff;
		}
	}
}
