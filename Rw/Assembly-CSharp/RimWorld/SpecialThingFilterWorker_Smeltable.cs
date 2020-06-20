using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FE4 RID: 4068
	public class SpecialThingFilterWorker_Smeltable : SpecialThingFilterWorker
	{
		// Token: 0x060061B1 RID: 25009 RVA: 0x0021FC53 File Offset: 0x0021DE53
		public override bool Matches(Thing t)
		{
			return this.CanEverMatch(t.def) && t.Smeltable;
		}

		// Token: 0x060061B2 RID: 25010 RVA: 0x0021FC6B File Offset: 0x0021DE6B
		public override bool CanEverMatch(ThingDef def)
		{
			return def.smeltable;
		}

		// Token: 0x060061B3 RID: 25011 RVA: 0x0021FC73 File Offset: 0x0021DE73
		public override bool AlwaysMatches(ThingDef def)
		{
			return def.smeltable && !def.MadeFromStuff;
		}
	}
}
