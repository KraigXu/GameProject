using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FEF RID: 4079
	public class SpecialThingFilterWorker_DeadmansApparel : SpecialThingFilterWorker
	{
		// Token: 0x060061D8 RID: 25048 RVA: 0x0021FF24 File Offset: 0x0021E124
		public override bool Matches(Thing t)
		{
			Apparel apparel = t as Apparel;
			return apparel != null && apparel.WornByCorpse;
		}

		// Token: 0x060061D9 RID: 25049 RVA: 0x0021FF0A File Offset: 0x0021E10A
		public override bool CanEverMatch(ThingDef def)
		{
			return def.IsApparel && def.apparel.careIfWornByCorpse;
		}
	}
}
