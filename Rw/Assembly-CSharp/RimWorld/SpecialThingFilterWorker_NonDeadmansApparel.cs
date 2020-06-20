using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FEE RID: 4078
	public class SpecialThingFilterWorker_NonDeadmansApparel : SpecialThingFilterWorker
	{
		// Token: 0x060061D5 RID: 25045 RVA: 0x0021FEE8 File Offset: 0x0021E0E8
		public override bool Matches(Thing t)
		{
			Apparel apparel = t as Apparel;
			return apparel != null && !apparel.WornByCorpse;
		}

		// Token: 0x060061D6 RID: 25046 RVA: 0x0021FF0A File Offset: 0x0021E10A
		public override bool CanEverMatch(ThingDef def)
		{
			return def.IsApparel && def.apparel.careIfWornByCorpse;
		}
	}
}
